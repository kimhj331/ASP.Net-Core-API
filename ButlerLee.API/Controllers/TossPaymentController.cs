using ButlerLee.API.Clients;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ButlerLee.API.Controllers
{
    [Produces("application/json")]
    [Route("api/payments/toss")]
    [ApiController]
    public class TossPaymentController : Controller
    {
        private readonly IServiceWrapper _service;
        public TossPaymentController(IServiceWrapper service)
        {
            _service = service;
        }

        /// <summary>
        /// 토스 페이 결제 승인 요청
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     POST: "api/payments/toss/success"
        ///     
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("success", Name = "TossApproval")]
        public async Task<IActionResult> TossApproval([FromQuery] TossPayApprovalParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.OrderId)
                || string.IsNullOrEmpty(parameters.PaymentKey)
                || parameters.ReservationId < 1
                || parameters.Amount < 0)
                throw new BadRequestException(ErrorCodes.REQUEST_IS_INVALID);

            Reservation reservation =
                await _service.Reservation.GetReservationById((int)parameters.ReservationId);

            if (reservation == null)
                throw new NotFoundException(ErrorCodes.RESERVATION_NOT_FOUND);

            // 승인 요청 전에 유효한 예약인지 확인. (이미 취소된 경우)
            // 이미 취소된 경우 에러메세지 return 
            if (reservation.Status == ReservationStatus.Cancelled)
                throw new ConflictException(ErrorCodes.TIME_OUT_FOR_PAYMENT_REQUEST);

            //이미 결제된 예약건인지 확인
            if (reservation.IsPaid != null)
                throw new ConflictException(ErrorCodes.PAYMET_ALREADY_EXISTS);

            Models.Payment payment =
                await _service.TossPayment.Approval(parameters, reservation);

            return Ok(payment);
        }

        /// <summary>
        /// 토스 페이 결제 취소 요청
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     POST: "api/payments/toss/{reservationId}/cancel?cancelReason=고객변심"
        ///     
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{reservationId}/cancel", Name = "TossCancelPayment")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> TossCancelPayment(int reservationId, [FromQuery] PaymentCancelParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.CancelReason))
                throw new BadRequestException(ErrorCodes.CANCEL_REASON_IS_NULL);

            Models.Payment payment =
                await _service.TossPayment.Cancel(parameters, reservationId);

            return Ok(payment);
        }
    }
}
