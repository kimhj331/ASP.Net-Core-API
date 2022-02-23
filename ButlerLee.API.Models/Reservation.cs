using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ButlerLee.API.Models
{
    public class Reservation : Card
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "ListingId is required")]
        [JsonProperty(PropertyName = "listingMapId")]
        public int ListingId { get; set; }

        [JsonProperty(PropertyName = "channelId")]
        private int ChannelId { get; set; } = 2000;

        public string Source { get; set; }

        public string ChannelName { get; set; }

        public string ReservationId { get; set; }

        public string HostawayReservationId { get; set; }

        public string ChannelReservationId { get; set; }

        public DateTime? ReservationDate { get; set; }

        public string GuestName
        {
            get
            {
                return $"{this.GuestFirstName} {this.GuestLastName}";
            }
        }

        [JsonProperty(PropertyName = "guestFirstName")]
        public string GuestFirstName { get; set; }

        [JsonProperty(PropertyName = "guestLastName")]
        public string GuestLastName { get; set; }

        public string GuestAddress { get; set; }

        public string GuestCity { get; set; }

        public string GuestZipCode { get; set; }

        public string GuestCountry { get; set; }

        [EmailAddress]
        [JsonProperty(PropertyName = "guestEmail")]
        public string GuestEmail { get; set; }

        [JsonProperty(PropertyName = "numberOfGuests")]
        public int NumberOfGuests 
        {
            get 
            {
                int totalGuests = 0;
                if (Adults.HasValue)
                    totalGuests += (int)Adults;

                if (Children.HasValue)
                    totalGuests += (int)Children;

                if (Infants.HasValue)
                    totalGuests += (int)Infants;

                return totalGuests;
            } 
        }

        public int? Adults { get; set; }

        public int? Children { get; set; }

        public int? Infants { get; set; }

        public int? Pets { get; set; }

        [JsonProperty(PropertyName = "arrivalDate")]
        [Required(ErrorMessage = "ArrivalDate is required")]
        public DateTime ArrivalDate { get; set; }

        [JsonProperty(PropertyName = "departureDate")]
        [Required(ErrorMessage = "DepartureDate is required")]
        public DateTime DepartureDate { get; set; }

        [Range(0, 24)]
        public int? CheckInTime { get; set; }

        [Range(0, 24)]
        public int? CheckOutTime { get; set; }

        public int? Nights 
        {
            get 
            {
               return DepartureDate.Subtract(ArrivalDate).Days; 
            } 
        }

        public string NationTelCode = "82";

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public float TotalPrice { get; set; }

        public int? IsPaid { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; } = "KRW";

        public string Comment { get; set; }

        public string HostNote { get; set; }

        public string GuestNote { get; set; }

        //public PaymentMethod PaymentMethod { get; set; }

        public CancelledBy? CancelledBy { get; set; }

        public IEnumerable<CustomFieldValue> CustomFieldValues { get; set; }

        public ReservationStatus Status { get; set; }

        /// <summary>
        /// 예약 요금 리스트
        /// </summary>
        //public IEnumerable<ReservationFee> ReservationFees { get; set; }

        /// <summary>
        /// Child Listing Unit Reservation 정보
        /// </summary>
        //public IEnumerable<ReservationUnit> ReservationUnits { get; set; }

        public bool IsValidationUpdateReservation()
        {
            const string korTimeZoneId = "Korea Standard Time";

            //string validation
            if(string.IsNullOrEmpty(GuestFirstName) 
                || string.IsNullOrEmpty(GuestLastName) 
                || string.IsNullOrEmpty(GuestEmail) 
                || string.IsNullOrEmpty(Phone)
                || string.IsNullOrEmpty(NationTelCode))
                return false;

            DateTime dateToCompare =
                 TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, korTimeZoneId);

            //date validation
            if (ArrivalDate < dateToCompare
                || DepartureDate < ArrivalDate)
                return false;

            if (Id < 1
                || ListingId < 1
                || Adults < 1
                || Children < 0
                || Infants < 0
                || NumberOfGuests < 1)
                return false;

            if (this.Phone.Contains("-"))
                this.Phone = this.Phone.Replace("-", "");

            //this.Phone = (this.Phone.Contains("0") && this.Phone.IndexOf("0") == 0) ? 
            //    (this.NationTelCode + this.Phone.Remove(0,1)) : (this.NationTelCode + this.Phone);

            var unspecialCharacterCheck = new Regex(@"[^a-zA-Z0-9가-힣]");

            var emailCheck = 
                new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            var phoneCheck =
                //new Regex(@"(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}");
                new Regex(@"01{1}[016789]{1}[0-9]{7,8}");

            //특수문자 없으면 true, 있으면 false
            if (unspecialCharacterCheck.IsMatch(this.GuestFirstName)
                 || unspecialCharacterCheck.IsMatch(this.GuestLastName))
                return false;

            //이메일 문법과 맞지 않을경우
            if (!emailCheck.IsMatch(this.GuestEmail))
                return false;

            //휴대폰번호 문법과 맞지 않을경우
            if (!phoneCheck.IsMatch(this.Phone))
                return false;
            // throw new BadRequestException($"{this.Phone}");

            return true;
        }
    }

    public class UpdateReservation : Card //선점 후에 카드정보 업데이트 할때 사용
    {
        [JsonProperty(PropertyName = "listingMapId")]
        public int? ListingId { get; set; }
        public string GuestName
        {
            get
            {
                return $"{this.GuestFirstName} {this.GuestLastName}";
            }
        }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public string GuestAddress { get; set; }
        public string GuestCity { get; set; }
        public string GuestZipCode { get; set; }
        public string GuestCountry { get; set; }
        public string GuestEmail { get; set; }
        public int? NumberOfGuests { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
        public int? Pets { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public int? CheckInTime { get; set; } 
        public int? CheckOutTime { get; set; } 
        public int? Nights { get; set; }
        public string Phone { get; set; }
        public float? TotalPrice { get; set; }
        public int? IsPaid { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }
        public string GuestNote { get; set; }
        public CancelledBy? CancelledBy { get; set; }
        public IEnumerable<CustomFieldValue> CustomFieldValues { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
