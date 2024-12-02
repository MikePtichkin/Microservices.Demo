using Microsoft.AspNetCore.Builder;
using Microservices.Demo.ReportService.Infrastructure.Middlewares;

namespace Microservices.Demo.ReportService.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder app) =>
        app.UseMiddleware<RequestTimingMiddleware>();
}
