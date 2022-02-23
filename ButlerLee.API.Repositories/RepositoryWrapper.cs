using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Entities;
using ButlerLee.API.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _context;
        private Configurations _configurations;

        public RepositoryWrapper(RepositoryContext context, Configurations configurations)
        {
            _context = context;
            _configurations = configurations;
        }


        private IUnpaidReservationRepository _unpaidReservation;
        public IUnpaidReservationRepository UnpaidReservation
        {
            get
            {
                if (_unpaidReservation == null)
                    _unpaidReservation = new UnpaidReservationRepository(_context, _configurations);

                return _unpaidReservation;
            }
        }

        private IPaymentRepository _payment;
        public IPaymentRepository Payment
        {
            get 
            {
                if (_payment == null)
                    _payment = new PaymentRepository(_context);
                return _payment;
            }
          
        }
       
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
