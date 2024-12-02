using Microservices.Demo.TestService.Data.Orders;

namespace Microservices.Demo.TestService.Domain.Actions.MatchOrderError;

public class StoredData
{
    public required IReadOnlyCollection<Order> Orders { get; init; }
}
