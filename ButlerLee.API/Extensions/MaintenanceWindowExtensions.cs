using ButlerLee.API.Middleware;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ButlerLee.API.Extensions
{
    public static class MaintenanceWindowExtensions
    {
        public static IServiceCollection AddMaintenance(this IServiceCollection services, MaintenanceWindow window)
        {
            services.AddSingleton(window);
            return services;
        }

        /// <summary>
        /// Service에 미들웨어로 등록
        /// </summary>
        /// <param name="services">모든 서비스목록</param>
        /// <param name="enabler">실행여부</param>
        /// <param name="response"></param>
        /// <param name="contentType"></param>
        /// <param name="retryAfterInSeconds"></param>
        /// <returns></returns>
        public static IServiceCollection AddMaintenance(this IServiceCollection services, Func<bool> enabler, byte[] response, string contentType = "text/html", int retryAfterInSeconds = 3600)
        {

            AddMaintenance(services, new MaintenanceWindow(enabler, response, services)
            {
                ContentType = contentType,
                RetryAfterInSeconds = retryAfterInSeconds
            });

            return services;
        }

        //public static IApplicationBuilder UseMaintenance(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<MaintenanceMiddleware>();
        //}
    }
}
