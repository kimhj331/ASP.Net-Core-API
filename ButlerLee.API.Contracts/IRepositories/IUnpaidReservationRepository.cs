using ButlerLee.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IRepositories
{
    public interface IUnpaidReservationRepository : IRepositoryBase<UnpaidReservation>
    {
        Task<IEnumerable<UnpaidReservation>> GetReservationsToCancel();
        Task<IEnumerable<UnpaidReservation>> GetReservationsToDelete();
        Task<UnpaidReservation> GetReservationById(uint id);
        Task<UnpaidReservation> GetReservationByReservationId(int reservationId);
        Task Delete(int reservationId);
    }
}
