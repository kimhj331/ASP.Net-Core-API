using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Contracts.IClients
{
    public interface IClientWrapper
    {
        IAmenityClient Amenity { get; }
        ICommonClient Common { get; }
        IListingClient Listing { get; }
        IReservationClient Reservation { get; }
        ITossPayClient TossPay { get; }
        IKakaoPayClient KakaoPay { get; }
    }
}
