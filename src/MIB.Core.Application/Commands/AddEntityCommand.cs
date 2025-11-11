using MIB.Core.Domain;
using MIB.Core.Domain.Commands;
using MIB.Core.Domain.Events;
using MIB.Core.Domain.Events.Attributes;
using MIB.Core.Domain.Query;

namespace MIB.Core.Application.Commands;
public class AddEntityCommand<TEntity> : ICommand<HandlerResult<TEntity>>
    where TEntity : Entity, IAggregateRoot
{
    public AddEntityCommand(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; set; }
}

[NoEventsAutoload]
public class AddEntityCommandHandler<TEntity> : ICommandHandler<AddEntityCommand<TEntity>, HandlerResult<TEntity>>
where TEntity : Entity, IAggregateRoot
{
    private readonly IRepository<TEntity> _repository;
    private readonly IDispatcher _dispatcher;

    public AddEntityCommandHandler(IRepository<TEntity> repository, IDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
    }

    public async Task<HandlerResult<TEntity>> Handle(AddEntityCommand<TEntity> command, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.AddAsync(command.Entity, cancellationToken);

            var localEntity = await _repository.GetAsync(command.Entity.Id);

            var createdEntity = new EntityCreatedEvent<TEntity>(localEntity!, DateTime.Now);
            await _dispatcher.DispatchAsync(createdEntity);

            return HandlerResult.Success(localEntity!);
        }
        catch (Exception ex)
        {
            return HandlerResult.Error<TEntity>(ex);
        }
    }
}