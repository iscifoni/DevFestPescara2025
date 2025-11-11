using Microsoft.Extensions.DependencyInjection;

using MIB.Core.Domain.Entities;
using MIB.Core.Infrastructure.DB;
using MIB.Core.Infrastructure.Events;
using Asp.Versioning;
using MIB.Core.Infrastructure.NHibernate;

namespace MIB.Microservices.OrderService.Infrastructure
{
    public static class OrderServiceInfrastructureServiceCollection
    {
        public static IServiceCollection AddOrderServiceInfrastructure(this IServiceCollection services)
        {
            services.AddEventInfrastructureServices();
            services.AddNHibernateInfrastructureBase();
            services.WithGlobalRepository<Order>();
            services.WithGlobalRepository<InventoryItem>();
            services.WithGlobalRepository<Product>();

            services
            .AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version")
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;

        }

    }
}
