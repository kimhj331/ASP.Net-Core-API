using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IClientWrapper _client;

        public AmenityService(IClientWrapper client) => this._client = client;

        /// <summary>
        /// 어메니티 목록
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Amenity>> GetAmenities()
        {
            var response = await this._client.Amenity.GetAmenities();
            return response.Result;
        }

        /// <summary>
        /// 어메니티 정보
        /// </summary>
        /// <param name="listingId">Id</param>
        /// <returns></returns>
        public async Task<Amenity> GetAmenityById(int listingId)
        {
            var response = await this._client.Amenity.GetAmenityById(listingId);
            return response.Result;
        }
    }
}
