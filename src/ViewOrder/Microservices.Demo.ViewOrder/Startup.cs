using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Demo.ViewOrder.Bll.DI;
using Microservices.Demo.ViewOrder.Infra.DI;

namespace Microservices.Demo.ViewOrder;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructureServices(_configuration);
        services.AddBllServices();
    }

    public void Configure(IApplicationBuilder app)
    { }
}
