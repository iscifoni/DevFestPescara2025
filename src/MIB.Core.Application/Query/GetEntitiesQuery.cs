using System.Linq.Expressions;

using MIB.Core.Domain;
using MIB.Core.Domain.Entities;
using MIB.Core.Domain.Events.Attributes;
using MIB.Core.Domain.Query;

namespace MIB.Core.Application.Query;

public class GetEntitiesQuery<TEntity> : IQuery<HandlerResult<List<TEntity>>>
     where TEntity : Entity, IAggregateRoot
{
    public Expression<Func<TEntity, bool>>? Filter { get; set; }

    public GetEntitiesQuery()
    {
    }

    public GetEntitiesQuery(Expression<Func<TEntity, bool>>? filter)
    {
        Filter = filter;
    }

    public Expression<Func<TEntity, object>>? OrderBy { get; set; }

    public EnumOrderByType OrderByType { get; set; }

}

public enum EnumOrderByType
{
    Asc,

    Desc
}

[NoEventsAutoload]
public class GetEntitiesQueryHandler<TEntity> : IQueryHandler<GetEntitiesQuery<TEntity>, HandlerResult<List<TEntity>>>
where TEntity : Entity, IAggregateRoot
{
    private readonly IRepository<TEntity> _repository;

    public GetEntitiesQueryHandler(IRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public Task<HandlerResult<List<TEntity>>> Handle(GetEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            var qry = _repository.DataSet;

            if (request.Filter is not null)
                qry = qry.Where(request.Filter);

            if (request.OrderBy is not null)
                qry = request.OrderByType == EnumOrderByType.Asc ? qry.OrderBy(request.OrderBy) : qry.OrderByDescending(request.OrderBy);



            var response = qry.ToList();

            return Task.FromResult(HandlerResult.Success(response));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HandlerResult.Error<List<TEntity>>(ex));
        }
    }
}