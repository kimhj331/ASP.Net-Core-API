using ButlerLee.API.Models.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class CalendarDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? IsAvailable { get; set; }
        public CalendarStatus Status { get; set; }
        public float? Price { get; set; }
        
        [JsonProperty(PropertyName = "minimumStay")]
        public int? MinStay { get; set; }
        
        [JsonProperty(PropertyName = "maximumStay")]
        public int? MaxStay { get; set; }
        public string Note { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
        public BookStatus BookStatus { get; set; }
    }
}
