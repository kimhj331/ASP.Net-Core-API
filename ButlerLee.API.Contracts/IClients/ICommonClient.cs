using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IClients
{
    public interface ICommonClient
    {
        Task<HostawayResponse<Dictionary<string, string>>> GetContries();
        Task<HostawayResponse<Dictionary<string, string>>> GetCurrencies();
    }
}
