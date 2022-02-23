using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models.Filters
{
    //ListingId = 89864 &
    //ArrivalDate= 2022-01-01&
    //DepartureDate= 2022-01-03
    public class ReservationParameters : PagenationParameters
    {
        [JsonProperty(PropertyName = "listingId")]
        public int? ListingId { get; set; }

        [JsonProperty(PropertyName = "assigneeUserId")]
        public int? UserId { get; set; }

        [JsonProperty(PropertyName = "match")]
        public string GuestName { get; set; }

        [JsonProperty(PropertyName = "arrivalStartDate")]
        public DateTime? ArrivalStartDate { get; set; }

        [JsonProperty(PropertyName = "departureStartDate")]
        public DateTime? DepartureStartDate { get; set; }

        [JsonProperty(PropertyName = "arrivalEndDate")]
        private DateTime? ArrivalEndDate
        {
            get
            {
                return this.ArrivalStartDate;
            }
        }

        [JsonProperty(PropertyName = "departureEndDate")]
        private DateTime? DepartureEndDate
        {
            get
            {
                return this.DepartureStartDate;
            }
        }

        [JsonProperty(PropertyName = "channelId")]
        private int ChannelId { get; set; }// = 2000;
    }
}
