using System.Linq.Expressions;

using Microsoft.Extensions.Logging;

using MIB.Core.Domain;

using NHibernate;
using NHibernate.Linq;

namespace MIB.Core.Infrastructure.NHibernate.Repository
{
    public class NHibernateRepository<TEntity>(INHibernateUnitOfWork nHibernateUnitOfWork, ILogger<NHibernateRepository<TEntity>> logger) : IRepository<TEntity>
        where TEntity : Entity, IAggregateRoot
    {
        public IUnitOfWork UnitOfWork => nHibernateUnitOfWork;

        public ISession Database => nHibernateUnitOfWork.Database;

        public IQueryable<TEntity> DataSet => Database.Query<TEntity>();

        public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            logger.LogDebug("Add entity on {modelname}", nameof(entity));
            Database.SaveOrUpdateAsync(entity, cancellationToken).Wait(cancellationToken);
            return Database.FlushAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
                await AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Database.DeleteAsync(entity, cancellationToken).Wait(cancellationToken);
            return Database.FlushAsync(cancellationToken);
        }

        public async Task MassiveDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var entity in entities)
                    await Database.DeleteAsync(entity, cancellationToken);

                await Database.FlushAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Database.Query<TEntity>().ToListAsync(cancellationToken: cancellationToken);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1079:Throwing of new NotImplementedException", Justification = "<Pending>")]
        public async Task<TEntity?> GetAsync(object id, CancellationToken cancellationToken = default)
        {
            return (await GetAsync(a => a.Id.Equals(id), cancellationToken)).FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            logger.LogDebug("Retrieving entity with filter ");
            //logger.LogDebug("Retrieved entity: {Payload}", SerializeToJson(entity));
            return await Database.Query<TEntity>().Where(filter).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Database.MergeAsync(entity, cancellationToken);
            await Database.FlushAsync(cancellationToken);

            return entity;
        }
    }
}