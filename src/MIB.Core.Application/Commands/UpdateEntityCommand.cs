using MIB.Core.Domain;
using MIB.Core.Domain.Commands;
using MIB.Core.Domain.Events;
using MIB.Core.Domain.Events.Attributes;
using MIB.Core.Domain.Query;

namespace MIB.Core.Application.Commands;

public class UpdateEntityCommand<TEntity> : ICommand<HandlerResult<TEntity>>
    where TEntity : Entity, IAggregateRoot
{
    public UpdateEntityCommand(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; set; }
}

[NoEventsAutoload]
public class UpdateEntityCommandHandler<TEntity> : ICommandHandler<UpdateEntityCommand<TEntity>, HandlerResult<TEntity>>
where TEntity : Entity, IAggregateRoot
{
    private readonly IRepository<TEntity> _repository;
    private readonly IDispatcher _dispatcher;

    public UpdateEntityCommandHandler(IRepository<TEntity> repository, IDispatcher dispatcher)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<HandlerResult<TEntity>> Handle(UpdateEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.UpdateAsync(request.Entity, cancellationToken);

            var updatedEntity = new EntityUpdatedEvent<TEntity>(request.Entity, DateTime.Now);
            await _dispatcher.DispatchAsync(updatedEntity);

            return HandlerResult.Success(request.Entity);
        }
        catch (Exception ex)
        {
            return HandlerResult.Error<TEntity>(ex);
        }
    }
}