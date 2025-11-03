using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IResenaRepository
    {
        Resena ReadById(long id);
        IList<Resena> ReadAll();
        void New(Resena entity);
        void Modify(Resena entity);
        void Delete(Resena entity);
        IList<Resena> ReadByFilter(string filter);
    }
}
