using MediatR;

using MIB.Core.Domain.Query;

namespace MIB.Core.Infrastructure.Events.MediatR.Events.Query
{
    internal class QueryProvider(IMediator mediator) : IQueryProviderCA
    {
        public async Task<M> SendAsync<T, M>(T message, CancellationToken cancellationToken = default) where T : IQuery<M>
        {
            return await mediator.Send(message, cancellationToken);
        }
    }
}