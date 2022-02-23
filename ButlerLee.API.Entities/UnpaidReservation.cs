using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ButlerLee.API.Entities
{
    public partial class UnpaidReservation
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime LimitStartDate { get; set; }
        public DateTime? CancelDate { get; set; }
    }
}
