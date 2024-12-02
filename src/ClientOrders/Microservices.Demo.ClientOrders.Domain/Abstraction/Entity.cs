namespace Microservices.Demo.ClientOrders.Abstraction;

public abstract class Entity
{
    protected Entity()
    { }
    protected Entity(long id)
    {
        Id = id;
    }
    public long Id { get; private set; }
}
