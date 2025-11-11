using Microsoft.Extensions.DependencyInjection;

using MIB.Core.Domain;
using MIB.Core.Infrastructure.Events.MediatR;

namespace MIB.Core.Infrastructure.Events
{
    public static class EventsServiceCollectionExtensions
    {
        public static IServiceCollection AddEventInfrastructureServices(this IServiceCollection services)
        {
            services.AddInboxEventInfrastructureServices();
            services.AddOutboxEventInfrastructureServices();
            services.AddScoped<IDispatcher, Dispatcher>();

            return services;
        }

        public static IServiceCollection AddCQRSHandlerServices(this IServiceCollection services)
        {
            return services;
        }
    }
}