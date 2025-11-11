using Asp.Versioning.Conventions;
using MIB.Core.Application.Commands;
using MIB.Core.Application.Query;
using MIB.Core.Domain;
using MIB.Core.Domain.Entities;
using MIB.Core.Infrastructure.EndPoint.MinimalApi;
using Microsoft.OpenApi.Models;

namespace MIB.Microservices.OrderService.Api.features.Orders.Insert
{
    public class OrderInsertEndPoint : IEndpointHandler
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            var versionSet = builder.NewApiVersionSet("OrdersGetAll")
              .HasApiVersion(1)
              .ReportApiVersions()
              .Build();

            builder.MapPost("api/v{version:apiVersion}/Orders/", HandleAsync)
                .WithName("OrdersInsert")
                .Produces<Order>(statusCode: 200, contentType: "application/json")
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithOpenApi(operation => new OpenApiOperation(operation) { Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Ordini" } } })
                .DisableAntiforgery()
                .WithApiVersionSet(versionSet)
                .MapToApiVersion(1);
        }

        private async Task<IResult> HandleAsync(IDispatcher dispatcher, OrderInsertRequest request)
        {
            var query = new AddEntityCommand<Order>(new Order { Product = new Product { Id = request.idProdotto } });
            var result = await dispatcher.DispatchAsync(query);

            if (result.Success)
            {
                return Results.Ok(
                    new OrderItemResult(
                        result.Result!.Id,
                        result.Result.Confirmed
                    )
                );
            }

            return Results.Problem(
                "Errore in fase di inserimento",
                null,
                null,
                "Inserimento ordine",
                null,
                result.Messages?.ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => (object?)kvp.Value
                                ) ?? new Dictionary<string, object?>()
            );
        }

    }

    public record OrderInsertRequest(int idProdotto);

    public record OrderItemResult(int Id, bool Confirmed);
   
}
