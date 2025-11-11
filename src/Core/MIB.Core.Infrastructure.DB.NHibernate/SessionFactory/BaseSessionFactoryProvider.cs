using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using Microsoft.Extensions.Configuration;

using NHibernate;
using NHibernate.Caches.StackExchangeRedis;
using NHibernate.Driver;

namespace MIB.Core.Infrastructure.NHibernate.SessionFactory
{
    public abstract class BaseSessionFactoryProvider(IConfiguration configuration)
    {
        public ISessionFactory GetSession(string ConnectionString)
        {
            var redisConnectionString = configuration.GetConnectionString("redis");
            var session = Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2012.Driver<MicrosoftDataSqlClientDriver>().ConnectionString(ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BaseSessionFactoryProvider>());

            if (redisConnectionString is not null)
            {
                session = session
                    .Cache(c => c.ProviderClass(typeof(RedisCacheProvider).AssemblyQualifiedName)
                     //.UseQueryCache()
                     //.UseSecondLevelCache()
                     )
                    .ExposeConfiguration(cfg =>
                    {
                        cfg.Properties.Add("cache.default_expiration", "900");
                        cfg.Properties.Add("cache.use_sliding_expiration", "true");
                        cfg.Properties.Add("cache.configuration", redisConnectionString);
                    });
            }

            var localSessionFactory = session.BuildSessionFactory();
            localSessionFactory.Statistics.IsStatisticsEnabled = true;
            return localSessionFactory;
        }
    }
}