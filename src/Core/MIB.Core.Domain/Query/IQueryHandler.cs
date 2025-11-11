using MediatR;

namespace MIB.Core.Domain.Query;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
     where TQuery : IQuery<TResult>
{
    //Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}