using System.Diagnostics.CodeAnalysis;
using Microservices.Demo.TestService.Domain.Metrics;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.TestService.Domain.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services
            .AddMediatR(config => config.RegisterServicesFromAssemblyContaining<IDomainMarker>())
            .AddSingleton<MismatchMetricManager>()
            .AddSingleton<IMismatchMetricReporter>(provider => provider.GetRequiredService<MismatchMetricManager>())
            .AddSingleton<IMismatchStatisticsReader>(provider => provider.GetRequiredService<MismatchMetricManager>());
}
