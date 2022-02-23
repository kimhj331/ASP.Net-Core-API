using ButlerLee.API.Contracts;
using ButlerLee.API.Models;
using IdentityModel.AspNetCore.AccessTokenManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ButlerLee.API.HttpClient.Framework
{
    /// <summary>
    /// 호출하는 응용프로그램의 이름과 버전을 서버에 전달하기 위해
    /// User-Agent Http Header에 기록
    /// </summary>
    public class UserAgentDelegatingHandler : DelegatingHandler
    {
      
        public UserAgentDelegatingHandler()//AccessTokenManagementDefaults.DefaultTokenClientName)
            : this(Assembly.GetEntryAssembly())
        {        }

        public UserAgentDelegatingHandler(Assembly assembly)
            : this(GetProduct(assembly), GetVersion(assembly))
        {
        }

        public UserAgentDelegatingHandler(string applicationName, string applicationVersion)
        {
            if (applicationName == null)
            {
                throw new ArgumentNullException(nameof(applicationName));
            }

            if (applicationVersion == null)
            {
                throw new ArgumentNullException(nameof(applicationVersion));
            }

            this.UserAgentValues = new List<ProductInfoHeaderValue>()
            {
                new ProductInfoHeaderValue(applicationName.Replace(' ', '-'), applicationVersion),
                new ProductInfoHeaderValue($"({Environment.OSVersion})"),
            };
        }

        public UserAgentDelegatingHandler(List<ProductInfoHeaderValue> userAgentValues) =>
            this.UserAgentValues = userAgentValues ?? throw new ArgumentNullException(nameof(userAgentValues));

        public List<ProductInfoHeaderValue> UserAgentValues { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.UserAgent.Any())
            {
                foreach (var userAgentValue in this.UserAgentValues)
                {
                    request.Headers.UserAgent.Add(userAgentValue);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
        private static string GetProduct(Assembly assembly) =>
          assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        private static string GetVersion(Assembly assembly) =>
            assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
    }

}
