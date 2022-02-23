using ButlerLee.API.Clients;
using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.HttpClient.Options;
using ButlerLee.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http.Headers;
using System.Text;
using TossPayClient = ButlerLee.API.Models.TossPayClient;

namespace ButlerLee.API.HttpClient.Framework
{
    public static class ServiceCollectionExtensions
    {
        private const string PoliciesConfigurationSectionName = "Policies";

        /// <summary>
        /// 정책 설정
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configurationSectionName"></param>
        /// <returns></returns>
        public static IServiceCollection AddPolicies(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName = PoliciesConfigurationSectionName)
        {
            services.Configure<PolicyOptions>(configuration);
            var policyOptions = configuration.GetSection($"{configurationSectionName}").Get<PolicyOptions>();

            var policyRegistry = services.AddPolicyRegistry();
            policyRegistry.Add(
                PolicyName.HttpRetry,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        policyOptions.HttpRetry.Count,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(policyOptions.HttpRetry.BackoffPower, retryAttempt))));
            policyRegistry.Add(
                PolicyName.HttpCircuitBreaker,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: policyOptions.HttpCircuitBreaker.ExceptionsAllowedBeforeBreaking,
                        durationOfBreak: policyOptions.HttpCircuitBreaker.DurationOfBreak));

            return services;
        }

        #region originalHttpClient 설정
        /*
       /// <summary>
       /// HttpClient 설정
       /// </summary>
       /// <typeparam name="TClient"></typeparam>
       /// <typeparam name="TImplementation"></typeparam>
       /// <typeparam name="TClientOptions"></typeparam>
       /// <param name="services"></param>
       /// <param name="configuration"></param>
       /// <param name="configurationSectionName"></param>
       /// <returns></returns>
       public static IServiceCollection AddHttpClient<TClient, TImplementation, TClientOptions>(
           this IServiceCollection services,
           IConfiguration configuration,
           string configurationSectionName)
           where TClient : class
           where TImplementation : class, TClient
           where TClientOptions : HttpClientOptions, new() =>
           services
               .Configure<TClientOptions>(configuration.GetSection($"{configurationSectionName}"))
               .AddTransient<UserAgentDelegatingHandler>()
               .AddHttpClient<TClient, TImplementation>()
               .AddClientAccessTokenHandler()
               .ConfigureHttpClient(
                   (serviceProvider, httpClient) =>
                   {
                       var httpClientOptions = serviceProvider
                           .GetRequiredService<IOptions<TClientOptions>>()
                           .Value;

                       httpClient.BaseAddress = httpClientOptions.BaseAddress;
                       httpClient.Timeout = httpClientOptions.Timeout;
                       httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                       httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                       configuration.GetSection(configurationSectionName);
                   })
               .ConfigurePrimaryHttpMessageHandler(x => new DefaultHttpClientHandler())
               .AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
               .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
               .AddHttpMessageHandler<UserAgentDelegatingHandler>()
               .Services;

       /// <summary>
       /// PG 클라이언트 설정
       /// </summary>
       /// <typeparam name="TClient"></typeparam>
       /// <typeparam name="TImplementation"></typeparam>
       /// <typeparam name="TClientOptions"></typeparam>
       /// <param name="services"></param>
       /// <param name="configuration"></param>
       /// <param name="configurationSectionName"></param>
       /// <returns></returns>
       public static IServiceCollection AddHttpPayClient<TClient, TImplementation, TClientOptions>(
          this IServiceCollection services,
          IConfiguration configuration,
          string configurationSectionName)
          where TClient : class
          where TImplementation : class, TClient
          where TClientOptions : HttpClientOptions, new() =>
          services
              .Configure<TClientOptions>(configuration.GetSection($"{configurationSectionName}"))
              .AddTransient<UserAgentDelegatingHandler>()
              .AddHttpClient<TClient, TImplementation>()
              //.AddClientAccessTokenHandler()
              .ConfigureHttpClient(
                  (serviceProvider, httpClient) =>
                  {
                      var httpClientOptions = serviceProvider
                          .GetRequiredService<IOptions<TClientOptions>>()
                          .Value;

                      httpClient.BaseAddress = httpClientOptions.BaseAddress;
                      httpClient.Timeout = httpClientOptions.Timeout;
                      httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                      httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                      if (configurationSectionName == nameof(TossPayClient))
                      {
                          string key = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(httpClientOptions.Key + ":"));
                          httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(httpClientOptions.Schema, key);
                      }
                      else
                      {
                          httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(httpClientOptions.Schema, httpClientOptions.Key);
                          httpClient.DefaultRequestHeaders.Add(nameof(httpClientOptions.Cid), httpClientOptions.Cid);
                      }

                      configuration.GetSection(configurationSectionName);
                  })
              .ConfigurePrimaryHttpMessageHandler(x => new DefaultHttpClientHandler())
              .AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
              .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
              .AddHttpMessageHandler<UserAgentDelegatingHandler>()
              .Services;
       */
        #endregion


        #region ClientTest
        /// <summary>
        /// HttpClient 설정
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TClientOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configurationSectionName"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClient<TClientOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName)
            where TClientOptions : HttpClientOptions, new() =>
            services
                .Configure<TClientOptions>(configuration.GetSection($"{configurationSectionName}"))
                .AddTransient<UserAgentDelegatingHandler>()
                .AddTransient<ExpiredTokenCheckDelegationHandler>()
                .AddHttpClient(nameof(HostAwayClient))
                .AddClientAccessTokenHandler()
                .ConfigureHttpClient(
                    (serviceProvider, httpClient) =>
                    {
                        var httpClientOptions = serviceProvider
                            .GetRequiredService<IOptions<TClientOptions>>()
                            .Value;

                        httpClient.BaseAddress = httpClientOptions.BaseAddress;
                        httpClient.Timeout = httpClientOptions.Timeout;
                        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                        configuration.GetSection(configurationSectionName);
                    })
                .ConfigurePrimaryHttpMessageHandler(x => new DefaultHttpClientHandler())
                .AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
                .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
                .AddHttpMessageHandler<UserAgentDelegatingHandler>()
                .AddHttpMessageHandler<ExpiredTokenCheckDelegationHandler>()
                .Services;
        #endregion

        public static IServiceCollection AddKakaoHttpClient<TClientOptions>(
          this IServiceCollection services,
          IConfiguration configuration,
          string configurationSectionName)
          where TClientOptions : HttpClientOptions, new() =>
          services
              .Configure<TClientOptions>(configuration.GetSection($"{configurationSectionName}"))
              .AddTransient<UserAgentDelegatingHandler>()
              .AddHttpClient(nameof(Models.KakaoPayClient))
              //.AddClientAccessTokenHandler()
              .ConfigureHttpClient(
                  (serviceProvider, httpClient) =>
                  {
                      var httpClientOptions = serviceProvider
                          .GetRequiredService<IOptions<TClientOptions>>()
                          .Value;

                      httpClient.BaseAddress = httpClientOptions.BaseAddress;
                      httpClient.Timeout = httpClientOptions.Timeout;
                      httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                      httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                     
                      httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(httpClientOptions.Schema, httpClientOptions.Key);
                      httpClient.DefaultRequestHeaders.Add(nameof(httpClientOptions.Cid), httpClientOptions.Cid);

                      configuration.GetSection(configurationSectionName);
                  })
              .ConfigurePrimaryHttpMessageHandler(x => new DefaultHttpClientHandler())
              .AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
              .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
              .AddHttpMessageHandler<UserAgentDelegatingHandler>()
              .Services;

        public static IServiceCollection AddTossHttpClient<TClientOptions>(
         this IServiceCollection services,
         IConfiguration configuration,
         string configurationSectionName)
         where TClientOptions : HttpClientOptions, new() =>
         services
             .Configure<TClientOptions>(configuration.GetSection($"{configurationSectionName}"))
             .AddTransient<UserAgentDelegatingHandler>()
             .AddHttpClient(nameof(Models.TossPayClient))
             //.AddClientAccessTokenHandler()
             .ConfigureHttpClient(
                 (serviceProvider, httpClient) =>
                 {
                     var httpClientOptions = serviceProvider
                         .GetRequiredService<IOptions<TClientOptions>>()
                         .Value;

                     httpClient.BaseAddress = httpClientOptions.BaseAddress;
                     httpClient.Timeout = httpClientOptions.Timeout;
                     httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                     httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                     string key = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(httpClientOptions.Key + ":"));
                     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(httpClientOptions.Schema, key);
                     
                     configuration.GetSection(configurationSectionName);
                 })
             .ConfigurePrimaryHttpMessageHandler(x => new DefaultHttpClientHandler())
             .AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
             .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
             .AddHttpMessageHandler<UserAgentDelegatingHandler>()
             .Services;


        //private static string RetrieveToken(HostAwayConfiguration hostAwayConfig)
        //{
        //    using (WebClient client = new WebClient())
        //    {
        //        System.Collections.Specialized.NameValueCollection parameters =
        //            new System.Collections.Specialized.NameValueCollection();
        //        parameters.Add("grant_type", "client_credentials");
        //        parameters.Add("client_id", hostAwayConfig.ClientId);
        //        parameters.Add("client_secret", hostAwayConfig.ClientSecret);

        //        byte[] response = client.UploadValues(hostAwayConfig.Endpoint, "POST", parameters);
        //        string body = Encoding.UTF8.GetString(response);

        //        AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(body);
        //        return authResponse.AccessToken;
        //    }
        //}
    }


}
