namespace Microservices.Demo.TestService.Data.Customers;

public class Customer
{
    public long Id { get; set; }

    public long RegionId { get; set; }

    public required string FullName { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
