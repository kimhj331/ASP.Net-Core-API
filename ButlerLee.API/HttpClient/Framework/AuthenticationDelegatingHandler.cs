//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Polly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Reflection;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ButlerLee.API.HttpClient.Framework
//{
//    public class AuthenticationDelegatingHandler : DelegatingHandler
//    {
//        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            //var token = await GetTokenAsync();
//            //request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.AccessToken);
//            var response = await base.SendAsync(request, cancellationToken);

//            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
//            {
//                //token = await RefreshTokenAsync();
//                //request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.AccessToken);
//                response = await base.SendAsync(request, cancellationToken);
//            }

//            return response;
//        }
//    }
//}


//    //public class AuthenticationDelegatingHandler : DelegatingHandler
//    //{
//    //    private readonly IHttpContextAccessor _httpContextAccessor;
//    //    private readonly ILogger<AuthenticationDelegatingHandler> _logger;

//    //    public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor, ILogger<AuthenticationDelegatingHandler> logger)
//    //    {
//    //        _httpContextAccessor = httpContextAccessor;
//    //        _logger = logger;
//    //    }

//    //    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    //    {
//    //        HttpResponseMessage httpResponseMessage;
//    //        try
//    //        {
//    //            string accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
//    //            if (string.IsNullOrEmpty(accessToken))
//    //            {
//    //                throw new Exception($"Access token is missing for the request {request.RequestUri}");
//    //            }
//    //            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

//    //            //var headers = _httpContextAccessor.HttpContext.Request.Headers;
//    //            //if (headers.ContainsKey("X-Correlation-ID") && !string.IsNullOrEmpty(headers["X-Correlation-ID"]))
//    //            //{
//    //            //    request.Headers.Add("X-Correlation-ID", headers["X-Correlation-ID"].ToString());
//    //            //}

//    //            httpResponseMessage = await base.SendAsync(request, cancellationToken);
//    //            httpResponseMessage.EnsureSuccessStatusCode();
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            _logger.LogError(ex, "Failed to run http query {RequestUri}", request.RequestUri);
//    //            throw;
//    //        }
//    //        return httpResponseMessage;
//    //    }
//    //}
////}
