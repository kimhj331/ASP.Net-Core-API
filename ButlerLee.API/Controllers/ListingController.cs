using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Extensions;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ButlerLee.API.Controllers
{
    [Produces("application/json")]
    [Route("api/listings")]
    [ApiController]
    public class ListingController : Controller
    {
        private readonly IServiceWrapper _service;

        public ListingController(IServiceWrapper service)
        {
            _service = service;
        }

        /// <summary>
        /// 숙소 목록
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/listings
        /// 
        /// </remarks>
        /// <returns></returns>	    
        [HttpGet("", Name = "GetListings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListings([FromQuery] ListingParameters parameters)
        {
            PagedList<Listing> listings =
                await _service.Listing.GetListings(parameters);

            var metadata = new
            {
                listings.TotalCount,
                listings.PageSize,
                listings.CurrentPage,
                listings.TotalPages,
                listings.HasNext,
                listings.HasPrevious
            };

            Response.Headers.Add("x-pagination", metadata.ToJson());

            return Ok(listings);
        }

        /// <summary>
        /// 숙소 정보
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/listings/{id}
        /// 
        /// </remarks>
        /// <returns></returns>	        
        [HttpGet("{id}", Name = "GetListingById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListingById(int id)
        {
            Listing listing =
                await _service.Listing.GetListingById(id);

            if (listing == null)
                throw new NotFoundException(ErrorCodes.LISTING_NOT_FOUND);   

            return Ok(listing);
        }

        /// <summary>
        /// 달력 정보
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/listings/{id}/calendar
        /// 
        /// </remarks>
        /// <returns></returns>	               
        [HttpGet("{id}/calendar", Name = "GetCalendar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCalendar(int id, [FromQuery] CalendarParameters parameters)
        {
            Listing listing = await _service.Listing.GetListingById(id);
            
            if (listing == null)
                throw new NotFoundException(ErrorCodes.LISTING_NOT_FOUND);

            IEnumerable<CalendarDay> calendar = 
                await _service.Listing.GetCalender(id, parameters);

            return Ok(calendar);
        }

        /// <summary>
        /// 숙소 가격 정보
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/listings/{id}/price-detail
        /// 
        /// </remarks>
        /// <returns></returns>	        
        [HttpGet("{id}/price-detail", Name = "GetPriceDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPriceDetail(int id, [FromQuery] PriceParameters prameters)
        {
            if (prameters == null || ModelState.IsValid == false)
                throw new BadRequestException(ErrorCodes.PARAMETER_OBJECT_IS_NULL);

            Listing listing = await _service.Listing.GetListingById(id);

            if (listing == null)
                throw new NotFoundException(ErrorCodes.LISTING_NOT_FOUND);

            PriceDetail priceDetail = 
                await _service.Listing.GetPrice(id, prameters);

            return Ok(priceDetail);
        }
    }
}
