using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

namespace ButlerLee.API.Models.Filters
{
    //[DataContract]
    public abstract class PagenationParameters : BaseParameters
    {
        const int maxPageSize = 1000;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        [JsonProperty(PropertyName = "limit")]
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        [JsonProperty(PropertyName = "offset")]
        internal int Offset
        {
            get
            {
                if (PageNumber > 0)
                    return (PageNumber - 1) * PageSize;
                else
                    return 0;
            }
        }
    }
}
