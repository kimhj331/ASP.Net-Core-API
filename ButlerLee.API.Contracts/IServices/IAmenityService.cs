
using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IServices
{
    public interface IAmenityService
    {
        /// <summary>
        /// 어메니티 목록
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Amenity>> GetAmenities();

        /// <summary>
        /// 어메니티 정보
        /// </summary>
        /// <param name="amenityId"></param>
        /// <returns></returns>
        Task<Amenity> GetAmenityById(int amenityId);
    }
}
