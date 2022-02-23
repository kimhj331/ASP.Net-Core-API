using System;

namespace ButlerLee.API.HttpClient.Options
{
    public class HttpClientOptions
    {
        public Uri BaseAddress { get; set; }
        public TimeSpan Timeout { get; set; }
        public string Schema { get; set; } 
        public string Key { get; set; }
        public string Cid { get; set; }
    }
}
