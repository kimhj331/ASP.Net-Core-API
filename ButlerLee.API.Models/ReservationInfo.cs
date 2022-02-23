using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ButlerLee.API.Models
{
    public class ReservationInfo
    {
        //public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public Listing Listing { get; set; }
        public Payment Payment { get; set; }

        public ReservationInfo()
        {
            Reservation = new Reservation();
            Listing = new Listing();
            Payment = new Payment();
        }
        public ReservationInfo(Reservation reservation, Listing listing, Payment payment)
        {
            Reservation = reservation;
            Listing = listing;
            Payment = payment;
        }
    }
}
