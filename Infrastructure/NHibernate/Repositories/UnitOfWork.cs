using ApplicationCore.Domain.Repositories;
using NHibernate;

namespace Infrastructure.NHibernate.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private ITransaction? _transaction;

        public UnitOfWork(ISession session)
        {
            _session = session;
        }

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public void SaveChanges()
        {
            try
            {
                BeginTransaction();
                _session.Flush();
                Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
        }
    }
}