using MIB.Core.Domain;

using NHibernate;

namespace MIB.Core.Infrastructure.NHibernate
{
    public interface INHibernateUnitOfWork : IUnitOfWork
    {
        public ISession Database { get; init; }
    }
}