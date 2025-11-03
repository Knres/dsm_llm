using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IListaRepository
    {
        Lista ReadById(long id);
        IList<Lista> ReadAll();
        void New(Lista entity);
        void Modify(Lista entity);
        void Delete(Lista entity);
    }
}
