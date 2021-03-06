
using ButlerLee.API.Models.Enumerations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ButlerLee.API.Models
{
    public class Payment 
    {
        public uint Id { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public int ReservationId { get; set; }
        public string ReservationNo { get; set; }
        public string PaymentKey { get; set; }
        public int ListingId { get; set; }
        public string OrderName { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }    
        public string Status { get; set; }
        public string EasyPay { get; set; }
        public string Method { get; set; }
        public int TotalAmount { get; set; }
        public int TaxFreeAmount { get; set; }
        public int CanceledAmount { get; set; }
        public string Currency { get; set; }
        public int DiscountAmount { get; set; }
        public string CardCompany { get; set; }
        public string CardNumber { get; set; }
        public int? CardInstallmentPlanMonths { get; set; }
        public string CardApproveNo { get; set; }
        public string CardType { get; set; }
        public string CardOwnerType { get; set; }
        public string CardReceiptUrl { get; set; }
        public bool? IsInsterestFree { get; set; }
        public string CardAcquireStatus { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Payment() { }
    }
}
