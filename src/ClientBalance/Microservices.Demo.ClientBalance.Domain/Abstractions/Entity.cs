using System;

namespace Microservices.Demo.ClientBalance.Domain.Abstractions;

public abstract class Entity<T> where T : struct
{
    protected Entity(T id)
    {
        Id = id;
    }
    public T Id { get; init; }
}
