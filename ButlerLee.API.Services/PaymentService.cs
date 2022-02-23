using AutoMapper;
using ButlerLee.API.Clients;
using ButlerLee.API.Contracts;
using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Entities;
using ButlerLee.API.Extensions;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ButlerLee.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IClientWrapper _client;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly uint _extraMinute;

        public PaymentService(IRepositoryWrapper repository, IClientWrapper client,
            IMapper mapper, ILoggerManager logger, Configurations configurations)
        {
            this._repository = repository;
            this._client = client;
            this._mapper = mapper;
            this._logger = logger;
            this._extraMinute = configurations.SchedulerSettings.ExtraMinute;
        }

        #region  Reservation No Generation
        //private string GenerateRandomCode(int length)
        //{
        //    const string src = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

        //    var sb = new StringBuilder();
        //    Random random = new Random();
        //    for (var i = 0; i < length; i++)
        //    {
        //        var c = src[random.Next(0, src.Length)];
        //        sb.Append(c);
        //    }

        //    return sb.ToString();
        //}

        //private async Task<string> GenerateReservationNo()
        //{
        //    string prefix = "BL";
        //    int randomCodeLength = 6;

        //    while (true)
        //    {
        //        string ramdomCode = GenerateRandomCode(randomCodeLength);
        //        string reservationNo = $"{prefix}-{ramdomCode}";

        //        var payments = await _repository.Payment.FindByAsync(o => o.ReservationNo.Equals(reservationNo));
        //        if (payments.Any() == false)
        //        {
        //            return reservationNo;
        //        }
        //    }
        //}
        #endregion

        #region Payment DB CRUD
        public async Task<PagedList<Models.Payment>> GetPayments(PaymentParameters parameters)
        {
            var payments = await _repository.Payment.GetPayments(parameters);

            return _mapper.Map<PagedList<Entities.Payment>, PagedList<Models.Payment>>(payments);
        }

        public async Task<Models.Payment> GetPaymentById(uint id)
        {
            var response =
                 await this._repository.Payment.GetPaymentById(id);

            return _mapper.Map<Entities.Payment, Models.Payment>(response);
        }

        public async Task<Models.Payment> GetPaymentByReservationId(int reservationId)
        {
            var response =
                await this._repository.Payment.GetPaymentByReservationId(reservationId);

            return _mapper.Map<Entities.Payment, Models.Payment>(response);
        }

        public async Task<Models.Payment> CreatePayment(Models.Payment payment)
        {
            Entities.Payment daoPayment = _mapper.Map<Entities.Payment>(payment);

            _repository.Payment.Create(daoPayment);
            await _repository.SaveChangesAsync();

            return await this.GetPaymentById(daoPayment.Id);
        }

        public async Task<Models.Payment> UpdatePayment(uint id, Models.Payment payment)
        {
            Entities.Payment originPayment =
                await _repository.Payment.GetPaymentById(id);

            Entities.Payment daoToUpdate = _mapper.Map<Entities.Payment>(payment);

            daoToUpdate.Id = originPayment.Id;
            daoToUpdate.UpdateDate = DateTime.UtcNow;

            _repository.Payment.Update(daoToUpdate, id);
            await _repository.SaveChangesAsync();

            return await this.GetPaymentById(id);
        }

        public async Task<Models.Payment> UpdatePaymentStatus(uint id, string status)
        {
            Models.Payment originPayment = await this.GetPaymentById(id);
            originPayment.Status = status;
            originPayment.UpdateDate = DateTime.UtcNow;

            await this.UpdatePayment(originPayment.Id, originPayment);

            await _repository.SaveChangesAsync();

            return await this.GetPaymentById(id);
        }

        public async Task DeletePayment(uint id)
        {
            Entities.Payment originPayment = await _repository.Payment.GetPaymentById(id);

            _repository.Payment.Delete(originPayment);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteReadyPaymentByReservationId(int reservationId)
        {
            Entities.Payment originPayment =
                await _repository.Payment.GetPaymentByReservationId(reservationId);

            if (originPayment != null
                && originPayment.TotalAmount < 1)
                _repository.Payment.Delete(originPayment);

            await _repository.SaveChangesAsync();
        }
        #endregion


        public async Task<Models.Reservation> UpdateReservationBeforePay(PaymentGateway gateway, Reservation reservation, float? cleaningFee)
        {
            int reservationId = reservation.Id;

            PriceParameters parameters = new PriceParameters();
            parameters.StartingDate = reservation.ArrivalDate;
            parameters.EndingDate = reservation.DepartureDate;
            parameters.Adults = reservation.Adults;
            parameters.Children = reservation.Children;

            var response = await _client.Listing.GetPrice(reservation.ListingId, parameters);

            PriceDetail priceDetail = response.Result;
            //int serverTotalPrice = (int)priceDetail.TotalPrice;

            //가격 정보 안맞을 경우 Error Return 
            if ((int)priceDetail.TotalPrice != (int)reservation.TotalPrice)
                throw new BadRequestException(ErrorCodes.TOTAL_PRICE_IS_INVALID);

            var reservationResponse = await _client.Reservation.GetReservationById(reservation.Id);
            Reservation originReservation = reservationResponse.Result;

            //기존 데이터 없거나, 상태가 취소인경우
            if (originReservation == null
                || originReservation.Status == ReservationStatus.Cancelled)
            {
                reservation.Id = 0;
                reservation.TotalPrice = (int)priceDetail.TotalPrice;
               // reservation.Phone = reservation.Phone

                //새로운 예약생성,
                //unpaidreservation LifeTime 15분으로 생성
                var createdResponse = await _client.Reservation.CreateReservation(reservation);
                Reservation crestedReservation = createdResponse.Result;

                UnpaidReservation unpaidReservation = new UnpaidReservation
                {
                    ReservationId = crestedReservation.Id,
                    LimitStartDate = DateTime.UtcNow.AddMinutes(_extraMinute)
                };

                _repository.UnpaidReservation.Create(unpaidReservation);
                await _repository.SaveChangesAsync();

                return crestedReservation;
            }
            else
            {
                //기존 데이터 있는경우 
                //Hostaway 결제 완료 -> isPaid = 0, 
                //Kakaopay, tospay 완료 -> isPaid = 1
                //미결제 -> isPaid = null
                if (originReservation.IsPaid != null)
                    throw new ConflictException(ErrorCodes.ALREADY_PAID_RESERVATION);

                originReservation.GuestFirstName = reservation.GuestFirstName;
                originReservation.GuestLastName = reservation.GuestLastName;
                originReservation.GuestEmail = reservation.GuestEmail;
                originReservation.GuestAddress = reservation.GuestAddress;
                originReservation.GuestCity = reservation.GuestCity;
                originReservation.GuestCountry = reservation.GuestCountry;
                originReservation.Currency = reservation.Currency;
                originReservation.Phone = reservation.Phone;
                originReservation.GuestNote = reservation.GuestNote;
                originReservation.TotalPrice = (int)priceDetail.TotalPrice;//reservation.TotalPrice;

                if (gateway == PaymentGateway.DirectCreditCard)
                {
                    originReservation.CardNumber = reservation.CardNumber;
                    originReservation.ExpirationMonth = reservation.ExpirationMonth;
                    originReservation.ExpirationYear = reservation.ExpirationYear;
                    originReservation.Cvc = reservation.Cvc;
                }

                var updatedReservation =
                    await _client.Reservation.UpdateReservation(originReservation.Id, originReservation);

                //unpaid reservation LifeTime 15분으로 연장
                UnpaidReservation unpaidReservation =
                    await _repository.UnpaidReservation.GetReservationByReservationId(reservation.Id);

                if (unpaidReservation != null)
                {
                    unpaidReservation.LimitStartDate = DateTime.UtcNow.AddMinutes(_extraMinute);
                    _repository.UnpaidReservation.Update(unpaidReservation, unpaidReservation.Id);
                }
                await _repository.SaveChangesAsync();

                return updatedReservation.Result;
            }
        }

        public async Task<Models.Payment> CreateHostAwayCardPayment(Reservation reservation)
        {
            //ReservationNo : 고객에게 보여줄 예약 번호 생성
            string reservationNo = await _repository.Payment.GenerateReservationNo();

            //HostAway에 IsPaid, ReservationNo저장
            //카드 직접결제 이므로 => IsPaid = false 
            var updatedResponse =
                await _client.Reservation.UpdateReservation(reservation.Id, PaymentGateway.DirectCreditCard.ToString(), false);

            DateTime realApproveTime =
               TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Korea Standard Time");

            Reservation updatedReservation = updatedResponse.Result;
            string listingName = await _client.Listing.GetListingNameById(updatedReservation.ListingId);
            Entities.Payment payment = new Entities.Payment();
            payment.TotalAmount = (int)updatedReservation.TotalPrice;
            payment.PaymentGateway = PaymentGateway.DirectCreditCard.ToString();
            payment.PaymentKey = string.Empty;
            payment.ListingId = updatedReservation.ListingId;
            payment.ApproveDate = realApproveTime;
            payment.ReservationId = reservation.Id;
            payment.ReservationNo = reservationNo;
            payment.OrderName = $"{listingName} ({updatedReservation.Nights}박)";
            payment.UpdateDate = DateTime.UtcNow;
            payment.ArrivalDate = updatedReservation.ArrivalDate;
            payment.DepartureDate = updatedReservation.DepartureDate;

            Entities.Payment upsertedPayment = await _repository.Payment.UpsertPayment(payment);

            //예약 완료 후 unpaid reservation 삭제
            await _repository.UnpaidReservation.Delete(updatedReservation.Id);

            return _mapper.Map<Models.Payment>(upsertedPayment);
        }

        public async Task DeleteReadyPayments()
        {
            try
            {
                await _repository.Payment.DeleteReadyPayments();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in the background service(Delete Payments). error : " + ex.Message);
            }
        }

    }
}
