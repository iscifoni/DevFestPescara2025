using MIB.Core.Domain;
using MIB.Core.Domain.Events.Attributes;
using MIB.Core.Domain.Query;

namespace MIB.Core.Application.Query;

public class GetEntitiesByIdQuery<TEntity>(object id) : IQuery<HandlerResult<TEntity>>
    where TEntity : Entity, IAggregateRoot
{
    public object Id { get; set; } = id;
}

[NoEventsAutoload]
public class GetEntitiesByIdQueryHandler<TEntity>(IRepository<TEntity> repository) : IQueryHandler<GetEntitiesByIdQuery<TEntity>, HandlerResult<TEntity>>
    where TEntity : Entity, IAggregateRoot
{
    private readonly IRepository<TEntity> _repository = repository;

    public async Task<HandlerResult<TEntity>> Handle(GetEntitiesByIdQuery<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _repository.GetAsync(request.Id, cancellationToken);
            if (response is null)
                return HandlerResult.NotFound<TEntity>();

            return HandlerResult.Success<TEntity>(response);
        }
        catch (Exception ex)
        {
            return HandlerResult.Error<TEntity>(ex);
        }
    }
}