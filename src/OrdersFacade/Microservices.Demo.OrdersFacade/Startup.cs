using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microservices.Demo.OrdersFacade.Application.DI;
using Microservices.Demo.OrdersFacade.Extensions;
using Microservices.Demo.OrdersFacade.Features.Orders.Grpc;
using Microservices.Demo.OrdersFacade.Infra.DI;
using Microservices.Demo.OrdersFacade.Infrastructure.GrpcInterceptors;
using Microservices.Demo.OrdersFacade.Infrastructure.Metrics;
using Serilog;
using System;
using System.Collections.Generic;

namespace Microservices.Demo.OrdersFacade;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

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

        services.AddInfrastructureServices(_configuration);
        services.AddApplicationServices();

        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName: typeof(Startup).Assembly.GetName()?.Name!)
                );

                metrics.AddMeter(GetOrdersMeter.MeterName);

                metrics.AddMeter("Microsoft.AspNetCore.Hosting");
                metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");

                metrics.AddProcessInstrumentation();
                metrics.AddRuntimeInstrumentation();
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();

                metrics.AddPrometheusExporter();
            })
            .WithTracing(trace =>
            {
                var resourceBuilder = ResourceBuilder.CreateDefault()
                    .AddService(serviceName: typeof(Startup).Assembly.GetName()?.Name!)
                    .AddAttributes(new Dictionary<string, object> { { "from", "docker" } });

                trace.SetResourceBuilder(resourceBuilder)
                    .AddNpgsql()
                    .AddAspNetCoreInstrumentation()
                    .SetSampler(new AlwaysOnSampler())
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(o => o.Endpoint = new Uri("http://jaeger:4317"));
            });

        services.AddSingleton<IGetOrdersMeter, GetOrdersMeter>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseSerilogRequestLogging();

        app.UseRequestTiming();

        app.UseRouting();

        app.UseValidationMiddleware();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGrpcService<OrderGrpcService>();
            endpoints.MapGrpcReflectionService();

            endpoints.MapPrometheusScrapingEndpoint();
        });
    }
}
