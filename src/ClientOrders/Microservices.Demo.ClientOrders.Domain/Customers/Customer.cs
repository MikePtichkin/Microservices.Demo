using Microservices.Demo.ClientOrders.Abstraction;
using Microservices.Demo.ClientOrders.Customers;
using Microservices.Demo.ClientOrders.Domain.Common;

namespace Microservices.Demo.ClientOrders.Domain.Customers;

public sealed class Customer : Entity
{
    public Customer(
        long id,
        long regionId,
        FullName fullName,
        TimeStamp createdAt)
        : base(id)
    {
        RegionId = regionId;
        FullName = fullName;
        CreatedAt = createdAt;
    }

    public long RegionId { get; init; }
    public FullName FullName { get; init; }
    public TimeStamp CreatedAt { get; init; }
}
