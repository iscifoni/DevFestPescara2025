using System.Reflection;

using Asp.Versioning.Conventions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using MIB.Core.Domain.Events;

namespace MIB.Core.Infrastructure.Events.Dapr;

public static class DaprIHostApplicationBuilderExtension
{
    // funzione locale per creare un nome leggibile per tipi generici
    
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1118:Mark local variable as const", Justification = "<Pending>")]
    public static WebApplication AddSubscriberConfiguration(this WebApplication builder)
    {
        var versionSet = builder.NewApiVersionSet("queues")
            .HasApiVersion(1)
            .ReportApiVersions()
            .Build();

        // Dapr configurations
        builder.UseCloudEvents();

        //Micerco tutti gli handler che sono sottoscritti alle code
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName?.Contains("Application", StringComparison.CurrentCultureIgnoreCase) ?? false))
        {
            foreach (var t in asm.GetTypes())
            {
                var localInterface = t.GetInterface("IOutboxDomainEventHandler`1");
                if (localInterface is not null)
                {
                    var message = localInterface.GenericTypeArguments[0];

                    var oeAttribute = message.GetCustomAttribute<OutboxEventAttribute>(true);
                    string PubSubName = oeAttribute?.PubSubName ?? "pubsub";
                    string TopicName = oeAttribute?.TopicName ?? Utility.GetFriendlyTypeName(message);

                    var endPoint = $"queue/{PubSubName}/{TopicName}";

                    builder.MapPost(
                        endPoint,
                        async (IServiceProvider service, HttpRequest request, CancellationToken cancellationToken) =>
                        {
                            var jsonObject = await request.ReadFromJsonAsync(message, cancellationToken: cancellationToken);
                            var messageObject = Convert.ChangeType(jsonObject, message) as IOutBoxDomainEvent;

                            var EndPointHandler = service.GetRequiredService(localInterface);

                            var method = localInterface.GetMethod("Handle");
                            var parametersArray = new object[] { messageObject!, cancellationToken };
                            _ = method?.Invoke(EndPointHandler, parametersArray);
                        })
                        .WithName(t.Name)
                        .WithOpenApi(operation => new OpenApiOperation(operation) { Tags = new List<OpenApiTag> { new OpenApiTag { Name = "queue" } } })
                        .DisableAntiforgery()
                        .WithApiVersionSet(versionSet)
                        .MapToApiVersion(1)
                        .WithTopic(PubSubName, TopicName);
                }
            }
        }

        builder.MapSubscribeHandler();

        return builder;
    }
}