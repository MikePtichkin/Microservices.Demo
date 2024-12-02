using System.Diagnostics.CodeAnalysis;
using Microservices.Demo.TestService.AppStart;
using Microservices.Demo.TestService.Domain.Extensions;

namespace Microservices.Demo.TestService;

[ExcludeFromCodeCoverage]
public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSwaggerGen()
            .AddMvcCore()
            .AddApiExplorer();

        services
            .AddIntegrationServices(_configuration.GetSection("Integrations"))
            .AddDataServices(_configuration.GetSection("Data"))
            .AddDomainServices();
    }

    public void Configure(IApplicationBuilder app)
    {
        app
            .UseSwagger()
            .UseSwaggerUI();

        app
            .UseRouting()
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
