using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Controllers
{
    [Produces("application/json")]
    [Route("api/amenities")]
    [ApiController]
    public class AmenityController : Controller
    {
        private readonly IServiceWrapper _service;

        public AmenityController(IServiceWrapper service)
        {
            _service = service;
        }

        /// <summary>
        /// 어메니티 목록
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/amenities
        ///     
        /// </remarks>
        /// <returns></returns>	        
        [HttpGet("", Name = "GetAmenities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAmenities()
        {
            IEnumerable<Amenity> amenities =
                await _service.Amenity.GetAmenities();

            return Ok(amenities);
        }

        /// <summary>
        /// 어메니티 정보
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/amenities/{id}
        ///     
        /// </remarks>
        /// <returns></returns>	        
        [HttpGet("{id}", Name = "GetAmenityById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAmenityById(int id)
        {
            Amenity amenity =
                await _service.Amenity.GetAmenityById(id);

            return Ok(amenity);
        }
    }
}
