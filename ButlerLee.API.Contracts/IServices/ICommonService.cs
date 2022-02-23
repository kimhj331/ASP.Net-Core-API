using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IServices
{
    public interface ICommonService
    {
        Task<Dictionary<string, string>> GetContries();

        Task<Dictionary<string, string>> GetCurrencies();

    }
}
