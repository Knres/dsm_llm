using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IListaRepository
    {
        Lista GetById(long id);
        IEnumerable<Lista> GetAll();
        void New(Lista entity);
        void Modify(Lista entity);
        void Destroy(long id);
    }
}
