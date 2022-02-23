using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Extensions;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ButlerLee.API.Clients
{
    public class KakaoPayClient : IKakaoPayClient
    {
        private readonly HttpClient httpClient;

        private const string readytUri = "payment/ready";
        private const string approvalUri = "payment/approve";
        private const string cancelUri = "payment/cancel";
        private readonly string Cid;

        public KakaoPayClient(IHttpClientFactory clientFactory) 
        {
            this.httpClient = clientFactory.CreateClient(nameof(Models.KakaoPayClient));
            
            var keyValuePair = httpClient.DefaultRequestHeaders.FirstOrDefault(header => header.Key == nameof(Cid));
            if(keyValuePair.Value != null)
                Cid = keyValuePair.Value.FirstOrDefault();
        }

        public async Task<KakaoPayReadyResponse> Ready(KakaoPayReadyRequest readyRequest)
        {
            readyRequest.Cid = Cid;
            HttpContent content = new FormUrlEncodedContent(readyRequest.GetDictionary());

            HttpResponseMessage response = await this.httpClient.PostAsync(readytUri, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<KakaoPayReadyResponse>(responseContent);
        }

        public async Task<KakaoPaymentResponse> Approval(KakaoPaymentResponse approvalRequest)
        {
            approvalRequest.Cid = Cid;

            HttpContent data =
                 new FormUrlEncodedContent(approvalRequest.GetDictionary());

            HttpResponseMessage response = await this.httpClient.PostAsync(approvalUri, data);

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.KakaoPaymentResponse>(responseContent);
        }

        public async Task<KakaoPaymentResponse> Cancel(KakaoCanelRequest cancelRequest)
        {
            cancelRequest.Cid = Cid;

            HttpContent data =
                    new FormUrlEncodedContent(cancelRequest.GetDictionary());

            HttpResponseMessage response = await this.httpClient.PostAsync(cancelUri, data);

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.KakaoPaymentResponse>(responseContent);
        }
    }
}
