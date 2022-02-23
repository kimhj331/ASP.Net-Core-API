using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ButlerLee.API.Models.Filters
{
    public class PaymentCancelParameters
    {
        [Required]
        public string PaymentKey { get; set; }

        [Required]
        public int CancelAmount { get; set; }

        [Required]
        public int CancelTaxFreeAmount { get; set; }
        
        public string CancelReason { get; set; }
    }
}
