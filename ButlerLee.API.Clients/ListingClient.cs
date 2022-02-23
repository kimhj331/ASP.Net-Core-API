using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using Newtonsoft.Json;
using System;
//using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ButlerLee.API.Clients
{
    public class ListingClient: IListingClient
    {
        private readonly HttpClient httpClient;
        private const string defaultUri = "listings";
        //HostAway

        public ListingClient(IHttpClientFactory clientFactory) => this.httpClient = clientFactory.CreateClient(nameof(HostAwayClient));
        
        /// <summary>
        /// 숙소 목록
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<HostawayResponse<IEnumerable<Listing>>> GetListings(ListingParameters parameters)
        {
            //HttpResponseMessage response =
            // await this.httpClient.GetAsync($"{defaultUri}?{parameters.GetQueryString()}&amenities%5B%5D=4");

            HttpResponseMessage response =
                await this.httpClient.GetAsync($"{defaultUri}?{parameters.GetQueryString()}");

            await response.EnsureSuccessStatusCodeAsync();
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<IEnumerable<Listing>>>(content);
        }

        /// <summary>
        /// 숙소 정보
        /// </summary>
        /// <param name="listingId">Id</param>
        /// <returns></returns>
        public async Task<HostawayResponse<Listing>> GetListingById(int listingId, bool includeResources=true)
        {
            string queryString = string.Empty;
           
            if (includeResources == false)
                queryString = "?includeResources=0";

            HttpResponseMessage response =
                await this.httpClient.GetAsync($"{defaultUri}/{listingId}{queryString}");

            if (response.IsSuccessStatusCode == false)
            {
                HostawayResponse<Listing> hostawayResponse =
                     new HostawayResponse<Listing>(ResponseStatus.Fail, null);

                return hostawayResponse;
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Listing>>(content);
        }
       
        /// <summary>
        /// 숙소별 달력정보
        /// </summary>
        /// <param name="listingId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<HostawayResponse<IEnumerable<CalendarDay>>> GetCalender(int listingId, CalendarParameters parameters)
        {
            //listings/{id}/calendar
            HttpResponseMessage response =
                await this.httpClient.GetAsync($"{defaultUri}/{listingId}/calendar?{parameters.GetQueryString()}");

            await response.EnsureSuccessStatusCodeAsync();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<IEnumerable<CalendarDay>>>(content);
        }

        /// <summary>
        /// 숙소 가격정보
        /// </summary>
        /// <param name="listingId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<HostawayResponse<PriceDetail>> GetPrice(int listingId, PriceParameters parameters)
        {
            //listings/{id}/calendar/priceDetails
            HttpResponseMessage response = 
                await this.httpClient.GetAsync($"{defaultUri}/{listingId}/calendar/priceDetails?{parameters.GetQueryString()}");

            await response.EnsureSuccessStatusCodeAsync();

            var content = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<HostawayResponse<PriceDetail>>(content);
        }

      
        public async Task<IEnumerable<PropertyType>> GetPropertyTypes()
        {
            HttpResponseMessage response =
                await this.httpClient.GetAsync($"propertyTypes");
            
            await response.EnsureSuccessStatusCodeAsync();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HostawayResponse<IEnumerable<PropertyType>>>(content).Result;
        }

        public async Task<string> GetListingNameById(int listingId)
        {
            var response = await this.GetListingById(listingId);

            return response.Result.Name;
        }

        public async Task<Listing> GetBookableListing(int listingId, ListingParameters parameters)
        {
            parameters.PageSize = 100;
            parameters.PageNumber = 1;

            var response = await this.GetListings(parameters);

            if (response.Result == null
                || response.Result.Any() == false)
                throw new ConflictException(ErrorCodes.UNBOOKABLE_LISTING);

            Listing bookableListing = response.Result.SingleOrDefault(listing => listing.Id == listingId);

            return bookableListing;
        }
    }
}
