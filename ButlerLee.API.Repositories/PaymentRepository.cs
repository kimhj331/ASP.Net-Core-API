using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Entities;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Repositories
{
    public class PaymentRepository : RepositoryBase<Entities.Payment>, IPaymentRepository
    {
        private readonly RepositoryContext _context;
        private readonly int _readyLimitMinute = 30;

        public PaymentRepository(RepositoryContext repositoryContext)
          : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public async Task<PagedList<Entities.Payment>> GetPayments(PaymentParameters parameters) 
        {
            IQueryable<Entities.Payment> query = _context.Payment;

            if (parameters.Gateway != null)
                query = query.Where(o => o.PaymentGateway.Equals(parameters.Gateway.ToString()));

            if (parameters.ApproveDate != null)
                query = query.Where(o => o.ApproveDate != null &&
                                         ((DateTime)o.ApproveDate).Date == ((DateTime)parameters.ApproveDate).Date);

            if (parameters.ListingId > 0)
                query = query.Where(o => o.ListingId == parameters.ListingId);

            if (parameters.Ordering < 0)
                query = query.OrderByDescending(o => o.Id);

            return await PagedList<Entities.Payment>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Entities.Payment> GetPaymentById(uint id)
        {
            IQueryable<Entities.Payment> query = _context.Payment
                .Where(o => o.Id.Equals(id));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Entities.Payment> GetPaymentByReservationId(int reservationId)
        {
            IQueryable<Entities.Payment> query = _context.Payment
               .Where(o => o.ReservationId.Equals(reservationId));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Entities.Payment> GetPaymentByReservationNo(string reservationNo)
        {
            IQueryable<Entities.Payment> query = _context.Payment
                   .Where(o => o.ReservationNo.Equals(reservationNo));

            return await query.SingleOrDefaultAsync();
        }

        public async Task DeleteReadyPayments()
        {
            DateTime now = DateTime.UtcNow;

            IQueryable<Entities.Payment> query = _context.Payment
                   .Where(o => o.PaymentGateway == PaymentGateway.KakaoPay.ToString()
                   && string.IsNullOrEmpty(o.ReservationNo)
                   && EF.Functions.DateDiffMinute(o.UpdateDate, now) > _readyLimitMinute)
                   .OrderBy(o => o.Id)
                   .AsNoTracking();

            _context.Payment.RemoveRange(await query.ToListAsync());
            await _context.SaveChangesAsync();
        }

        public async Task<Entities.Payment> UpsertPayment(Entities.Payment payment)
        {
            Entities.Payment originPayment  = 
                await this.GetPaymentByReservationId(payment.ReservationId);

            if (originPayment == null)
            {
                this.Create(payment);
            }
            else
            {
                payment.Id = originPayment.Id;
                this.Update(payment, originPayment.Id);
            }

            await _context.SaveChangesAsync();
            return await this.GetPaymentByReservationId(payment.ReservationId);
        }

        #region  Reservation No Generation
        private string GenerateRandomCode(int length)
        {
            const string src = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var sb = new StringBuilder();
            Random random = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[random.Next(0, src.Length)];
                sb.Append(c);
            }

            return sb.ToString();
        }

        public async Task<string> GenerateReservationNo()
        {
            string prefix = "BL";
            int randomCodeLength = 6;

            while (true)
            {
                string ramdomCode = GenerateRandomCode(randomCodeLength);
                string reservationNo = $"{prefix}-{ramdomCode}";

                var payments =
                    await RepositoryContext.Set<Entities.Payment>().
                    Where(o => o.ReservationNo.Equals(reservationNo)).ToListAsync();

                if (payments.Any() == false)
                {
                    return reservationNo;
                }
            }
        }
        #endregion
    }
}
