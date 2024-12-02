using Microservices.Demo.OrdersFacade.Domain.Abstraction;
using System;

namespace Microservices.Demo.OrdersFacade.Domain.Customers;

public sealed class Customer : Entity
{
    public Customer(
        long id,
        long regionId,
        FullName fullName,
        DateTime createdAt)
        : base(id)
    {
        RegionId = regionId;
        FullName = fullName;
        CreatedAt = createdAt;
    }

    public long RegionId { get; init; }
    public FullName FullName { get; init; }
    public DateTime CreatedAt { get; init; }
}
