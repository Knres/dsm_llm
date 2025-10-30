using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IRepository<T, TKey>
    {
        T? DamePorOID(TKey id);
        IList<T> DameTodos();
        void New(T entity);
        void Modify(T entity);
        void Destroy(T entity);
    }
}
