namespace MIB.Core.Domain.Events;

public interface IOutboxDomainEventHandler<in TRequest> : IDomainEventHandler<TRequest>
    where TRequest : IOutBoxDomainEvent
{
    
    public Task Handle(TRequest notification, CancellationToken cancellationToken);
}