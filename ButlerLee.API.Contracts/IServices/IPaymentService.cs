
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IServices
{
    public interface IPaymentService
    {
        #region Payment DB CRUD
        /// <summary>
        /// 결제정보 목록
        /// </summary>
        /// <returns></returns>
        Task<PagedList<Models.Payment>> GetPayments(PaymentParameters parameters);

        /// <summary>
        /// 아이디로 결제정보 찾기
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Models.Payment> GetPaymentById(uint id);

        /// <summary>
        /// 예약 id로 결제정보 찾기 결제정보
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        Task<Models.Payment> GetPaymentByReservationId(int reservationId);

        /// <summary>
        /// 결제정보 생성
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        Task<Models.Payment> CreatePayment(Models.Payment payment);

        /// <summary>
        /// 결제정보 업데이트
        /// </summary>
        /// <param name="id"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        Task<Models.Payment> UpdatePayment(uint id, Models.Payment payment);

        /// <summary>
        /// 상태 업데이트
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<Models.Payment> UpdatePaymentStatus(uint id, string status);

        /// <summary>
        /// 결제정보 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePayment(uint id);
        #endregion

        /// <summary>
        /// 결제 전 유효한 예약이면 예약 업데이트 혹은 생성
        /// </summary>
        /// <param name="method"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        Task<Reservation> UpdateReservationBeforePay(PaymentGateway method, Reservation reservation, float? cleaningFee);
        /// <summary>
        /// HostAway 일반카드결제
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        Task<Models.Payment> CreateHostAwayCardPayment(Reservation reservation);

        Task DeleteReadyPayments();

        Task DeleteReadyPaymentByReservationId(int reservationId);
    }
}
