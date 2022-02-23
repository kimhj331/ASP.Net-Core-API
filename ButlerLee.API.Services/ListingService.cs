using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Exceptions;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ButlerLee.API.Services
{
    public class ListingService : IListingService
    {
        private readonly IClientWrapper _client;
        public ListingService(IClientWrapper client) => this._client = client;

        /// <summary>
        /// 숙소 목록
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<PagedList<Listing>> GetListings(ListingParameters parameters)
        {
            var response = await this._client.Listing.GetListings(parameters);

            IEnumerable<PropertyType> properties = await this._client.Listing.GetPropertyTypes();

            foreach (var listing in response.Result)
            {
                if (listing.PropertyTypeId != null)
                {
                    var selectedPropertyType =
                        properties.SingleOrDefault(o => o.Id.Equals((int)listing.PropertyTypeId));

                    if (selectedPropertyType != null)
                        listing.PropertyType =
                            selectedPropertyType.Name;

                    if (listing.CustomFieldValues != null
                        && listing.CustomFieldValues.Any())
                        listing.CustomFieldValues = new List<CustomFieldValue>(listing.CustomFieldValues.Where(o => o.Value != null && o.CustomField.IsPublic == 1));
                }
            }
          
            return new PagedList<Listing>(response.Result, response.Count, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Listing> GetBookableListing(int listingId, ListingParameters parameters)
        {
            Listing listing = 
                await this._client.Listing.GetBookableListing(listingId, parameters);
            
            return listing;
        }

        /// <summary>
        /// 숙소 정보
        /// </summary>
        /// <param name="listingId">Id</param>
        /// <returns></returns>
        public async Task<Listing> GetListingById(int listingId)
        {
            IEnumerable<PropertyType> properties = await this._client.Listing.GetPropertyTypes();

            var response = await this._client.Listing.GetListingById(listingId);

            if (response.Result == null)
                throw new NotFoundException(ErrorCodes.LISTING_NOT_FOUND);

            if (response.Result.PropertyTypeId != null)
            {
                var selectedPropertyType =
                    properties.SingleOrDefault(o => o.Id.Equals((int)response.Result.PropertyTypeId));

                if (selectedPropertyType != null)
                    response.Result.PropertyType =
                        selectedPropertyType.Name;

                if (response.Result.CustomFieldValues != null
                        && response.Result.CustomFieldValues.Any())
                    response.Result.CustomFieldValues = new List<CustomFieldValue>(response.Result.CustomFieldValues.Where(o => o.Value != null && o.CustomField.IsPublic == 1));
            }
        
            return response.Result;
        }

        public async Task<IEnumerable<CalendarDay>> GetCalender(int listingId, CalendarParameters parameters)
        {
            var response = await this._client.Listing.GetCalender(listingId, parameters);

            CalendarDay preCalendarDay = null;

            foreach (var day in response.Result)
            {
                if (day.Status == CalendarStatus.Available)
                {
                    day.BookStatus = BookStatus.Bookable;
                    preCalendarDay = day;
                    continue;
                }

                if (preCalendarDay!= null 
                    && ( day.Status == CalendarStatus.Reserved || day.Status == CalendarStatus.Blocked)
                    && preCalendarDay.Status == CalendarStatus.Available)
                    day.BookStatus = BookStatus.CheckOutOnly;

                if(preCalendarDay!=null
                    && day.Status == CalendarStatus.Available
                    && preCalendarDay.Status == CalendarStatus.Reserved)
                    day.BookStatus = BookStatus.CheckInOnly;

                preCalendarDay = day;
            }

            return response.Result;
        }

        public async Task<PriceDetail> GetPrice(int listingId, PriceParameters parameters)
        {
            var response = await this._client.Listing.GetPrice(listingId, parameters);
            
            if (response.Result != null)
            {
                int nights = 
                    parameters.EndingDate.Date
                        .Subtract(parameters.StartingDate.Date)
                        .Days;

                response.Result.Nights = nights;
               
                double totalPrice = Math.Round(response.Result.TotalPrice);
                response.Result.TotalPrice = totalPrice;
            }

            return response.Result;
        }
    }
}
