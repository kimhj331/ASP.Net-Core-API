using System.Net;
using System.Net.Http;

namespace ButlerLee.API.HttpClient.Framework
{
    public class DefaultHttpClientHandler : HttpClientHandler
    {
        public DefaultHttpClientHandler() =>
            this.AutomaticDecompression = DecompressionMethods.Brotli | DecompressionMethods.Deflate | DecompressionMethods.GZip;
    }
}
