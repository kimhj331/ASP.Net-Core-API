using ButlerLee.API.Models.Enumerations;
using System;
using System.Threading.Tasks;

namespace ButlerLee.API.Models.Filters
{
    public class PaymentParameters : PagenationParameters
    {
        /// <summary>
        /// 결제 일자
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// 객실 ID
        /// </summary>
        public int ListingId { get; set; }

        /// <summary>
        /// 결제 PG 명
        /// </summary>
        public PaymentGateway? Gateway { get; set; }
        
        /// <summary>
        /// 0 = 오름차순
        /// -1 = 내림차순
        /// </summary>
        public int Ordering { get; set; } 
    }
}
