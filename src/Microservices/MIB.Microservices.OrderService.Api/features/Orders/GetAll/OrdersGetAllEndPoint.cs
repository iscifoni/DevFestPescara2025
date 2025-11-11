using Asp.Versioning.Conventions;
using MIB.Core.Application.Query;
using MIB.Core.Domain;
using MIB.Core.Domain.Entities;
using MIB.Core.Infrastructure.EndPoint.MinimalApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace MIB.Microservices.OrderService.Api.features.Orders.GetAll
{
    public class OrdersGetAllEndPoint : IEndpointHandler
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            var versionSet = builder.NewApiVersionSet("OrdersGetAll")
               .HasApiVersion(1)
               .ReportApiVersions()
               .Build();

            builder.MapGet("api/v{version:apiVersion}/Orders/", HandleAsync)
                .WithName("OrdersGetAll")
                .Produces<List<Order>>(statusCode: 200, contentType: "application/json")
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithOpenApi(operation => new OpenApiOperation(operation) { Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Ordini" } } })
                .DisableAntiforgery()
                .WithApiVersionSet(versionSet)
                .MapToApiVersion(1);
        }

        private async Task<IResult> HandleAsync(IDispatcher dispatcher)
        {
            var query = new GetEntitiesQuery<Order>();
            var result = await dispatcher.DispatchAsync(query);

            if (result.Success)
            {   
                return Results.Ok(
                    result.Result?.Select(
                        a=> new OrderItemResult(
                            a.Id,
                            a.Confirmed, 
                            new OrderItemProductResult(
                                a.Product.Id, 
                                a.Product.Description, 
                                a.Product.Price)
                        )
                    )
                );
            }

            return Results.Problem(
                "Errore in fase di lettura", 
                null, 
                null, 
                "Lista ordini", 
                null, 
                result.Messages?.ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => (object?)kvp.Value
                                ) ?? new Dictionary<string, object?>() 
            );
        }
    }

    public record OrderItemResult(int Id, bool Confirmed, OrderItemProductResult Product);
    public record OrderItemProductResult(int Id, string? Description, string? Price);

}
