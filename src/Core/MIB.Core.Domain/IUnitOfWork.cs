namespace MIB.Core.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        public bool HasActiveTransaction { get; }

        public Task<string> BeginTransactionAsync();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public Task<int> CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}