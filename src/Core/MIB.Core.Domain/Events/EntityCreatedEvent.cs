namespace MIB.Core.Domain.Events;

public class EntityCreatedEvent<T>(T entity, DateTime eventDateTime) : IOutBoxDomainEvent
    where T : Entity
{
    public T Entity { get; } = entity;

    public DateTime EventDateTime { get; } = eventDateTime;
}