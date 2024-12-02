namespace Microservices.Demo.CustomerService.Infrastructure.PostgresRepositories.Contracts;

public class RegionRecord
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;
}