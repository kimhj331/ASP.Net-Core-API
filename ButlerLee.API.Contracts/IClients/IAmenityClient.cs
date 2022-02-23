using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IClients
{
    public interface IAmenityClient
    {
        Task<HostawayResponse<IEnumerable<Amenity>>> GetAmenities();

        Task<HostawayResponse<Amenity>> GetAmenityById(int listingId);
    }
}
