using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace ButlerLee.API.Models.Filters
{
    public class PriceParameters : BaseParameters
    {
        [Required]
        [JsonProperty(PropertyName = "startingDate")]
        public DateTime StartingDate { get; set; } 

        [Required]
        [JsonProperty(PropertyName = "endingDate")]
        public DateTime EndingDate { get; set; } 

        
        [JsonProperty(PropertyName = "numberOfGuests")]
        private int NumberOfGuests
        {
            get
            {
                int totalGuests = 0;

                if (Adults.HasValue)
                    totalGuests += (int)Adults;

                if (Children.HasValue)
                    totalGuests += (int)Children;

                //if (Infants.HasValue)
                //    totalGuests += (int)Infants;

                return totalGuests;
            }
        }

        [Required]
        public int? Adults { get; set; }
        public int? Children { get; set; }

        //public int? Infants { get; set; }

        //[JsonProperty(PropertyName = "couponName")]
        //public string CouponName { get; set; }


    }
}
