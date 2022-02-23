using ButlerLee.API.Models.Enumerations;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace ButlerLee.API.Models
{
    [SwaggerSchema(Required = new[] { "Listing" })]
    public class Listing
    {
        public int Id { get; set; }

        public int? PropertyTypeId { get; set; }
        
        public string PropertyType { get; set; }
        
        public string Name { get; set; }

        public string EngName
        {
            get => GetCustomValueById(CustomValueId.EngName);
        }

        public string Highlight
        {
            get => GetCustomValueById(CustomValueId.Highlight);
        }
        public string EngHighlight
        {
            get => GetCustomValueById(CustomValueId.EngHighlight);
        }
      
        public string Description { get; set; }

        public string EngDescrption
        {
            get => GetCustomValueById(CustomValueId.EngHighlight);
        }

        public string ThumbnailUrl { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public float Price { get; set; }

        public float? PriceForExtraPerson { get; set; }

        public float? CleaningFee
        {
            get
            {
                float.TryParse(GetCustomValueById(CustomValueId.CleaningFee), out float cleaningFee);

                if (cleaningFee > 0)
                    return cleaningFee;

                return 0;
            }
        
        }

        [JsonProperty(PropertyName = "personCapacity")]
        public int? MaxGuestNumber { get; set; }

        [JsonProperty(PropertyName= "guestsIncluded")]
        public int? StandardGuestNumber { get; set; }

        private double? _latitude;

        [JsonProperty(PropertyName = "lat")]
        public double? Latitude
        {
            get
            {
                double.TryParse(GetCustomValueById(CustomValueId.Latitude), out double customLatitude);
                
                if(customLatitude > 0)
                    return customLatitude;

                return _latitude;
            }

            set => _latitude = value;
        }
        private double? _longitude;

        [JsonProperty(PropertyName = "lng")] 
        public double? Longitude 
        {
            get
            {
                double.TryParse(GetCustomValueById(CustomValueId.Longitude), out double customLongtitde);
                if (customLongtitde > 0)
                    return customLongtitde;

                return _longitude;
            }

            set => _longitude = value;
        }

        public int? CheckInTimeStart { get; set; }

        public int? CheckInTimeEnd { get; set; }

        public int? CheckOutTime { get; set; }

        public string RoomType { get; set; }

        public int BedroomsNumber { get; set; }
        
        public int BedsNumber { get; set; }
        
        public int BathroomsNumber { get; set; }
        
        public string BathroomType { get; set; }

        [JsonProperty(PropertyName = "listingAmenities")]
        public List<ListingAmenity> Amenities { get; set; }

        [JsonProperty(PropertyName = "listingBedTypes")]
        public List<BedType> BedTypes { get; set; }

        [JsonProperty(PropertyName = "listingImages")]
        public List<Image> Images { get; set; }

        [JsonProperty(PropertyName = "customFieldValues")]
        public List<CustomFieldValue> CustomFieldValues { get; set; }

        
        public string GetCustomValueById(CustomValueId customValueId) 
        {
           
            if (this.CustomFieldValues != null
                   && this.CustomFieldValues.Any())
            {
                var customField = this.CustomFieldValues.
                    SingleOrDefault(o => o.CustomFieldId == (int)customValueId);

                if (customField != null)
                    return (string)customField.Value;
            }
            return null;
        }
    }
}
