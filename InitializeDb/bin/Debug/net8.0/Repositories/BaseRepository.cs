using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.NHibernate.Repositories
{
    public class BaseRepository<T> : IRepository<T, long> where T : class
    {
        protected readonly ISession _session;

        public BaseRepository(ISession session)
        {
            _session = session;
        }

        public virtual T? DamePorOID(long id) => _session.Get<T>(id);

        public virtual IList<T> DameTodos() => _session.Query<T>().ToList();

        public virtual void New(T entity) => _session.Save(entity);

        public virtual void Modify(T entity) => _session.Update(entity);

        public virtual void Destroy(T entity) => _session.Delete(entity);
    }
}
