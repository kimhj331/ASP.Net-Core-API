using AutoMapper;
using ButlerLee.API.Contracts;
using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Entities;
using ButlerLee.API.LoggerService;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace ButlerLee.API.Services
{
    public class KakaoPaymentService : IKakaoPaymentService
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        private IClientWrapper _clinet;
        private Configurations _configurations;
        private ILoggerManager _logger;

        public KakaoPaymentService(IRepositoryWrapper repository, IClientWrapper clinet, 
           Configurations configurations, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _clinet = clinet;
            _configurations = configurations;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<KakaoPayReadyResponse> Ready(Reservation reservation)
        {
            var listingResponse = await _clinet.Listing.GetListingById(reservation.ListingId);
            string listingName = listingResponse.Result.Name;

            KakaoPayReadyResponse readyResponse = 
                await RequestReady(reservation.Id, (int)reservation.TotalPrice, (int)reservation.Nights, listingName);

            // TID 저장 
            Entities.Payment payment = new Entities.Payment();
            payment.PaymentGateway = PaymentGateway.KakaoPay.ToString();
            payment.ReservationId = reservation.Id;
            payment.PaymentKey = readyResponse.Tid;
            payment.UpdateDate = DateTime.UtcNow;
            await _repository.Payment.UpsertPayment(payment);
                
            //결제 준비 요청
            return readyResponse;
        }

        public async Task<KakaoPayReadyResponse> RequestReady(int reservationId, int totalPrice, int nights, string listingName)
        {
            KakaoPayReadyRequest requestData = new KakaoPayReadyRequest();

            //requestData.CidSecret = "";         //가맹점 코드 인증키
            requestData.PartnerOrderId = $"{reservationId}_{DateTime.UtcNow.ToString("yyMMddHHmmss")}";    // 가맹점 주문번호
            requestData.PartnerUserId = _configurations.UserId;     //가맹점 회원 Id                                                                            
            requestData.ItemName = $"{listingName} ({nights}박)";  //상품명
            requestData.ItemCode = reservationId.ToString(); //상품코드
            requestData.Quantity = nights;  // 상품수량
            requestData.TotalAmount = totalPrice; //(int)originReservation.TotalPrice;    //Math.Ceiling(reservation.TotalPrice);  //상품총액-> 소수점 어떻게 처리할것인지
            requestData.TaxFreeAmount = 0;  //상품 비과세 금액
            //requestData.VatAmount = 0;    //값을 보내지 않을 경우 다음과 같이 VAT 자동 계산 (상품총액 - 상품 비과세 금액)/ 11 : 소숫점 이하 반올림

            KakaoPayApprovalParameters approvalParameters = new KakaoPayApprovalParameters();
            approvalParameters.PartnerOrderId = requestData.PartnerOrderId;
            approvalParameters.PartnerUserId = requestData.PartnerUserId;
            approvalParameters.ReservationId = reservationId;

            string queryString = approvalParameters.GetQueryString();
            requestData.ApprovalUrl = $"{_configurations.ApplicationUrl}/modal/payment/kakao/approval?{queryString}";   //결제 성공 시 redirect url
            requestData.CancelUrl = $"{_configurations.ApplicationUrl}/modal/payment/kakao/cancel?{queryString}";     //결제 취소시 redirect url
            requestData.FailUrl = $"{_configurations.ApplicationUrl}/modal/payment/kakao/fail?{queryString}";       //결제 실패시 redirect url
            requestData.PaymentMethodType = PaymentType.Card;   //지정하지 않으면 모든 결제 수단 허용 CARD 또는 MONEY 중 하나
            //requestData.InstallMonth = 10; //카드 할부개월

            KakaoPayReadyResponse readyResponse = await _clinet.KakaoPay.Ready(requestData);

            //ready 요청 실패로 Error Message가 전달된 경우 
            if (string.IsNullOrEmpty(readyResponse.Code) == false)
                throw new PaymentException(HttpStatusCode.Conflict, readyResponse.Message);

            return readyResponse;
        }

        public async Task<Models.Payment> Approval(KakaoPayApprovalParameters parameters, Reservation reservation)
        {
            int reservationId = parameters.ReservationId;

            Entities.Payment payment =
              await _repository.Payment.GetPaymentByReservationId(parameters.ReservationId);

            if (payment == null)
                throw new ConflictException(ErrorCodes.PAYMENT_NOT_FOUND); 

            //승인요청
            KakaoPaymentResponse responsePayment = await RequestApproval(parameters, payment.PaymentKey);

            //2. 승인 성공
            //2-1. unpaid reservation 삭제
            await _repository.UnpaidReservation.Delete(reservationId);
            //고객에게 제공할 reservation No
            string reservationNo = await _repository.Payment.GenerateReservationNo();

            try
            {
                await _clinet.Reservation.UpdateReservation(reservationId, hostNote: $"Kakao Pay: {responsePayment.Tid}");
            }
            catch (Exception) { }

            responsePayment.ReservationId = reservationId;
            //HostAway IsPaid 정상 업데이트 여부,
            //예약건 취소 여부 판단. 이상있을경우 Exception 반환
            await CheckPaidReservation(responsePayment);

            Entities.Payment paymentDao = _mapper.Map<Entities.Payment>(responsePayment);

            //2-3. DB Payment 생성
            paymentDao.PaymentGateway = PaymentGateway.KakaoPay.ToString();
            paymentDao.Id = payment.Id;
            paymentDao.ListingId = reservation.ListingId;
            paymentDao.ArrivalDate = reservation.ArrivalDate;
            paymentDao.DepartureDate = reservation.DepartureDate;
            //paymentDao.ReservationId = reservationId;
            paymentDao.ReservationNo = reservationNo;
            paymentDao.UpdateDate = DateTime.UtcNow;

            this._repository.Payment.Update(paymentDao, payment.Id);

            await _repository.SaveChangesAsync();

            var response =
               await this._repository.Payment.GetPaymentByReservationId(reservationId);

            return _mapper.Map<Entities.Payment, Models.Payment>(response);
        }

        /// <summary>
        /// 카카오 API에 승인 요청
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        private async Task<Models.KakaoPaymentResponse> RequestApproval(KakaoPayApprovalParameters parameters, string pyamentKey)
        {
            //1. 승인 요청
            Models.KakaoPaymentResponse requestData = new Models.KakaoPaymentResponse();
            requestData.Tid = pyamentKey;            //결제 고유번호, 결제 준비 API 응답에 포함
            requestData.PartnerOrderId = parameters.PartnerOrderId; //가맹점 주문번호, 결제 준비 API 요청과 일치해야 함
            requestData.PartnerUserId = parameters.PartnerUserId;  //가맹점 회원 id, 결제 준비 API요청과 일치해야함
            requestData.PgToken = parameters.PgToken;        //결제승인 요청을 인증하는 토큰  //사용자 결제 수단 선택 완료 시, approval_url로 redirection해줄 때 pg_token을 query string으로 전달
            //requestData.Payload = "";        //결제 승인 요청에 대해 저장하고 싶은 값, 최대 200자

            KakaoPaymentResponse responsePayment = await _clinet.KakaoPay.Approval(requestData);

            //승인 실패로 Error Message가 전달된 경우 
            if (string.IsNullOrEmpty(responsePayment.Message) == false)
                throw new PaymentException(HttpStatusCode.Conflict, responsePayment.Message);

            return responsePayment;
        }

        private async Task CheckPaidReservation(KakaoPaymentResponse kakaoPayment)
        {
            var response = 
                await _clinet.Reservation.GetReservationById((int)kakaoPayment.ReservationId);

            if (response.Result == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            Reservation onriginReservation = response.Result;

            if (onriginReservation.IsPaid != 1 //IsPaid 상태가 제대로 업데이트 되지 않았거나
                || onriginReservation.Status == ReservationStatus.Cancelled) //혹시라도 '결제 완료' <-> 'Delete unpaid reservation' 사이에 결제가 취소 된 경우
            {
                PaymentCancelParameters cancelParameters = new PaymentCancelParameters
                {
                    PaymentKey = kakaoPayment.Tid,
                    CancelAmount = (int)kakaoPayment.Amount.Total,
                    CancelTaxFreeAmount = (int)kakaoPayment.Amount.TaxFree
                };

                //스케줄러 데이터 기입
                //취소 시간(CancelDate) 5분뒤에 Hostaway예약 삭제
                UnpaidReservation reservationToDelete = new UnpaidReservation
                {
                    ReservationId = (int)kakaoPayment.ReservationId,
                    LimitStartDate = DateTime.UtcNow.AddMinutes(-5),
                    CancelDate = DateTime.UtcNow
                };

                await this.Cancel(cancelParameters, (int)kakaoPayment.ReservationId);

                _repository.UnpaidReservation.Create(reservationToDelete);
                await _repository.SaveChangesAsync();

                throw new ConflictException(ErrorCodes.TIME_OUT_FOR_PAYMENT_REQUEST);
            }
        }

        public async Task<Models.Payment> Cancel(PaymentCancelParameters parameters, int reservationId)
        {
            KakaoCanelRequest canelRequest = new KakaoCanelRequest
            {
                Tid = parameters.PaymentKey,
                CancelAmount = parameters.CancelAmount,
                CancelTaxFreeAmount = parameters.CancelTaxFreeAmount
            };
            
            KakaoPaymentResponse response = await _clinet.KakaoPay.Cancel(canelRequest);

            //결제 취소 성공 여부
            if (string.IsNullOrEmpty(response.Code) == false)
                throw new PaymentException(HttpStatusCode.Conflict, response.Message);

            Entities.Payment originPayment =
                await _repository.Payment.GetPaymentByReservationId((int)reservationId);

            //Db에 저장된 paymet가 없는 경우 Response를 return
            if (originPayment == null)
                return _mapper.Map<Models.Payment>(response);

            Entities.Payment canceledPayment = _mapper.Map<Entities.Payment>(response);

            //Db에 저장된 paymet가 있는경우 Update
            originPayment.CanceledAmount = canceledPayment.CanceledAmount;
            originPayment.CancelDate = canceledPayment.CancelDate;
            originPayment.UpdateDate = DateTime.UtcNow;

            _repository.Payment.Update(originPayment, originPayment.Id);
            await _repository.SaveChangesAsync();

            Entities.Payment updatedPayment = await _repository.Payment.GetPaymentByReservationId(reservationId);

            return _mapper.Map<Models.Payment>(updatedPayment);
        }
    }
}
