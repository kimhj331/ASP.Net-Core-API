using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Entities;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace ButlerLee.API.Controllers
{
    [Produces("application/json")]
    [Route("api/payments/kakao")]
    [ApiController]
    public class KakaoPaymentController : Controller
    {
        private readonly IServiceWrapper _service;
        //private readonly int _testListingId = 88635;
        public KakaoPaymentController(IServiceWrapper service)
        {
            _service = service;
        }

        /// <summary>
        /// Kakaopay결제요청
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     step1. Reservation update(hostaway)  
        ///     step2. KakaoPay결제 준비 요청
        /// 
        ///     POST: api/payments/kakao/ready
        ///     {
        ///         "id" : reservationId,
        ///         "listingId": 88635,
        ///         "guestFirstName": "HeeJi",           
        ///         "guestLastName": "Kim",
        ///         "guestEmail": "hjkim@saladsoft.com",
        ///         "guestAddress" : "서울시 성동구",
        ///         "guestCity" : "서울특별시",
        ///         "guestZipCode" : "12345",
        ///         "guestCountry" : "KR",
        ///         "numberOfGuests": 3,
        ///         "adults" : 3,
        ///         "children" : null,
        ///         "totalPrice" : 1000000,
        ///         "currency" : "KRW",
        ///         "arrivalDate": "2023-09-15",
        ///         "departureDate": "2023-09-16",
        ///         "phone": "+821053418210",
        ///         "totalPrice": 510000,
        ///         "guestNote": "깨끗하게 청소해주세요"
        ///     }
        /// </remarks>
        /// <returns></returns>	    
        [HttpPost("ready", Name = "KakaoReady")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> KakaoReady([FromBody] Reservation reservation)
        {
            if (reservation == null)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID);

            //if (reservation.ListingId.Equals(_testListingId) == false)
            //    throw new BadRequestException(ErrorCodes.NOT_TEST_LISTING);

            Reservation originReservation = 
                await _service.Reservation.GetReservationById(reservation.Id);

            if(originReservation == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            if (originReservation.Status == ReservationStatus.Cancelled)
                throw new ConflictException(ErrorCodes.ALREADY_CANCELED_RESERVATION);

            if (originReservation.IsPaid != null)
                throw new ConflictException(ErrorCodes.PAYMET_ALREADY_EXISTS);

            KakaoPayReadyResponse response = await _service.KakaoPayment.Ready(originReservation);

            return Ok(response);
        }

        /// <summary>
        /// 결제 fail
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET: "api/payments/kakao/fail?reservationId={reservationId}"
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("fail", Name = "KakaoFail")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> KakaoFail([FromQuery]int reservationId)//[FromQuery] KakaoPayApprovalParameters parameters)
        {
            if (reservationId < 1)
                return BadRequest(ErrorCodes.PARAMETER_OBJECT_IS_NULL);

            await _service.Payment.DeleteReadyPaymentByReservationId((int)reservationId);

            return Ok();
        }

        /// <summary>
        /// 결제 승인 요청
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Post : api/payments/kakao/approval
        ///     
        /// </remarks>
        /// <returns></returns>	    
        [HttpPost("approval", Name = "KakaoApproval")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> KakaoApproval([FromQuery] KakaoPayApprovalParameters parameters)
        {
            if (parameters == null
                || string.IsNullOrEmpty(parameters.PartnerOrderId)
                || string.IsNullOrEmpty(parameters.PartnerUserId)
                || string.IsNullOrEmpty(parameters.PgToken)
                || parameters.ReservationId < 1)
                throw new BadRequestException(ErrorCodes.PARAMETER_OBJECT_IS_NULL);

            Reservation reservation =
                await _service.Reservation.GetReservationById(parameters.ReservationId);

            //예약 정보 있는지 확인
            if (reservation == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            // 승인 요청 전에 유효한 예약인지 확인.
            // 이미 취소된 경우 에러메세지 return 
            if (reservation.Status == ReservationStatus.Cancelled)
                throw new ConflictException(ErrorCodes.TIME_OUT_FOR_PAYMENT_REQUEST);

            //이미 결제된 예약건인지 확인
            if (reservation.IsPaid != null)
                throw new ConflictException(ErrorCodes.PAYMET_ALREADY_EXISTS);

            Models.Payment response =
                await _service.KakaoPayment.Approval(parameters, reservation);
            
            //response
            return Ok(response);
        }

        /// <summary>
        /// 카카오 페이 결제 취소
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     POST: "api/payments/kakao/{reservationId}/cancel"
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("{reservationId}/cancel", Name = "KakaoCancelPayment")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> KakaoCancelPayment(int reservationId, [FromQuery] PaymentCancelParameters parameters)
        {
            Models.Payment canceledPayment = 
                await _service.KakaoPayment.Cancel(parameters, reservationId);

            return Ok(canceledPayment);
        }

    }
}
