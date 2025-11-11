
using Microsoft.Extensions.DependencyInjection;
using MIB.Core.Application.Commands;
using MIB.Core.Application.Query;
using MIB.Core.Domain;
using MIB.Core.Domain.Query;

using MediatR;


namespace MIB.Core.Application
{
    public static class CoreApplicationServiceCollectionExtensions
    {


        public static IServiceCollection AddDefaultCrud<TEntity>(this IServiceCollection services) where TEntity : Entity, IAggregateRoot
        {
            services
                .AddGetEntitiesQuery<TEntity>()
                .AddGetEntityByIdQuery<TEntity>()
                .AddAddEntityCommand<TEntity>()
                .AddUpdateEntityCommand<TEntity>()
                .AddDeleteEntityCommand<TEntity>()
                .AddMassiveDeleteEntityCommand<TEntity>();

            return services;
        }

        private static IServiceCollection AddGetEntitiesQuery<TEntity>(this IServiceCollection services) where TEntity : Entity, IAggregateRoot
        {
            services.AddScoped(typeof(IRequestHandler<GetEntitiesQuery<TEntity>, HandlerResult<List<TEntity>>>), typeof(GetEntitiesQueryHandler<TEntity>));
            return services;
        }

        private static IServiceCollection AddAddEntityCommand<TEntity>(this IServiceCollection services) where TEntity : Entity, IAggregateRoot
        {
            services.AddScoped(typeof(IRequestHandler<AddEntityCommand<TEntity>, HandlerResult<TEntity>>), typeof(AddEntityCommandHandler<TEntity>));

            return services;
        }

        private static IServiceCollection AddGetEntityByIdQuery<TEntity>(this IServiceCollection services) where TEntity : Entity, IAggregateRoot
        {
            services.AddScoped(typeof(IRequestHandler<GetEntitiesByIdQuery<TEntity>, HandlerResult<TEntity>>), typeof(GetEntitiesByIdQueryHandler<TEntity>));
            return services;
        }

        private static IServiceCollection AddUpdateEntityCommand<TEntity>(this IServiceCollection services) where TEntity : Entity, IAggregateRoot

        {
            services.AddScoped(typeof(IRequestHandler<UpdateEntityCommand<TEntity>, HandlerResult<TEntity>>), typeof(UpdateEntityCommandHandler<TEntity>));
            return services;
        }

        private static IServiceCollection AddDeleteEntityCommand<TEntity>(this IServiceCollection services) where TEntity : Entity, IAggregateRoot
        {
            services.AddScoped(typeof(IRequestHandler<DeleteEntityCommand<TEntity>, HandlerResult<TEntity>>), typeof(DeleteEntityCommandHandler<TEntity>));
            return services;
        }

        private static IServiceCollection AddMassiveDeleteEntityCommand<TEntity>(this IServiceCollection services) where TEntity : Entity, IAggregateRoot
        {
            services.AddScoped(typeof(IRequestHandler<MassiveDeleteEntityCommand<TEntity>, HandlerResult<IEnumerable<TEntity>>>), typeof(MassiveDeleteEntityCommandHandler<TEntity>));
            return services;
        }
    }
}