using ButlerLee.API.Contracts.IClients;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ButlerLee.API.Clients
{
    public class ClientWrapper : IClientWrapper
    {
        private readonly IHttpClientFactory _clientFactory;
        public ClientWrapper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        private IAmenityClient _amenity;
        public IAmenityClient Amenity 
        {
            get 
            {
                if (_amenity == null)
                    _amenity = new AmenityClient(_clientFactory);

                return _amenity;
            }
        }
        private ICommonClient _common;
        public ICommonClient Common
        {
            get 
            {
                if (_common == null)
                    _common = new CommonClient(_clientFactory);

                return _common;
            }
        }
        private IListingClient _listing;
        public IListingClient Listing
        {
            get 
            {
                if (_listing == null)
                    _listing = new ListingClient(_clientFactory);

                return _listing;
            }
        }
        private IReservationClient _reservation;
        public IReservationClient Reservation
        {
            get
            {
                if (_reservation == null)
                    _reservation = new ReservationClient(_clientFactory);

                return _reservation;
            }
        }

        private ITossPayClient _tossPay;
        public ITossPayClient TossPay
        {
            get
            {
                if (_tossPay == null)
                    _tossPay = new TossPayClient(_clientFactory);

                return _tossPay;
            }
        }

        public IKakaoPayClient _kakaoPay;
        public IKakaoPayClient KakaoPay
        {
            get
            {
                if (_kakaoPay == null)
                    _kakaoPay = new KakaoPayClient(_clientFactory);

                return _kakaoPay;
            }
        }
    }
}
