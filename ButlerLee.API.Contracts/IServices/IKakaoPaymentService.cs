using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IServices
{
    public interface IKakaoPaymentService
    {
        Task<KakaoPayReadyResponse> Ready(Reservation reservation);
        Task<Payment> Approval(KakaoPayApprovalParameters parameters, Reservation reservation);
        Task<Payment> Cancel(PaymentCancelParameters parameters, int reservationId);
    }
}
