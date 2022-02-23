using ButlerLee.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ButlerLee.API.Extensions
{
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
