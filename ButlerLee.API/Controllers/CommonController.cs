using ButlerLee.API.Contracts.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Controllers
{
    [Produces("application/json")]
    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IServiceWrapper _service;

        public CommonController(IServiceWrapper service)
        {
            _service = service;
        }

        /// <summary>
        /// 국가코드 리스트
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/countries
        /// 
        /// </remarks>
        /// <returns></returns>	    
        [HttpGet("countries", Name = "GetContries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContries()
        {
            Dictionary<string,string> contries =  await _service.Common.GetContries();
            return Ok(contries);
        }

        /// <summary>
        /// 통화코드 리스트
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/currencies
        /// 
        /// </remarks>
        /// <returns></returns>	    
        [HttpGet("currencies", Name = "GetCurrencies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrencies()
        {
            Dictionary<string, string> currencies = await _service.Common.GetCurrencies();

            return Ok(currencies);
        }
    }
}
