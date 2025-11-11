using MIB.Core.Application;
using MIB.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;


namespace MIB.Microservices.OrderService.Application
{
    public static class OrderServiceInfrastructureServiceCollection
    {
        public static IServiceCollection AddOrderServiceApplication(this IServiceCollection services)
        {
            services.AddDefaultCrud<Order>();
            services.AddDefaultCrud<InventoryItem>();
            services.AddDefaultCrud<Product>();

            return services;

        }

    }
}
