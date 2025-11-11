using Microsoft.Extensions.DependencyInjection;

using MIB.Core.Domain.Events;
using MIB.Core.Infrastructure.Events.Dapr.Providers;

namespace MIB.Core.Infrastructure.Events;

public static class DaprIServiceCollectionExtension
{
    public static IServiceCollection AddOutboxEventInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IOutboxEventProvider, OutBoxEventProvider>();
        services.AddDaprClient();

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName?.Contains("Application", StringComparison.CurrentCultureIgnoreCase) ?? false))
        {
            foreach (var t in asm.GetTypes().Where(a => a.GetInterface("IOutboxDomainEventHandler`1") is not null))
                services.AddScoped(t.GetInterface("IOutboxDomainEventHandler`1")!, t);
        }

        return services;
    }
}