
namespace MIB.Core.Domain.Events;

public interface IDomainEventHandler<in T>
    where T : IDomainEvent
{
    //Task Handle(T request, CancellationToken cancellationToken);
}