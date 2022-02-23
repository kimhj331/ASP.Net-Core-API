using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class BedType
    {
        public int Id { get; set; }

        public int BedTypeId { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Qty { get; set; }

        public int BedroomNumber { get; set; }
    }
}
