using System.Linq.Expressions;

namespace MIB.Core.Domain;

public interface IRepository<TEntity>
    where TEntity : Entity, IAggregateRoot
{
    public IUnitOfWork UnitOfWork { get; }

    public IQueryable<TEntity> DataSet { get; }

    public Task<TEntity?> GetAsync(object id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task MassiveDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}