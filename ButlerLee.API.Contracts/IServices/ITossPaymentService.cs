using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IServices
{
    public interface ITossPaymentService
    {
        Task<Models.Payment> Approval(TossPayApprovalParameters parameters, Reservation reservation);

        Task<Models.Payment> Cancel(PaymentCancelParameters parameters, int reservationId);
    }
}
