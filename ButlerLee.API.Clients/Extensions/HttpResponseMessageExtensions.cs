using ButlerLee.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ButlerLee.API.Clients
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var content = await response.Content.ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<BaseResponse>(content);

            if (response.Content != null)
                response.Content.Dispose();

            throw new HttpResponseException(response.StatusCode, result.Message);
        }
    }

    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ErrorCode { get; } = "Hostaway";

        public HttpResponseException(HttpStatusCode statusCode, string content) : base(content)
        {
            StatusCode = statusCode;
        }
    }
}
