using AutoMapper;
using ButlerLee.API.Contracts;
using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Models;

namespace ButlerLee.API.Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        internal IClientWrapper _client;
        internal IMapper _mapper;
        internal IRepositoryWrapper _repository;
        internal ILoggerManager _logger;
        
        internal Configurations _configurations;
       
      

        public ServiceWrapper(IClientWrapper client, IMapper mapper, Configurations configurations, IRepositoryWrapper repository, ILoggerManager logger)
        {
            _repository = repository;
            _mapper = mapper;
            _configurations = configurations;
            _logger = logger;
            _client = client;
        }

        private IListingService _listing;
        public IListingService Listing
        {
            get
            {
                if (_listing == null)
                    _listing = new ListingService(_client);

                return _listing;
            }
        }

        private IAmenityService _amenity;
        public IAmenityService Amenity
        {
            get
            {
                if (_amenity == null)
                    _amenity = new AmenityService(_client);

                return _amenity;
            }
        }
        private IReservationService _reservation;
        public IReservationService Reservation
        {
            get
            {
                if (_reservation == null)
                    _reservation = new ReservationService(_repository, _client, _logger, _mapper);
                return _reservation;
            }
        }
        private ICommonService _common;
        public ICommonService Common
        {
            get
            {
                if (_common == null)
                    _common = new CommonService(_client);
                return _common;
            }
        }

        private IPaymentService _payment;
        public IPaymentService Payment
        {
            get
            {
                if (_payment == null)
                    _payment = new PaymentService(_repository, _client, _mapper, _logger, _configurations);
                return _payment;
            }
        }

        private IKakaoPaymentService _kakaoPayment;
        public IKakaoPaymentService KakaoPayment
        {
            get
            {
                if (_kakaoPayment == null)
                    _kakaoPayment = new KakaoPaymentService(_repository, _client, _configurations, _mapper, _logger);
                return _kakaoPayment;
            }
        }

        private ITossPaymentService _tossPayment;
        public ITossPaymentService TossPayment
        {
            get
            {
                if (_tossPayment == null)
                    _tossPayment = new TossPaymentService(_repository, _client, _mapper, _logger);
                return _tossPayment;
            }
        }

    }
}
