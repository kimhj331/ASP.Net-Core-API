using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IClients
{
    public interface IReservationClient
    {
        Task<HostawayResponse<IEnumerable<Reservation>>> GetReservations(ReservationParameters parameters);
        Task<HostawayResponse<Reservation>> GetReservationById(int reservationId);
        Task<HostawayResponse<Reservation>> CreateReservation(Reservation reservation);
        Task<HostawayResponse<Reservation>> CreateReservationWithCard(Reservation cardReservation);
        Task<HostawayResponse<Reservation>> UpdateReservation(int reservationId, Reservation reservation);
        Task<HostawayResponse<Reservation>> UpdateReservation(int reservationId, string hostNote, bool isPaid = true);
        Task<HostawayResponse<Reservation>> CancelReservaion(int reservationId, CancelledBy cancelledBy);
        Task DeleteReservation(int reservationId);
    }
}
