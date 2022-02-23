using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Services
{
    public class CommonService : ICommonService
    {
        private readonly IClientWrapper _client;

        public CommonService(IClientWrapper client) => this._client = client;

        public async Task<Dictionary<string, string>> GetContries()
        {
            var response = await this._client.Common.GetContries();

            return response.Result;
        }

        public async Task<Dictionary<string, string>> GetCurrencies()
        {
            var response = await this._client.Common.GetCurrencies();

            return response.Result;
        }

    }
}
