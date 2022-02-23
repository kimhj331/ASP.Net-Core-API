using Microsoft.Extensions.DependencyInjection;
using System;

namespace ButlerLee.API.Middleware
{
    public class MaintenanceWindow
    {
        private Func<bool> enabledFunc;
        private byte[] response;
        private IServiceCollection service;

        public MaintenanceWindow(Func<bool> enabledFunc, byte[] response, IServiceCollection service)
        {
            this.enabledFunc = enabledFunc;
            this.response = response;
            this.service = service;
        }

        public bool Enabled => enabledFunc();

        public IServiceCollection Service => service;
        public byte[] Response => response;

        public int RetryAfterInSeconds { get; set; } = 3600;
        public string ContentType { get; set; } = "text/html";
    }
}
