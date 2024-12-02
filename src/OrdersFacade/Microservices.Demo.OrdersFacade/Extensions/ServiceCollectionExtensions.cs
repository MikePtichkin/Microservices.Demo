using Microsoft.AspNetCore.Builder;
using Microservices.Demo.OrdersFacade.Infrastructure.Middlewares;

namespace Microservices.Demo.OrdersFacade.Extensions;

public static class ServiceCollectionExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestTimingMiddleware>();
    }
}
