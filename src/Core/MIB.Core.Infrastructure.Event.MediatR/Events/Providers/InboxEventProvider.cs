using MediatR;

using MIB.Core.Domain.Events;

namespace MIB.Core.Infrastructure.Events.MediatR.Events.Providers
{
    internal class InboxEventProvider(IMediator mediator) : IInboxEventProvider
    {
        public async Task SendAsync<T>(T message, CancellationToken cancellationToken = default)
             where T : IDomainEvent
        {
            await mediator.Publish(message, cancellationToken);
        }
    }
}