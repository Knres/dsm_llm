using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.NHibernate.Repositories
{
    public class NHibernateUnitOfWork : IUnitOfWork, System.IDisposable
    {
        private readonly ISession _session;
        private ITransaction? _transaction;

        public NHibernateUnitOfWork(ISession session)
        {
            _session = session;
            _transaction = _session.BeginTransaction();
        }

        public void SaveChanges()
        {
            if (_transaction is null) _transaction = _session.BeginTransaction();
            try
            {
                _session.Flush();
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _session?.Dispose();
        }
    }
}
