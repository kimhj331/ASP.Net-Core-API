using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Extensions;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Controllers
{
    [Produces("application/json")]
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly IServiceWrapper _service;
        // private readonly int _testListingId = 88635;
        private const string _korTimeZoneId = "Korea Standard Time";
        public ReservationController(IServiceWrapper service)
        {
            _service = service;
        }

        /// <summary>
        /// 예약 목록
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/reservations
        ///     
        /// </remarks>
        /// <returns></returns>	        
        [HttpGet("", Name = "GetReservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetReservations([FromQuery] ReservationParameters parameters)
        {
            PagedList<Reservation> reservations =
                await _service.Reservation.GetReservations(parameters);

            if (reservations == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            var metadata = new
            {
                reservations.TotalCount,
                reservations.PageSize,
                reservations.CurrentPage,
                reservations.TotalPages,
                reservations.HasNext,
                reservations.HasPrevious
            };

            Response.Headers.Add("x-pagination", metadata.ToJson());

            return Ok(reservations);
        }

        /// <summary>
        /// 예약 정보
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/reservations/{id}
        ///     
        /// </remarks>
        /// <returns></returns>	        
        [HttpGet("{id}", Name = "GetReservationById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReservationById(int id)
        {
            Reservation reservation =
                await _service.Reservation.GetReservationById(id);

            if (reservation == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            return Ok(reservation);
        }

        /// <summary>
        /// 예약 생성
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: api/reservations
        ///     {
        ///         "listingId": 88635,
        ///         "adults" : 2,
        ///         "children" : 0,
        ///         "Infants" : 0,
        ///         "arrivalDate": "2023-09-15",
        ///         "departureDate": "2023-09-16",
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>	        
        [HttpPost("", Name = "CreateReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
        {
            if (reservation == null || ModelState.IsValid == false)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID);

            if(reservation.ListingId < 1 
                || reservation.Adults == null 
                || reservation.Children < 0)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID);

            //UTC -> 한국시간
            //변수로 들어온 날짜와 예약 가능일을 비교하기 위함
            //why? Kor + 8h = UTC. 00시 ~ 09에 예약 시도시 Convert 하지 않으면 어제 날짜 기입해도 예약됨
            DateTime dateToCompare = 
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, _korTimeZoneId);

            if (reservation.ArrivalDate.Date < dateToCompare.Date ||
               reservation.DepartureDate.Date <= reservation.ArrivalDate.Date)
                throw new BadRequestException(ErrorCodes.DATE_IS_INVALID);

            if (reservation.Adults < 1)
                throw new BadRequestException(ErrorCodes.ADULTS_IS_NULL);
           
            //if (reservation.ListingId.Equals(_testListingId) == false)
            //    throw new BadRequestException(ErrorCodes.NOT_TEST_LISTING);

            Reservation createdReservation =
               await _service.Reservation.CreateReservation(reservation);

            return Ok(createdReservation);
        }

        /// <summary>
        /// 카드 예약 생성
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: api/reservations/card
        ///     {
        ///         "listingId": 88635,
        ///         "guestFirstName": "HeeJi",           
        ///         "guestLastName": "Kim",
        ///         "guestEmail": "gmlwl7777@naver.com",
        ///         "adults" : 2,
        ///         "children" : 0,
        ///         "Infants" : 0,
        ///         "arrivalDate": "2023-09-15",
        ///         "departureDate": "2023-09-16",
        ///         "phone": "01053418210",
        ///         "totalPrice": 510000,
        ///         "guestNote": null,
        ///         "cardNumber": "",
        ///         "cardUserName": "",
        ///         "expirationMonth": "",
        ///         "expirationYear": "",
        ///         "cvc" : 123
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("card", Name = "CreateReservationWithCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CreateReservationWithCard([FromBody] Reservation reservation)
        {
            if (reservation == null || ModelState.IsValid == false)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID);

            if (reservation.IsCreditCardInfoValid() == false)
                throw new BadRequestException(ErrorCodes.INVALID_CREDIT_CARD);

            //if (reservation.ListingId.Equals(_testListingId) == false)
            //    throw new BadRequestException(ErrorCodes.NOT_TEST_LISTING);

            Reservation createdReservation =
               await _service.Reservation.CreateReservationWithCard(reservation);

            return Ok(createdReservation);
        }

        /// <summary>
        /// 예약 수정 (수정 할 데이터만 입력)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT: api/reservations/{id}
        ///     
        /// </remarks>
        /// <returns></returns>	        
        [HttpPut("{id}", Name = "UpdateReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            if (reservation == null || ModelState.IsValid == false)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID);

            Reservation updatedReservation =
               await _service.Reservation.UpdateReservation(id, reservation);

            return Ok(updatedReservation);
        }

        /// <summary>
        /// 호스트 예약 취소
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT: api/reservations/{id}/cancel-host
        ///
        /// </remarks>
        [HttpPut("{id}/cancel-host", Name = "CancelReservationAsHost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelReservationAsHost(int id)
        {
            Reservation cancelledReservation =
               await _service.Reservation.CancelReservaion(id, CancelledBy.Host);

            return Ok(cancelledReservation);
        }

        /// <summary>
        /// 게스트 예약 취소
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT: api/reservations/{id}/cancel-guest
        ///
        /// </remarks>
        [HttpPut("{id}/cancel-guest", Name = "CancelReservationAsGuest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelReservationAsGuest(int id)
        {
            Reservation cancelledReservation =
               await _service.Reservation.CancelReservaion(id, CancelledBy.Guest);

            return Ok(cancelledReservation);
        }

        /// <summary>
        /// 예약 삭제
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     DELETE: api/reservations/{id}
        /// </remarks>
        //[HttpDelete("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<IActionResult> DeleteReservation(int id)
        //{
        //    await _service.Reservation.DeleteReservation(id);
        //    return NoContent();
        //}

        /// <summary>
        /// (DB)예약 선점 트렌젝션 삭제, (Hostaway)예약 삭제 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     DELETE: api/reservations/{id}
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUnpaidReservation(int id)
        {
            await _service.Reservation.DeleteUnpaidReservation(id);
            return Ok();
        }

        /// <summary>
        /// 예약 상세정보 조회
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     Get: api/reservations/{id}/info
        ///     
        /// </remarks>
        [HttpGet("{id}/info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReservationInfo(int id)
        {
            Reservation reservation = await _service.Reservation.GetReservationById(id);

            if (reservation == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            Listing listing = await _service.Listing.GetListingById(reservation.ListingId);

            Payment payment = await _service.Payment.GetPaymentByReservationId(id);

            if (payment == null)
                throw new NotFoundException(ErrorCodes.PAYMENT_NOT_FOUND);

            ReservationInfo reservationInfo =
                new ReservationInfo(reservation, listing, payment);
           
            return Ok(reservationInfo);
        }

        /// <summary>
        /// 예약 가능 여부 조회
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET: api/reservations/{id}/check
        ///     
        /// </remarks>
        [HttpGet("{id}/check", Name = "CheckBookableReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckBookableReservation(int id)
        {
            Reservation reservation = await _service.Reservation.GetReservationById(id);

            if(reservation == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            await _service.Reservation.CheckBookableReservation(reservation);
            return Ok();
        }
    }
}
