using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class ReservationUnit
    {
        /// <summary>
        /// Unit Reservation id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Reservation Id
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// Child listing ID
        /// </summary>
        public int ListingUnitId { get; set; }

        /// <summary>
        /// 외부 예약번호
        /// </summary>
        public string ExternalReservationId { get; set; }

        /// <summary>
        /// 외부 예약 유닛 ID
        /// </summary>
        public string ExternalReservationUnitId { get; set; }

        public string GuestName { get; set; }

        public string GuestFirstName { get; set; }
        
        public string GuestLastName { get; set; }
        
        public int NumberOfGuests { get; set; }
        
        public int Adults { get; set; }
        
        public int Children { get; set; }
        
        public int Infants { get; set; }
        
        public int? Pets { get; set; }
        
        public float TotalPrice { get; set; }
    }
}
