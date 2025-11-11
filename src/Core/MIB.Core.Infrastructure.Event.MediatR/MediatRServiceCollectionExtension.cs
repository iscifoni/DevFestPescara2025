using System.Reflection;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using MIB.Core.Domain.Commands;
using MIB.Core.Domain.Events;
using MIB.Core.Domain.Events.Attributes;
using MIB.Core.Domain.Query;
using MIB.Core.Infrastructure.Events.MediatR.Events.Command;
using MIB.Core.Infrastructure.Events.MediatR.Events.Providers;
using MIB.Core.Infrastructure.Events.MediatR.Events.Query;

namespace MIB.Core.Infrastructure.Events.MediatR;

public static class MediatRServiceCollectionExtension
{
    public static IServiceCollection AddInboxEventInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IInboxEventProvider, InboxEventProvider>();
        services.AddScoped<ICommandProvider, CommandProvider>();
        services.AddScoped<IQueryProviderCA, QueryProvider>();

        services.AddMediatR(config =>
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Assembly? entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
                config.RegisterServicesFromAssemblies(executingAssembly, entryAssembly);
            else
                throw new NullReferenceException("Can't retrieve Entry Assembly");
        });

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.FullName?.StartsWith("System", StringComparison.CurrentCultureIgnoreCase) ?? false))
        {
            var queryhandlerTypes = asm.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Impl = t, Interface = i })
                .Where(x => x.Interface.IsGenericType &&
                    x.Interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

            foreach (var handler in queryhandlerTypes)
            {
                var attr = handler.Impl.GetCustomAttribute<NoEventsAutoloadAttribute>();
                if (attr is null)
                {
                    var requestHandlerType = typeof(IRequestHandler<,>)
                        .MakeGenericType(handler.Interface.GenericTypeArguments);

                    services.AddScoped(requestHandlerType, handler.Impl);
                }
            }

            var commandofthandlerTypes = asm.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Impl = t, Interface = i })
                .Where(x => x.Interface.IsGenericType &&
                    x.Interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

            foreach (var handler in commandofthandlerTypes)
            {
                var attr = handler.Impl.GetCustomAttribute<NoEventsAutoloadAttribute>();
                if (attr is null)
                {
                    var requestHandlerType = typeof(IRequestHandler<,>)
                        .MakeGenericType(handler.Interface.GenericTypeArguments);

                    services.AddScoped(requestHandlerType, handler.Impl);
                }
            }

            var commandhandlerTypes = asm.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Impl = t, Interface = i })
                .Where(x => x.Interface.IsGenericType &&
                    x.Interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

            foreach (var handler in commandhandlerTypes)
            {
                var attr = handler.Impl.GetCustomAttribute<NoEventsAutoloadAttribute>();
                if (attr is null)
                {
                    var requestHandlerType = typeof(IRequestHandler<>)
                        .MakeGenericType(handler.Interface.GenericTypeArguments);

                    services.AddScoped(requestHandlerType, handler.Impl);
                }
            }

            var eventhandlerTypes = asm.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Impl = t, Interface = i })
                .Where(x => x.Interface.IsGenericType &&
                    x.Interface.GetGenericTypeDefinition() == typeof(IInboxDomainEventHandler<>));

            foreach (var handler in eventhandlerTypes)
            {
                var attr = handler.Impl.GetCustomAttribute<NoEventsAutoloadAttribute>();
                if (attr is null)
                {
                    var requestHandlerType = typeof(INotificationHandler<>)
                        .MakeGenericType(handler.Interface.GenericTypeArguments);

                    services.AddScoped(requestHandlerType, handler.Impl);
                }
            }
        }

        return services;
    }
}