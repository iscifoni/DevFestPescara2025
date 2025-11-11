using MIB.Core.Domain;
using MIB.Core.Domain.Commands;
using MIB.Core.Domain.Events.Attributes;
using MIB.Core.Domain.Query;

public class MassiveDeleteEntityCommand<TEntity> : ICommand<HandlerResult<IEnumerable<TEntity>>>
     where TEntity : Entity, IAggregateRoot
{
    public MassiveDeleteEntityCommand(IEnumerable<TEntity> entities)
    {
        Entities = entities;
    }

    public IEnumerable<TEntity> Entities { get; set; }
}

[NoEventsAutoload]
public class MassiveDeleteEntityCommandHandler<TEntity> : ICommandHandler<MassiveDeleteEntityCommand<TEntity>, HandlerResult<IEnumerable<TEntity>>>
    where TEntity : Entity, IAggregateRoot
{
    private readonly IRepository<TEntity> _repository;

    public MassiveDeleteEntityCommandHandler(IRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<HandlerResult<IEnumerable<TEntity>>> Handle(MassiveDeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.MassiveDeleteAsync(request.Entities, cancellationToken);

            return HandlerResult.Success(request.Entities);
        }
        catch (Exception ex)
        {
            return HandlerResult.Error<IEnumerable<TEntity>>(ex);
        }
    }
}