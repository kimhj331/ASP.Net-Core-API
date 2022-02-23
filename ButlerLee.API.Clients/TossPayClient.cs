using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Extensions;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Clients
{
    public class TossPayClient : ITossPayClient
    {
        private readonly HttpClient httpClient;
        private const string defaultUri = "payments";

        public TossPayClient(IHttpClientFactory clientFactory) => this.httpClient = clientFactory.CreateClient(nameof(Models.TossPayClient));

        public async Task<TossPaymentResponse> Approval(TossPayApprovalParameters parameters)
        {
            TossPayApprovalParameters data = new TossPayApprovalParameters();
            data.Amount = parameters.Amount;
            data.OrderId = parameters.OrderId;

            HttpContent content = 
                new StringContent(data.ToJson(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = 
                await this.httpClient.PostAsync($"{defaultUri}/{parameters.PaymentKey}", content);
           
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TossPaymentResponse>(responseContent);
        }

        public async Task<TossPaymentResponse> Cancel(TossCancel cancel)
        {
            HttpContent content =
                new StringContent(cancel.ToJson(), Encoding.UTF8, "application/json");

            HttpResponseMessage response =
                await this.httpClient.PostAsync($"{defaultUri}/{cancel.PaymentKey}/cancel ", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TossPaymentResponse>(responseContent);
        }

    }
}
