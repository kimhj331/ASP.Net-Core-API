using ButlerLee.API.Models.Filters;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace ButlerLee.API.Models
{
    public class KakaoPayErrorResponse : BaseParameters
    {
        [JsonProperty(PropertyName = "code")]
        public string Code  { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
        
        [JsonProperty(PropertyName = "extras")]
        public Extra Extras { get; set; }
    }

    public class Extra
    {
        [JsonProperty(PropertyName = "method_result_code")]
        public string MethodResultCode { get; set; }

        [JsonProperty(PropertyName = "method_result_message")]
        public string MethodResultMessage { get; set; }
    }
}
