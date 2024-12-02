namespace Microservices.Demo.CustomerService.Extensions;

public static class ConnectionStringProvider
{
    public static string? GetConnectionString()
    {
        return Environment.GetEnvironmentVariable("DEMO_CUSTOMER_SERVICE_DB_CONNECTION_STRING");
    }
}