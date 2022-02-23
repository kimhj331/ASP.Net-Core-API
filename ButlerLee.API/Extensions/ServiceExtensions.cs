using ButlerLee.API.Clients;
using ButlerLee.API.Contracts;
using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Entities;
using ButlerLee.API.LoggerService;
using ButlerLee.API.Repositories;
using ButlerLee.API.Scheduler;
using ButlerLee.API.Schedulers;
using ButlerLee.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ButlerLee.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                //options.AddPolicy("CorsPolicy",
                //    builder => builder.AllowAnyOrigin()
                //    .AllowAnyMethod()
                //    .AllowAnyHeader()
                //    .AllowCredentials());

                options.AddPolicy("CorsPolicy",
                   builder => builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureMySqlContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void ConfigureServiceWrapper(this IServiceCollection services)
        {
            services.AddScoped<IServiceWrapper, ServiceWrapper>();
        }

        public static void ConfigureClientWrapper(this IServiceCollection services)
        {
            services.AddScoped<IClientWrapper, ClientWrapper>();
        }

        public static void ConfigureBackgroundService(this IServiceCollection services)
        {
            services.AddSingleton<IHostedService, CancellingUnpaidReservations>();
            services.AddSingleton<IHostedService, DeleteReadyPayments>();
        }
    }
}
