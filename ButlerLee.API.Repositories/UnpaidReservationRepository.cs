using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Entities;
using ButlerLee.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ButlerLee.API.Repositories
{
    public class UnpaidReservationRepository : RepositoryBase<UnpaidReservation>, IUnpaidReservationRepository
    {
        private readonly RepositoryContext _context;
        private readonly uint _preemptingMinute; //선점시간

        public UnpaidReservationRepository(RepositoryContext repositoryContext, Configurations configurations)
          : base(repositoryContext)
        {
            _context = repositoryContext;
            _preemptingMinute = configurations.SchedulerSettings.PreemptingMinute;
        }

        public async Task<UnpaidReservation> GetReservationById(uint id)
        {
            IQueryable<UnpaidReservation> query = _context.UnpaidReservation
                .Where(o => o.Id.Equals(id));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<UnpaidReservation> GetReservationByReservationId(int reservationId)
        {
            IQueryable<UnpaidReservation> query = _context.UnpaidReservation
                .Where(o => o.ReservationId.Equals(reservationId));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<UnpaidReservation>> GetReservationsToCancel()
        {
            DateTime now = DateTime.UtcNow;

            IQueryable<UnpaidReservation> query = _context.UnpaidReservation
                .Where(o => o.CancelDate == null
                && EF.Functions.DateDiffMinute(o.LimitStartDate, now) >= _preemptingMinute)
                .OrderByDescending(o => o.Id)
                .AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UnpaidReservation>> GetReservationsToDelete()
        {
            DateTime now = DateTime.UtcNow;

            IQueryable<UnpaidReservation> query = _context.UnpaidReservation
                .Where(o => EF.Functions.DateDiffMinute(o.CancelDate, now) >= _preemptingMinute)
                .OrderByDescending(o => o.Id)
                .AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task Delete(int reservationId)
        {
            var unpaidReservation =
                await this.GetReservationByReservationId(reservationId);

            if (unpaidReservation != null)
                _context.UnpaidReservation.Remove(unpaidReservation);

            await _context.SaveChangesAsync();
        }
    }
}
