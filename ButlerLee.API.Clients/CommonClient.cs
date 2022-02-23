using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Clients
{
    public class CommonClient : ICommonClient
    {
        private readonly HttpClient httpClient;

        public CommonClient(IHttpClientFactory clientFactory) => this.httpClient = clientFactory.CreateClient(nameof(HostAwayClient));

        public async Task<HostawayResponse<Dictionary<string,string>>> GetContries()
        {
            HttpResponseMessage response =
                await this.httpClient.GetAsync("countries");

            await response.EnsureSuccessStatusCodeAsync();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Dictionary<string, string>>>(content); 
        }
        public async Task<HostawayResponse<Dictionary<string, string>>> GetCurrencies()
        {
            HttpResponseMessage response =
                await this.httpClient.GetAsync("currencies");

            await response.EnsureSuccessStatusCodeAsync();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Dictionary<string, string>>>(content);
        }
    }
}
