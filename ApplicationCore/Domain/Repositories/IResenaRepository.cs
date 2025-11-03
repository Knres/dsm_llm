using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IResenaRepository
    {
        Resena GetById(long id);
        IEnumerable<Resena> GetAll();
        void New(Resena entity);
        void Modify(Resena entity);
        void Destroy(long id);
        IEnumerable<Resena> FindByFilter(object filter);
    }
}
