
using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IServices
{
    public interface IListingService
    {
        /// <summary>
        /// 숙소 목록
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PagedList<Listing>> GetListings(ListingParameters parameters);

        /// <summary>
        /// 예약가능 숙소 검색
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Listing> GetBookableListing(int listingId, ListingParameters parameters);

        /// <summary>
        /// 숙소 정보
        /// </summary>
        /// <param name="listingId"></param>
        /// <returns></returns>
        Task<Listing> GetListingById(int listingId);

        Task<IEnumerable<CalendarDay>> GetCalender(int listingId, CalendarParameters parameters);

        Task<PriceDetail> GetPrice(int listingId, PriceParameters parameters);
    }
}
