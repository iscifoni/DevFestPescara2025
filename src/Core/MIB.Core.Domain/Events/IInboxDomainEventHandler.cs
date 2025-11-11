using MediatR;

namespace MIB.Core.Domain.Events;

public interface IInboxDomainEventHandler<in TRequest> : INotificationHandler<TRequest>, IDomainEventHandler<TRequest>
    where TRequest : IInboxDomainEvent;