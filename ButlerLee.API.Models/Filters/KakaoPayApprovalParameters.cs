using ButlerLee.API.Models.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ButlerLee.API.Models.Filters
{
    public class KakaoPayApprovalParameters : BaseParameters
    {
        [Required]
        [JsonProperty(PropertyName = "partnerOrderId")]
        public string PartnerOrderId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "partnerUserId")]
        public string PartnerUserId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "pgToken")]
        public string PgToken { get; set; }

        [Required]
        [JsonProperty(PropertyName = "reservationId")]
        public int ReservationId { get; set; }
    }

   
    
}
