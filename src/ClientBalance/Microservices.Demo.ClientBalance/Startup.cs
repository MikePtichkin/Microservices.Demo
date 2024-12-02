using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Demo.ClientBalance.Infrastructure.GrpcInterceptors;
using Microservices.Demo.ClientBalance.Infra.DI;
using Microservices.Demo.ClientBalance.Bll.DI;
using Microservices.Demo.ClientBalance.Features.Operations.Grpc;
using Microservices.Demo.ClientBalance.Features.Users.Grpc;

namespace Microservices.Demo.ClientBalance;

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
            endpoints.MapGrpcService<OperationGrpcService>();
            endpoints.MapGrpcService<UserGrpcService>();
            endpoints.MapGrpcReflectionService();
        });
    }
}
