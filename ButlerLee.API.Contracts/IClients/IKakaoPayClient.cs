using ButlerLee.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IClients
{
    public interface IKakaoPayClient
    {
        Task<KakaoPayReadyResponse> Ready(KakaoPayReadyRequest readyRequest);
        Task<KakaoPaymentResponse> Approval(KakaoPaymentResponse approvalRequest);
        Task<KakaoPaymentResponse> Cancel(KakaoCanelRequest cancelRequest);
    }
}
