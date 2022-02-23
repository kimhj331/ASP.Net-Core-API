using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IClients
{
    public interface ITossPayClient
    {
        Task<TossPaymentResponse> Approval(TossPayApprovalParameters parameters);
        Task<TossPaymentResponse> Cancel(TossCancel cancel);
    }
}
