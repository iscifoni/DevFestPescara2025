using Asp.Versioning.Conventions;
using MIB.Core.Application.Query;
using MIB.Core.Domain;
using MIB.Core.Domain.Entities;
using MIB.Core.Infrastructure.EndPoint.MinimalApi;
using MIB.Microservices.OrderService.Api.features.Orders.GetAll;
using Microsoft.OpenApi.Models;

namespace MIB.Microservices.OrderService.Api.features.Inventory.GetAll
{
    public class InventoryGetAllEndPoint : IEndpointHandler
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            var versionSet = builder.NewApiVersionSet("InventoryGetAll")
              .HasApiVersion(1)
              .ReportApiVersions()
              .Build();

            builder.MapGet("api/v{version:apiVersion}/Inventory/", HandleAsync)
                .WithName("InventoryGetAll")
                .Produces<List<InventoryItem>>(statusCode: 200, contentType: "application/json")
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithOpenApi(operation => new OpenApiOperation(operation) { Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Magazzino" } } })
                .DisableAntiforgery()
                .WithApiVersionSet(versionSet)
                .MapToApiVersion(1);
        }

        private async Task<IResult> HandleAsync(IDispatcher dispatcher)
        {
            var query = new GetEntitiesQuery<InventoryItem>();
            var result = await dispatcher.DispatchAsync(query);

            if (result.Success)
            {
                return Results.Ok(
                    result.Result?.Select(
                        a => new InventoryItemResult(
                            a.Id,
                            a.Quantity,
                            new InventoryItemProductResult(
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
                "Lista magazzino",
                null,
                result.Messages?.ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => (object?)kvp.Value
                                ) ?? new Dictionary<string, object?>()
            );
        }
    }

    public record InventoryItemResult(int Id, short Quantity, InventoryItemProductResult Product);
    public record InventoryItemProductResult(int Id, string? Description, string? Price);
}
