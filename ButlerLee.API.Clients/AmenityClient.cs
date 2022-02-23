using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using Newtonsoft.Json;
//using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ButlerLee.API.Clients
{
    public class AmenityClient : IAmenityClient
    {
        private readonly HttpClient httpClient;
        private const string defaultUri = "amenities";

        public AmenityClient(IHttpClientFactory clientFactory) => this.httpClient = clientFactory.CreateClient(nameof(HostAwayClient));

        /// <summary>
        /// 어메니티 목록
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<HostawayResponse<IEnumerable<Amenity>>> GetAmenities()
        {
            HttpResponseMessage response = 
                await this.httpClient.GetAsync($"{defaultUri}");

            await response.EnsureSuccessStatusCodeAsync();

            var content = await response.Content.ReadAsStringAsync();
           
            return JsonConvert.DeserializeObject<HostawayResponse<IEnumerable<Amenity>>>(content);
         }

        /// <summary>
        /// 어메니티 정보
        /// </summary>
        /// <param name="amenityId">Id</param>
        /// <returns></returns>
        public async Task<HostawayResponse<Amenity>> GetAmenityById(int amenityId)
        {
            HttpResponseMessage response =
                await this.httpClient.GetAsync($"{defaultUri}/{amenityId}");

            await response.EnsureSuccessStatusCodeAsync();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Amenity>>(content);
        }

      
    }
}
