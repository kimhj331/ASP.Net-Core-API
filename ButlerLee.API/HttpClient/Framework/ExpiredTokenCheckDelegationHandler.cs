using ButlerLee.API.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ButlerLee.API.HttpClient.Framework
{
    public class ExpiredTokenCheckDelegationHandler : DelegatingHandler
    {
        public readonly Models.HostAwayClient _hostAwayClient;
        public ExpiredTokenCheckDelegationHandler(Models.HostAwayClient hostAwayClient) => _hostAwayClient = hostAwayClient;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode == false)
            {
                BaseResponse result = JsonConvert.DeserializeObject<BaseResponse>(content);

                if (string.IsNullOrEmpty(result.Message) == false
                && result.Message == _hostAwayClient.ExpiredTokenMessage)
                 response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            }

            return response;
        }
    }
}
