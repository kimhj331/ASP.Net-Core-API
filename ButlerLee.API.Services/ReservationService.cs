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
using ButlerLee.API.Models.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ButlerLee.API.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IClientWrapper _client;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ReservationService(IRepositoryWrapper repository, IClientWrapper client, ILoggerManager logger, IMapper mapper)
        {
            this._repository = repository;
            this._client = client;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<PagedList<Reservation>> GetReservations(ReservationParameters parameters)
        {
            var response = await this._client.Reservation.GetReservations(parameters);

            return new PagedList<Reservation>(response.Result, response.Count, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Reservation> GetReservationById(int reservationId)
        {
            var response = await this._client.Reservation.GetReservationById(reservationId);

            return response.Result;
        }

        public async Task<Reservation> CreateReservation(Reservation reservation)
        {
            var listingResponse = await _client.Listing.GetListingById(reservation.ListingId, false);

            if (listingResponse.Result == null)
                throw new ConflictException(ErrorCodes.LISTING_NOT_FOUND);

            Listing listing = listingResponse.Result;

            if(listing.MaxGuestNumber < reservation.NumberOfGuests)
                throw new BadRequestException(ErrorCodes.ROOM_CAPACITY_HAS_EXCEEDED);

            ListingParameters parameters = new ListingParameters
            {
                StartDate = reservation.ArrivalDate.Date,
                EndDate = reservation.DepartureDate.Date,
                Adults = reservation.Adults == null ? 0 : (int)reservation.Adults,
                Children = reservation.Children == null ? 0 : (int)reservation.Children
            };

            var bookableListing = await this._client.Listing.GetBookableListing(reservation.ListingId, parameters);

            if (bookableListing == null)
                throw new ConflictException(ErrorCodes.UNBOOKABLE_LISTING);

            var response = await this._client.Reservation.CreateReservation(reservation);

            var createdReservation = response.Result;

            _repository.UnpaidReservation.Create(new Entities.UnpaidReservation()
            {
                ReservationId = createdReservation.Id,
                LimitStartDate = DateTime.UtcNow
            });

            await _repository.SaveChangesAsync();

            return response.Result;
        }

        public async Task<Reservation> CreateReservationWithCard(Reservation reservation)
        {
            var response = await this._client.Reservation.CreateReservationWithCard(reservation);

            return response.Result;
        }
        public async Task<Reservation> UpdateReservation(int reservationId, Reservation reservation)
        {
            var response = await this._client.Reservation.UpdateReservation(reservationId, reservation);

            return response.Result;
        }

        public async Task<Reservation> UpdateReservation(int reservationId, string hostNote)
        {
            var response =
                await this._client.Reservation.UpdateReservation(reservationId, hostNote);

            return response.Result;
        }

        public async Task<Reservation> CancelReservaion(int reservationId, CancelledBy cancelledBy)
        {
            var response = await this._client.Reservation.CancelReservaion(reservationId, cancelledBy);
            return response.Result;
        }

        public async Task DeleteReservation(int reservationId)
        {
            await this._client.Reservation.DeleteReservation(reservationId);
        }

        public async Task CreateUnpaidReservation(int reservationId)
        {
            _repository.UnpaidReservation.Create(new Entities.UnpaidReservation()
            {
                ReservationId = reservationId,
                LimitStartDate = DateTime.UtcNow
            });

            await _repository.SaveChangesAsync();
        }

        public async Task CancelUnpaidReservations()
        {
            try
            {
                var reservations = 
                    await _repository.UnpaidReservation.GetReservationsToCancel();

                foreach (var reservation in reservations)
                {
                    reservation.CancelDate = DateTime.UtcNow;
                    _repository.UnpaidReservation.Update(reservation, reservation.Id);
                    Reservation originReservation = await this.GetReservationById(reservation.ReservationId);

                    // 결제 완료시
                    // DirectCreditCard : IsPaid = 0 & 
                    // Toss, KakaoPay : IsPaid = 1
                    // 결제 완료 아닐 경우만 취소
                    if (originReservation != null
                        && originReservation.IsPaid == null)
                        await this.CancelReservaion(reservation.ReservationId, CancelledBy.Host);
                }

                var reservationsToDelete =
                    await _repository.UnpaidReservation.GetReservationsToDelete();

                foreach (var reservation in reservationsToDelete)
                {
                    _repository.UnpaidReservation.Delete(reservation);

                    Reservation originReservation = await this.GetReservationById(reservation.ReservationId);

                    if (originReservation != null
                        && originReservation.IsPaid == null)
                        await this.DeleteReservation(reservation.ReservationId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in the background service. error : " + ex.Message);
            }
            finally
            {
                await _repository.SaveChangesAsync();
            }
        }

        public async Task DeleteUnpaidReservation(int reservationId)
        {
            UnpaidReservation unpaidReservation =
             await _repository.UnpaidReservation.GetReservationByReservationId(reservationId);

            if (unpaidReservation != null)
            {
                _logger.LogError($"[!!!!occurred!!!] in DeleteUnpaidReservation :{unpaidReservation.ReservationId} ");
                unpaidReservation.CancelDate = DateTime.UtcNow;
                _repository.UnpaidReservation.Update(unpaidReservation, unpaidReservation.Id);
            }

            Reservation originReservation = await this.GetReservationById(reservationId);

            // 결제 완료시
            // DirectCreditCard : IsPaid = 0 & 
            // Toss, KakaoPay : IsPaid = 1
            // 결제 완료 아닐 경우만 취소
            if (originReservation != null 
                && originReservation.IsPaid == null) 
                await this.CancelReservaion(reservationId, CancelledBy.Host);

            await _repository.SaveChangesAsync();
        }

        public async Task CheckBookableReservation(Reservation reservation)
        {
            if (reservation.IsPaid != null)
                throw new ConflictException(ErrorCodes.ALREADY_PAID_RESERVATION);

            //해당 예약이 취소되었다면
            //해당 일자에 예약이 가능한지 확인핟다.
            if (reservation.Status == ReservationStatus.Cancelled)
            {
                ListingParameters parameters = new ListingParameters()
                {
                    StartDate = reservation.ArrivalDate.Date,
                    EndDate = reservation.DepartureDate.Date,
                    Adults = reservation.Adults == null ? 0 : (int)reservation.Adults,
                    Children = reservation.Children == null ? 0 : (int)reservation.Children
                };

                Listing bookableListing =
                    await _client.Listing.GetBookableListing(reservation.ListingId, parameters);

                if (bookableListing == null)
                    throw new ConflictException(ErrorCodes.UNBOOKABLE_LISTING);
            }
        }
    }
}
