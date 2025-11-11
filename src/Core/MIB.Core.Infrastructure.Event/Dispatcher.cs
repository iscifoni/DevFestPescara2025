using MIB.Core.Domain;
using MIB.Core.Domain.Commands;
using MIB.Core.Domain.Events;
using MIB.Core.Domain.Query;

namespace MIB.Core.Infrastructure.Events
{
    public class Dispatcher(
        IInboxEventProvider inboxEventProvider,
        IOutboxEventProvider outboxEventProvider,
        ICommandProvider commandProvider,
        IQueryProviderCA queryProvider)
        : IDispatcher
    {
        public async Task DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            await commandProvider.SendAsync(command, cancellationToken);
        }

        public async Task<T> DispatchAsync<T>(ICommand<T> command, CancellationToken cancellationToken = default)
        {
            return await commandProvider.SendAsync<ICommand<T>, T>(command, cancellationToken);
        }

        public async Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken = default)
        {
            return await queryProvider.SendAsync<IQuery<T>, T>(query, cancellationToken);
        }

        public async Task DispatchAsync(IOutBoxDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await outboxEventProvider.SendAsync(domainEvent, cancellationToken);
        }

        public async Task DispatchAsync(IInboxDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await inboxEventProvider.SendAsync(domainEvent, cancellationToken);
        }
    }
}