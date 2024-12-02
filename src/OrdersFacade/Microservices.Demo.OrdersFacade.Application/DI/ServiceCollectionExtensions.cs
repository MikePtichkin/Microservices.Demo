﻿using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.OrdersFacade.Application.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            });

        return services;
    }
}
