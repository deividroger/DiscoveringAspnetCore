using Microsoft.AspNetCore.Builder;

namespace DiscoveringAsp.netCore.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
