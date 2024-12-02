using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Demo.ClientOrders.Infrastructure.GrpcInterceptors;

namespace Microservices.Demo.ClientOrders.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<GrpcExceptionInterceptor>();
            options.Interceptors.Add<GrpcRequestValidationInterceptor>();
        })
        .AddJsonTranscoding();

        services.AddGrpcSwagger();
        services.AddSwaggerGen(config =>
        {
            config.CustomSchemaIds(type => type.FullName);
        });
        services.AddGrpcReflection();

        return services;
    }
}
