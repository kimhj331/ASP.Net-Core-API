using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ButlerLee.API.Models.Filters
{
    public class TossPayApprovalParameters
    {
        /// <summary>
        /// 결제 건에 대한 고유한 키 값입니다.
        /// </summary>
        [Required]
        public string PaymentKey { get; set; }
        
        /// <summary>
        /// 가맹점에서 발급된 고유한 주문 ID값 입니다. 
        /// 결제창을 열 때 requestPayment에 담아 보낸 값입니다.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 실제로 결제된 금액입니다.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }

        [Required]
        public int ReservationId { get; set; }
    }

    public class TossPayCancelParameters
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
