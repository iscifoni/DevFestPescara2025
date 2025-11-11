namespace MIB.Core.Domain.Commands
{
    public interface ICommandProvider
    {
        public Task SendAsync(ICommand message, CancellationToken cancellationToken = default);

        public Task<M> SendAsync<T, M>(T message, CancellationToken cancellationToken = default) where T : ICommand<M>;
    }
}