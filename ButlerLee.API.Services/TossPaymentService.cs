using AutoMapper;
using ButlerLee.API.Clients;
using ButlerLee.API.Contracts;
using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Entities;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ButlerLee.API.Services
{
    public class TossPaymentService : ITossPaymentService
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        private IClientWrapper _clinet;
        private ILoggerManager _logger;

        public TossPaymentService(IRepositoryWrapper repository, IClientWrapper client, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _clinet = client;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Models.Payment> Approval(TossPayApprovalParameters parameters, Reservation reservation)
        {
            //승인 요청
            TossPaymentResponse responsePayment = await _clinet.TossPay.Approval(parameters);

            //승인 실패일경우 
            if (string.IsNullOrEmpty(responsePayment.ErrorCode) == false)
                throw new PaymentException(HttpStatusCode.Conflict, responsePayment.ErrorMessage);

            //승인 성공일경우
            //unpaid reservation 삭제
            await _repository.UnpaidReservation.Delete(reservation.Id);

            //고객에게 제공할 reservation No
            string reservationNo = await _repository.Payment.GenerateReservationNo();

            //hostaway ispaid -> true
            try
            {
                await _clinet.Reservation
                    .UpdateReservation((int)parameters.ReservationId, hostNote: $"TossPay : {responsePayment.PaymentKey}");
            }
            catch (Exception)
            { }

            await CheckPaidReservation(responsePayment, (int)parameters.ReservationId);

            //혹시라도 '결제 완료' <-> 'Delete unpaid reservation' 사이에 결제가 취소 된 경우

            //HostAway IsPaid 정상 업데이트 여부,
            //예약건 취소 여부 판단. 이상있을경우 Exception 반환
            Entities.Payment paymentDto = _mapper.Map<Entities.Payment>(responsePayment);

            //payment data 생성
            paymentDto.PaymentGateway = PaymentGateway.TossPay.ToString();
            paymentDto.ListingId = reservation.ListingId;
            paymentDto.ArrivalDate = reservation.ArrivalDate;
            paymentDto.DepartureDate = reservation.DepartureDate;
            paymentDto.ReservationId = (int)parameters.ReservationId;
            paymentDto.ReservationNo = reservationNo;
            paymentDto.OrderName = $"{paymentDto.OrderName} ({reservation.Nights}박)";
            paymentDto.UpdateDate = DateTime.UtcNow;

            await this._repository.Payment.UpsertPayment(paymentDto);

            var createdPayment =
               await this._repository.Payment.GetPaymentByReservationId((int)parameters.ReservationId);
            
            return _mapper.Map<Entities.Payment, Models.Payment>(createdPayment );
        }

        private async Task CheckPaidReservation(TossPaymentResponse responsePayment, int reservationId)
        {
            var response = await _clinet.Reservation.GetReservationById(reservationId);

            if (response.Result == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            Reservation originReservation = response.Result;

            if (originReservation.Status == ReservationStatus.Cancelled)
            {
                PaymentCancelParameters cancelParameters = new PaymentCancelParameters
                {
                    PaymentKey = responsePayment.PaymentKey,
                    CancelAmount = (int)responsePayment.TotalAmount,
                    CancelTaxFreeAmount = (int)responsePayment.TaxFreeAmount,
                    CancelReason = "결제실패"
                };
                await this.Cancel(cancelParameters, reservationId);

                //스케줄러 데이터 기입
                //취소 시간(CancelDate) 5분뒤에 Hostaway예약 삭제
                UnpaidReservation reservationToDelete = new UnpaidReservation
                {
                    ReservationId = reservationId,
                    LimitStartDate = DateTime.UtcNow.AddMinutes(-5),
                    CancelDate = DateTime.UtcNow
                };
                _repository.UnpaidReservation.Create(reservationToDelete);
                await _repository.SaveChangesAsync();

                throw new ConflictException(ErrorCodes.TIME_OUT_FOR_PAYMENT_REQUEST);
            }
        }

        public async Task<Models.Payment> Cancel(PaymentCancelParameters parameters, int reservationId)
        {
            if (string.IsNullOrEmpty(parameters.CancelReason))
                throw new BadRequestException(ErrorCodes.CANCEL_REASON_IS_NULL);

            TossCancel cancel = new TossCancel
            {
                PaymentKey = parameters.PaymentKey,
                CancelAmount = parameters.CancelAmount,
                TaxFreeAmount = parameters.CancelTaxFreeAmount,
                CancelReason = parameters.CancelReason
            };

            var response = await _clinet.TossPay.Cancel(cancel);

            //에러 메세지 전달시
            if (string.IsNullOrEmpty(response.ErrorCode) == false)
                throw new PaymentException(HttpStatusCode.Conflict, response.ErrorMessage);

            Entities.Payment originPayment = await _repository.Payment.GetPaymentByReservationId(reservationId);

            //Db에 저장된 paymet가 없는 경우 Response를 return
            if (originPayment == null)
                return _mapper.Map<Models.Payment>(response);

            Entities.Payment canceledPayment = _mapper.Map<Entities.Payment>(response);

            originPayment.CanceledAmount = canceledPayment.CanceledAmount;
            originPayment.CancelDate = canceledPayment.CancelDate;
            originPayment.UpdateDate = DateTime.UtcNow;

            _repository.Payment.Update(originPayment, originPayment.Id);
            await _repository.SaveChangesAsync();

            Entities.Payment result = 
                await _repository.Payment.GetPaymentByReservationId(reservationId);

            return _mapper.Map<Models.Payment>(result);
        }
    }
}
