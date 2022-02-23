using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class ListingAmenity
    {
        [JsonProperty(PropertyName = "amenityId")]
        public int Id { get; set; }

        //public int AmenityId { get; set; }
        [JsonProperty(PropertyName = "amenityName")]
        public string Name { get; set; }
    }

    public class Amenity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
