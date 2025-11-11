namespace MIB.Core.Domain.Query
{
    public interface IQueryProviderCA
    {
        public Task<M> SendAsync<T, M>(T message, CancellationToken cancellationToken = default) where T : IQuery<M>;
    }
}