using Microsoft.Extensions.Configuration;

using NHibernate;

namespace MIB.Core.Infrastructure.NHibernate.SessionFactory
{
    public class GlobalSessionFactoryProvider : BaseSessionFactoryProvider, ISessionFactoryProvider
    {
        public GlobalSessionFactoryProvider(IConfiguration diConfiguration)
            : base(diConfiguration)
        {
            GlobalSession = GetSession(diConfiguration.GetConnectionString("MIBDb") ?? throw new NullReferenceException("ConnectionString::MIBDb"));
        }

        public ISessionFactory GlobalSession { get; }

        public ISession OpenSession()
        {
            return GlobalSession.OpenSession();
        }
    }
}