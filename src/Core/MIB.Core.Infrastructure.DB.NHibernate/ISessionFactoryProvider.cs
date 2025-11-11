using NHibernate;

namespace MIB.Core.Infrastructure.NHibernate
{
    public interface ISessionFactoryProvider
    {
        public ISession OpenSession();
    }
}