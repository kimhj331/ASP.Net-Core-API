using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class PriceDetail
    {
        public int Nights { get; set; }
        public int NumberOfGuests { get; set; }
        public double Price { get; set; }
        //public float RefundableDamageDeposit { get; set; }
        private double WeeklyDiscount { get; set; }
        private double MonthlyDiscount { get; set; }
        public float PriceForExtraPerson { get; set; }
        public double CleaningFee { get; set; }
        public double TotalPrice { get; set; }
        
        
        
        //public float CouponDiscount { get; set; }
        //public float CheckinFee { get; set; }
        //public float SalesTax { get; set; }
        //public float HotelTax { get; set; }
        //public float Vat { get; set; }
        //public float LodgingTax { get; set; }
        //public float TransientOccupancyTax { get; set; }
        //public float CityTax { get; set; }
        //public float RoomTax { get; set; }
        //public float OtherTaxes { get; set; }
        
        
        #region Total Price에 자동으로 포함되지 않는 항목
        private double AdditionalCleaningFee { get; set; }
        private double ParkingFee { get; set; }
        private double TowelChangeFee { get; set; }
        private double MidstayCleaningFee { get; set; }
        private double RoomRequestFee { get; set; }
        private double ReservationChangeFee { get; set; }
        private double LateCheckoutFee { get; set; }
        private double OtherFees { get; set; }
        private double ShareholderDiscount { get; set; }
        private double LastMinuteDiscount { get; set; }
        private double EmployeeDiscount { get; set; }
        private double OtherSpecialDiscount { get; set; }
        #endregion

    }
}
