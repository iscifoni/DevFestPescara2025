using MIB.Core.Domain;
using MIB.Core.Domain.Commands;
using MIB.Core.Domain.Events;
using MIB.Core.Domain.Events.Attributes;
using MIB.Core.Domain.Query;

namespace MIB.Core.Application.Commands;
public class DeleteEntityCommand<TEntity> : ICommand<HandlerResult<TEntity>>
     where TEntity : Entity, IAggregateRoot
{
    public DeleteEntityCommand(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; set; }
}

[NoEventsAutoload]
public class DeleteEntityCommandHandler<TEntity> : ICommandHandler<DeleteEntityCommand<TEntity>, HandlerResult<TEntity>>
where TEntity : Entity, IAggregateRoot
{
    private readonly IDispatcher _dispatcher;
    private readonly IRepository<TEntity> _repository;

    public DeleteEntityCommandHandler(IRepository<TEntity> repository, IDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
    }

    public async Task<HandlerResult<TEntity>> Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.DeleteAsync(request.Entity, cancellationToken);

            var deletedEntity = new EntityDeletedEvent<TEntity>(request.Entity, DateTime.Now);
            await _dispatcher.DispatchAsync(deletedEntity);

            return HandlerResult.Success(request.Entity);
        }
        catch (Exception ex)
        {
            return HandlerResult.Error<TEntity>(ex);
        }
    }
}