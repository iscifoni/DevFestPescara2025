namespace MIB.Core.Domain.Events;

public class EntityUpdatedEvent<T>(T entity, DateTime eventDateTime) : IInboxDomainEvent
    where T : Entity
{
    public T Entity { get; } = entity;

    public DateTime EventDateTime { get; } = eventDateTime;
}