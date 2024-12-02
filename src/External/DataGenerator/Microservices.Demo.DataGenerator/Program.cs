using Microservices.Demo.DataGenerator.Bll.DI;
using Microservices.Demo.DataGenerator.Infra.DI;

var builder = WebApplication.CreateBuilder(args);


var config = builder.Configuration
    .AddEnvironmentVariables("DEMO_")
    .Build();

builder.Services
    .AddBll()
    .AddInfra(config)
    .AddHealthChecks();

var app = builder.Build();

app.UseRouting();
app.MapHealthChecks("/health");

app.Run();
