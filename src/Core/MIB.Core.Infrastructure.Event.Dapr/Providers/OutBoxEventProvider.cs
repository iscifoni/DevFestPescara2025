using System.Reflection;

using Dapr.Client;

using MIB.Core.Domain.Events;

namespace MIB.Core.Infrastructure.Events.Dapr.Providers;

internal class OutBoxEventProvider(DaprClient daprClient) : IOutboxEventProvider
{
    public async Task SendAsync<T>(T message, CancellationToken cancellationToken = default) where T : IDomainEvent
    {
        var oeAttribute = message.GetType().GetCustomAttribute<OutboxEventAttribute>(true);
        string PubSubName = oeAttribute?.PubSubName ?? "pubsub";
        string TopicName = oeAttribute?.TopicName ?? Utility.GetFriendlyTypeName(message.GetType() );

        await daprClient.PublishEventAsync(PubSubName, TopicName, message, cancellationToken);
    }
}