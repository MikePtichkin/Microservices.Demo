using FluentValidation.AspNetCore;
using Microservices.Demo.ReportService.Bll.DI;
using Microservices.Demo.ReportService.Extensions;
using Microservices.Demo.ReportService.Infra.DI;
using Microservices.Demo.ReportService.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.ReportService;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.Filters.Add<HttpValidationFilter>();
            })
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

        services.AddSwaggerGen(config =>
        {
            config.CustomSchemaIds(type => type.FullName);
        });

        services.AddInfrastructureServices(_configuration);
        services.AddBllServices();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRequestTiming();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
