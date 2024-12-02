namespace Microservices.Demo.ViewOrder.Abstraction;

public abstract class Entity
{
    protected Entity(long id)
    {
        Id = id;
    }
    public long Id { get; private set; }
}
