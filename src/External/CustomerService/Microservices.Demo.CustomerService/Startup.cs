using FluentValidation;

using Microservices.Demo.CustomerService.Extensions;
using Microservices.Demo.CustomerService.Presentation.Controllers.Grpc;
using Microservices.Demo.CustomerService.Presentation.Interceptors;

namespace Microservices.Demo.CustomerService;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var currentAssembly = typeof(Startup).Assembly;
        services
            .AddMediatR(c => c.RegisterServicesFromAssembly(currentAssembly));

        services
            .AddDatabase()
            .AddRepositories();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddControllers();
        services.AddGrpc(
                options =>
                {
                    options.EnableDetailedErrors = true;
                    options.Interceptors.Add<GrpcExceptionInterceptor>();
                    options.Interceptors.Add<GrpcRequestValidationInterceptor>();
                })
            .AddJsonTranscoding();
        services.AddValidatorsFromAssembly(currentAssembly);
        services.AddGrpcSwagger();
        services.AddSwaggerGen();
        services.AddGrpcReflection();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<CustomerController>();
                endpoints.MapGrpcReflectionService();
            });
    }
}