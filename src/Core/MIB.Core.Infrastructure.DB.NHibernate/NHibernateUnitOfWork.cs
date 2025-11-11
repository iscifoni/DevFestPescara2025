using NHibernate;

namespace MIB.Core.Infrastructure.NHibernate
{
    public class NHibernateUnitOfWork : INHibernateUnitOfWork
    {
        private bool disposedValue;

        private ITransaction? _transaction;

        public NHibernateUnitOfWork(ISessionFactoryProvider sessionFactory)
        {
            Database = sessionFactory.OpenSession();
        }

        public ISession Database { get; init; }

        public bool HasActiveTransaction => _transaction is not null;

        public Task<string> BeginTransactionAsync()
        {
            _transaction = Database.BeginTransaction();
            return Task.FromResult(_transaction.ToString()!);
        }

        public async Task<int> CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync(cancellationToken);
                return 0;
            }

            throw new NullReferenceException("No active transaction present");
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                return;
            }

            throw new NullReferenceException("No active transaction present");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
                return;

            if (disposing)
            {
                _transaction?.Dispose();
                Database.Close();
                Database.Dispose();
            }

            disposedValue = true;
            _transaction = null;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}