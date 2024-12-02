using Microservices.Demo.CustomerService;
using Microservices.Demo.CustomerService.Infrastructure.Migrations.Database;

Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder
        .UseStartup<Startup>())
    .Build()
    .MigrateDatabase()
    .Run();