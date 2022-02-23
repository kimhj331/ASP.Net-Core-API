using Newtonsoft.Json;
using System;

namespace ButlerLee.API.Models.Filters
{
    public class CalendarParameters : BaseParameters
    {
        private DateTime _startDate = DateTime.UtcNow.Date;

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate
        {
            get { return _startDate; }

            set
            {
                if (DateTime.UtcNow.Date < value.Date)
                    _startDate = value;
                else
                    _startDate = DateTime.UtcNow.Date;
            }
        }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime? EndDate { get; set; }
    }
}
