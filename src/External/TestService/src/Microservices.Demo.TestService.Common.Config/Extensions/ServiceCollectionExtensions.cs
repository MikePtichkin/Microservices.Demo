using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.TestService.Common.Config;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonConfiguration(this IServiceCollection services)
    {
        return services
            .AddSingleton<IEnvironmentVariableProvider, EnvironmentVariableProvider>();
    }
}
