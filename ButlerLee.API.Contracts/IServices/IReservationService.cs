using ButlerLee.API.Entities;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IServices
{
    public interface IReservationService 
    {
        /// <summary>
        /// 예약 목록
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PagedList<Reservation>> GetReservations(ReservationParameters parameters);

        /// <summary>
        /// 예약 정보
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        Task<Reservation> GetReservationById(int reservationId);

        /// <summary>
        /// 예약 생성
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        Task<Reservation> CreateReservation(Reservation reservation);

        /// <summary>
        /// 카드 예약 생성
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        Task<Reservation> CreateReservationWithCard(Reservation reservation);

        /// <summary>
        /// 예약 정보 수정
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        Task<Reservation> UpdateReservation(int reservationId, Reservation reservation);

        /// <summary>
        /// 예약 Paid Update
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="isPaid"></param>
        /// <returns></returns>
        Task<Reservation> UpdateReservation(int reservationId, string hostNote);

        /// <summary>
        /// 예약 취소
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        Task<Reservation> CancelReservaion(int reservationId, CancelledBy cancelledBy);

        /// <summary>
        /// 예약 삭제
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        Task DeleteReservation(int reservationId);

        /// <summary>
        ///  unpaidReservation 생성
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        Task CreateUnpaidReservation(int reservationId);

        /// <summary>
        /// 미결제 예약 취소
        /// </summary>
        /// <returns></returns>
        Task CancelUnpaidReservations();

        /// <summary>
        /// DB 트랜젝션에서 결제 완료된 목록은 삭제
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        Task DeleteUnpaidReservation(int reservationId);

        /// <summary>
        /// 예약 가능 여부 조회
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task CheckBookableReservation(Reservation reservation);
    }
}
