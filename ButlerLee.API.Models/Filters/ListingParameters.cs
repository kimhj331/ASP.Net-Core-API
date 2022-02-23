using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace ButlerLee.API.Models.Filters
{
    public class ListingParameters : PagenationParameters
    {
        /// <summary>
        /// 시작일
        /// </summary>
        [JsonProperty(PropertyName = "availabilityDateStart")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 종료일
        /// </summary>
        [JsonProperty(PropertyName = "availabilityDateEnd")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 게스트 수
        /// </summary>
        [JsonProperty(PropertyName = "availabilityGuestNumber")]
        private int? GuestNumber 
        {
            get 
            {
                int totalGuest = 0;

                if (Adults > 0)
                    totalGuest += Adults;

                if (Children > 0)
                    totalGuest += Children;

                return totalGuest;
            }
        }

        public int Adults { get; set; }

        public int Children { get; set; }
        ///// <summary>
        ///// 어메니티
        ///// </summary>
        //[JsonProperty(PropertyName = "amenities")]
        //public IEnumerable<int> Amenities { get; set; }

        /// <summary>
        /// 침대 수
        /// </summary>
        //[JsonProperty(PropertyName = "numberOfBeds")]
        //public short? NumberOfBeds { get; set; }

        /// <summary>
        /// 침실 수
        /// </summary>
        //[JsonProperty(PropertyName = "numberOfBedrooms")]
        //public short? NumberOfBedrooms { get; set; }

        /// <summary>
        /// 화장실 수
        /// </summary>
        //[JsonProperty(PropertyName = "numberOfBathrooms")]
        //public short? NumberOfBathrooms { get; set; }

        /// <summary>
        /// 부킹엔진 활성화여부
        /// </summary>
        [JsonProperty(PropertyName = "isBookingEngineActive")]
        private int IsBookingEngineActive { get; } = 1;

        /// <summary>
        /// 리소스포함여부
        /// </summary>
        [JsonProperty(PropertyName = "includeResources")]
        private int IncludeResources { get; } = 1;
    }
}
