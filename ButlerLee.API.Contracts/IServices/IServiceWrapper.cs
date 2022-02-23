namespace ButlerLee.API.Contracts.IServices
{
    public interface IServiceWrapper
    {
        IListingService Listing { get; }
        IAmenityService Amenity { get; }
        IReservationService Reservation { get; }
        ICommonService Common { get; }
        IPaymentService Payment { get; }
        IKakaoPaymentService KakaoPayment { get; }
        ITossPaymentService TossPayment { get; }
    }
}
