using MIB.Core.Domain.Commands;
using MIB.Core.Domain.Events;
using MIB.Core.Domain.Query;

namespace MIB.Core.Domain
{
    public interface IDispatcher
    {
        public Task DispatchAsync(ICommand command, CancellationToken cancellationToken = default);

        public Task<T> DispatchAsync<T>(ICommand<T> command, CancellationToken cancellationToken = default);

        public Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken = default);

        public Task DispatchAsync(IInboxDomainEvent domainEvent, CancellationToken cancellationToken = default);

        public Task DispatchAsync(IOutBoxDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}