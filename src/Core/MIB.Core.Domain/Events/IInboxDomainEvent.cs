using MediatR;

namespace MIB.Core.Domain.Events;

public interface IInboxDomainEvent : INotification, IDomainEvent
{
}