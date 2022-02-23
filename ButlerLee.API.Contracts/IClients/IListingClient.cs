using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IClients
{
    public interface IListingClient
    {
        Task<HostawayResponse<IEnumerable<Listing>>> GetListings(ListingParameters parameters);

        Task<HostawayResponse<Listing>> GetListingById(int listingId, bool includeResources = true);

        Task<HostawayResponse<IEnumerable<CalendarDay>>> GetCalender(int listingId, CalendarParameters parameters);

        Task<HostawayResponse<PriceDetail>> GetPrice(int listingId, PriceParameters parameters);
        
        Task<IEnumerable<PropertyType>> GetPropertyTypes();

        Task<string> GetListingNameById(int listingId);

        Task<Listing> GetBookableListing(int listingId, ListingParameters parameters);
    }
}
