using Microservices.Demo.OrdersFacade.Domain.Abstraction;

namespace Microservices.Demo.OrdersFacade.Domain.Regions;

public sealed class Region : Entity
{
    public Region(
        long id,
        RegionName name)
        : base(id)
    {
        Name = name;
    }

    public RegionName Name { get; init; }
}
