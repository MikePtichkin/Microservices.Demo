using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Demo.ClientOrders.Bll.DI;
using Microservices.Demo.ClientOrders.Extensions;
using Microservices.Demo.ClientOrders.Features.Orders.Grpc;
using Microservices.Demo.ClientOrders.Infra.DI;

namespace Microservices.Demo.ClientOrders;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddPresentationServices();
        services.AddInfrastructureServices(_configuration);
        services.AddBllServices();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGrpcService<OrdersGrpcService>();
            endpoints.MapGrpcReflectionService();
        });
    }
}
