using Asp.Versioning.Conventions;
using MIB.Core.Application.Commands;
using MIB.Core.Application.Query;
using MIB.Core.Domain;
using MIB.Core.Domain.Entities;
using MIB.Core.Infrastructure.EndPoint.MinimalApi;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;

namespace MIB.Microservices.OrderService.Api.features.Orders.Delete
{
    public class OrderDeleteEndPoint : IEndpointHandler
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            var versionSet = builder.NewApiVersionSet("OrderDelete")
              .HasApiVersion(1)
              .ReportApiVersions()
              .Build();

            builder.MapDelete("api/v{version:apiVersion}/Orders/{id}", HandleAsync)
                .WithName("OrderDelete")
                //.Produces<List<Order>>(statusCode: 200, contentType: "application/json")
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithOpenApi(operation => new OpenApiOperation(operation) { Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Ordini" } } })
                .DisableAntiforgery()
                .WithApiVersionSet(versionSet)
                .MapToApiVersion(1);
        }

        private async Task<IResult> HandleAsync(IDispatcher dispatcher, int id)
        {
            var order = await dispatcher.DispatchAsync(new GetEntitiesByIdQuery<Order>(id));

            var OrderDeleteCommand = new DeleteEntityCommand<Order>(order.Result!);
            await dispatcher.DispatchAsync(OrderDeleteCommand);

            return Results.Ok();
        }
    }
}
