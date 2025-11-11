namespace MIB.Core.Domain.Events
{
    public interface IEventProvider
    {
        public Task SendAsync<T>(T message, CancellationToken cancellationToken = default) where T : IDomainEvent;
    }
}