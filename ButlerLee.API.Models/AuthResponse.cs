using Newtonsoft.Json;

namespace ButlerLee.API.Models
{
    public class AuthResponse
    {
        [JsonProperty(PropertyName = "Token_Type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "Access_Token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "Expires_In")]
        public int ExpiresIn { get; set; }
    }
}
