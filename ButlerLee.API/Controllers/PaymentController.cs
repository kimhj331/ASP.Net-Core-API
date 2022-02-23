using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Entities;
using ButlerLee.API.Extensions;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
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
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IServiceWrapper _service;
        //private readonly int _testListingId = 88635;

        public PaymentController(IServiceWrapper service)
        {
            _service = service;
        }

        #region Payment DB CRUD 관련
        /// <summary>
        /// 결제 목록
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/payments
        ///     
        /// </remarks>
        /// <returns></returns>	        
        [HttpGet("", Name = "GetPayments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPayments([FromQuery]PaymentParameters parameters)
        {
            if (parameters.PageNumber < 0 || parameters.PageSize < 0)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID);

            PagedList<Models.Payment> payments =
               await _service.Payment.GetPayments(parameters);

            if (payments == null)
                throw new NotFoundException(ErrorCodes.PAYMENT_NOT_FOUND);

            var metadata = new
            {
                payments.TotalCount,
                payments.PageSize,
                payments.CurrentPage,
                payments.TotalPages,
                payments.HasNext,
                payments.HasPrevious
            };

            Response.Headers.Add("x-pagination", metadata.ToJson());
            
            return Ok(payments);
        }

        /// <summary>
        /// 결제 정보
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: api/payments/{id}
        ///    
        /// </remarks>
        /// <returns></returns>	        
        [HttpGet("{id}", Name = "GetPaymentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetPaymentById(uint id)
        {
            var payment =
                await this._service.Payment.GetPaymentById(id);

            if (payment == null)
                throw new NotFoundException(ErrorCodes.PAYMENT_NOT_FOUND);

            return Ok(payment);
        }

        /// <summary>
        /// 결제 정보 생성
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: api/payments
        ///    
        /// </remarks>
        /// <returns></returns>	        
        [HttpPost("", Name = "CreatePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CreatePayment([FromBody] Models.Payment payment)
        {
            if (payment == null)
                throw new BadRequestException(ErrorCodes.PAYMENT_IS_NULL);

            if (ModelState.IsValid == false)
                throw new BadRequestException(ErrorCodes.REQUIRED_PRARMETER_IS_NULL);

            Models.Payment createdPayment = await _service.Payment.CreatePayment(payment);

            return Ok(createdPayment);
        }


        /// <summary>
        /// 결제 정보 수정
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT: api/payments/{id}
        ///    
        /// </remarks>
        /// <returns></returns>	        
        [HttpPut("{id}", Name = "UpdatePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdatePayment(uint id, [FromBody] Models.Payment payment)
        {
            if (payment == null)
                throw new BadRequestException(ErrorCodes.PAYMENT_IS_NULL);

            var originPayment = await GetPaymentById(id);

            if (originPayment == null)
                throw new NotFoundException(ErrorCodes.PAYMENT_NOT_FOUND);

            Models.Payment updatedPayment = await _service.Payment.UpdatePayment(id, payment);

            return Ok(updatedPayment);
        }

        /// <summary>
        /// 결제 상태 업데이트
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT: api/payments/{id}/status
        ///     
        /// </remarks>
        /// <returns></returns>	        
        [HttpPut("{id}/status", Name = "UpdateStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateStatus(uint id, [FromQuery] string status)
        {
            var originPayment = await GetPaymentById(id);

            if (originPayment == null)
                throw new NotFoundException(ErrorCodes.PAYMENT_NOT_FOUND);

            Models.Payment updatedPayment = await _service.Payment.UpdatePaymentStatus(id, status);

            return Ok(updatedPayment);
        }

        /// <summary>
        /// 결제 정보 삭제
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE: api/payments/{id}
        ///    
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeletePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeletePayment(uint id)
        {
            var originPayment = await GetPaymentById(id);

            if (originPayment == null)
                throw new NotFoundException(ErrorCodes.PAYMENT_NOT_FOUND);

            await _service.Payment.DeletePayment(id);

            return Ok();
        }
        #endregion

        /// <summary>
        /// 유효한 예약인지(선점 되어있는지 확인)
        /// </summary>
        /// <remarks>
        /// 
        ///     유효한 예약인지(선점 되어있는지 확인)
        ///     가능하다면 unpaid reservation 삭제,
        ///     reservation 업데이트, 혹은 생성
        ///     불가능 : Error발생
        /// 
        /// Sample request:
        /// 
        ///     POST: api/payments/validation-before-pay
        ///     {
        ///         "id" : reservationId,
        ///         "listingId" : 88635,
        ///         "adults" : 2,
        ///         "children" : 0,
        ///         "arrivalDate": "2023-09-15",
        ///         "departureDate": "2023-09-16",
        ///         "guestFirstName": "HeeJi",           
        ///         "guestLastName": "Kim",
        ///         "guestEmail": "hjkim@saladsoft.com",
        ///         "guestAddress" : "서울시 성동구",
        ///         "guestCity" : "서울특별시",
        ///         "guestZipCode" : "12345",
        ///         "guestCountry" : "KR",
        ///         "currency" : "KRW",
        ///         "phone": "+821053418210",
        ///         "totalPrice": 200000,
        ///         "guestNote": "깨끗하게 청소해주세요",
        ///         "cardNumber": "5531770022327808",
        ///         "cardUserName": "heejiKim",
        ///         "expirationMonth": "02",
        ///         "expirationYear": "27",
        ///         "cvc" : "123" 
        ///     }
        /// </remarks>
        /// <returns></returns>
        [HttpPost("validation-before-pay", Name = "UpdateReservationBeforePay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReservationBeforePay([FromQuery] PaymentGateway method, [FromBody] Reservation reservation)
        {
            if (reservation == null || reservation.IsValidationUpdateReservation() == false)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID);

            //CardPayment일 경우는 Card Validation Check
            if (method == PaymentGateway.DirectCreditCard && reservation.IsCreditCardInfoValid() == false) 
                throw new BadRequestException(ErrorCodes.INVALID_CREDIT_CARD);

            Listing listing = await _service.Listing.GetListingById(reservation.ListingId);

            if (listing == null)
                throw new NotFoundException(ErrorCodes.UNAVAILABLE_LISTING);

            if (listing.MaxGuestNumber < reservation.NumberOfGuests)
                throw new BadRequestException(ErrorCodes.ROOM_CAPACITY_HAS_EXCEEDED);

            //가격정보 확인
            Reservation result = 
                await _service.Payment.UpdateReservationBeforePay(method, reservation, listing.CleaningFee);
            
            return Ok(result);
        }

        /// <summary>
        /// 일반 카드 결제
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     POST: api/payments/host-away
        ///     {
        ///         "id": 11952332,
        ///         "listingId": 88635,
        ///         "channelId": 2000,
        ///         "source": "reservation",
        ///         "channelName": "direct",
        ///         "reservationId": "34449-88635-2000-4705423658",
        ///         "hostawayReservationId": "11952332",
        ///         "channelReservationId": "34449-88635-2000-4705423658",
        ///         "reservationDate": "2022-01-18T05:12:14",
        ///         "guestName": "HeeJi Kim",
        ///         "guestFirstName": "HeeJi",
        ///         "guestLastName": "Kim",
        ///         "guestAddress": "서울시 성동구",
        ///         "guestCity": "서울특별시",
        ///         "guestZipCode": null,
        ///         "guestCountry": "KR",
        ///         "guestEmail": "hjkim@saladsoft.com",
        ///         "numberOfGuests": 2,
        ///         "adults": 2,
        ///         "children": 0,
        ///         "infants": 0,
        ///         "pets": null,
        ///         "arrivalDate": "2023-09-15T00:00:00",
        ///         "departureDate": "2023-09-16T00:00:00",
        ///         "checkInTime": 15,
        ///         "checkOutTime": 11,
        ///         "nights": 1,
        ///         "phone": "+821053418210",
        ///         "totalPrice": 200000,
        ///         "isPaid": 0,
        ///         "currency": "KRW",
        ///         "comment": null,
        ///         "hostNote": "BL-NPFNbN",
        ///         "guestNote": "깨끗하게 청소해주세요",
        ///         "cancelledBy": null,
        ///         "customFieldValues": [],
        ///         "status": "Modified",
        ///         "cardNumber": null,
        ///         "cardEndingNumber": 4321,
        ///         "cardUserName": null,
        ///         "expirationMonth": "12",
        ///         "expirationYear": "25",
        ///         "cvc": "123"
        ///     }
        /// </remarks>
        /// <returns></returns>
        [HttpPost("host-away", Name = "CreateHostAwayCardPayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHostAwayCardPayment([FromBody] Reservation reservation)
        {
            if (reservation == null)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID); 

            //if (reservation.ListingId.Equals(_testListingId) == false)
            //    throw new BadRequestException(ErrorCodes.NOT_TEST_LISTING);

            Reservation originReservation = await _service.Reservation.GetReservationById(reservation.Id);

            if (originReservation == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            if (originReservation.Status == ReservationStatus.Cancelled)
                throw new ConflictException(ErrorCodes.ALREADY_CANCELED_RESERVATION);

            if (originReservation.IsPaid != null)
                throw new ConflictException(ErrorCodes.PAYMET_ALREADY_EXISTS);

            Models.Payment result = await _service.Payment.CreateHostAwayCardPayment(originReservation);

            if (result == null)
                throw new ConflictException(ErrorCodes.HOST_AWAY_CARD_PAYMENT_FAIL);

            return Ok(result);
        }
    }
}
