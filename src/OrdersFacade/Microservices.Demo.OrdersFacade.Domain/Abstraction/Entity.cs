namespace Microservices.Demo.OrdersFacade.Domain.Abstraction;

public abstract class Entity
{
    protected Entity(long id)
    {
        Id = id;
    }
    public long Id { get; init; }
}
