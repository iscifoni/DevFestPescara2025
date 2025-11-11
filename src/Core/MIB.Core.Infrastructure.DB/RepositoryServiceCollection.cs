using Microsoft.Extensions.DependencyInjection;

using MIB.Core.Domain;
using MIB.Core.Infrastructure.NHibernate;

namespace MIB.Core.Infrastructure.DB
{
    public static class RepositoryServiceCollection
    {
        public static IServiceCollection AddBaseDBInfrastructure(this IServiceCollection services)
        {
            services.AddNHibernateInfrastructureBase();
            return services;
        }

        public static IServiceCollection WithGlobalRepository<T>(this IServiceCollection services)
            where T : Entity, IAggregateRoot
        {
            return services.AddGlobalRepository<T>();
        }

    }
}