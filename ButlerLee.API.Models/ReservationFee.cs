using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class ReservationFee
    {
        public int Id { get; set; }

        /// <summary>
        /// butlerLee 계정 ID
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 객실 ID
        /// </summary>
        public int ListingMapId { get; set; }

        /// <summary>
        /// 예약번호
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// Fee 이름
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 통화 단위
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 예약 총액의 몇 퍼센트인지
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// 이미 총 금액에 포함되었는지 여부
        /// </summary>
        public int IsIncluded { get; set; }

        /// <summary>
        /// 인원당 부가 여부
        /// </summary>
        public int IsPerNight { get; set; }

        /// <summary>
        /// 숙박일당 부가 여부
        /// </summary>
        public int IsPerPerson { get; set; }

        /// <summary>
        /// 외부에서 생성되었는지 여부
        /// </summary>
        public int IsImported { get; set; }

        /// <summary>
        /// 생성 날짜
        /// </summary>
        public DateTime InsertedOn { get; set; }

        /// <summary>
        /// 업데이트 날짜
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
