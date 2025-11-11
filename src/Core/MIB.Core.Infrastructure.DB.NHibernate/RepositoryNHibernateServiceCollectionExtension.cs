using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MIB.Core.Domain;
using MIB.Core.Infrastructure.NHibernate.Repository;
using MIB.Core.Infrastructure.NHibernate.SessionFactory;

namespace MIB.Core.Infrastructure.NHibernate
{
    public static class RepositoryNHibernateServiceCollectionExtension
    {
        public static IServiceCollection AddNHibernateInfrastructureBase(this IServiceCollection services)
        {
            //services.AddScoped<MIPGTenantConfiguration>(static _ => new MIPGTenantConfiguration()
            //{
            //    AooConnectionString = "Server=10.40.3.39\\SQL22,1533;Database=MIPG2_DB_SCD;User Id=sa;Password=Accenture.1;TrustServerCertificate=True",
            //    GlobalConnectionString = "server=10.40.3.39\\SQL22,1533;Database=MIPG_Global_2;User ID=sa;Password=Accenture.1;TrustServerCertificate=True",
            //    MongoConnectionString = "mongodb://admin:Password1!@localhost:27017/mipgMaster?authSource=admin&replicaSet=rs0",
            //    Aoo = "123"
            //});

            services.AddScoped<GlobalSessionFactoryProvider>();
            services.AddScoped<INHibernateUnitOfWork, NHibernateUnitOfWork>((sp) =>
            {
                var sessionProvider = sp.GetRequiredService<GlobalSessionFactoryProvider>();
                return new NHibernateUnitOfWork(sessionProvider);
            });

            return services;
        }

        
        public static IServiceCollection AddRepository<T>(this IServiceCollection services)
            where T : Entity, IAggregateRoot
        {
            services.AddScoped<IRepository<T>, NHibernateRepository<T>>(sp =>
            {
                var unitofwork = sp.GetRequiredService<INHibernateUnitOfWork>();
                var logger = sp.GetRequiredService<ILogger<NHibernateRepository<T>>>();

                return new NHibernateRepository<T>(unitofwork, logger);
            });
            return services;
        }

        public static IServiceCollection AddGlobalRepository<T>(this IServiceCollection services)
            where T : Entity, IAggregateRoot
        {
            services.AddRepository<T>();
            return services;
        }

    }
}