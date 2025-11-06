using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    [System.Obsolete("Esta interfaz está obsoleta. Use IResenyaRepository en su lugar.", true)]
    public interface IResenaRepository
    {
        // Deprecated: use IResenyaRepository. This interface will be removed in future versions.
        Resena ReadById(long id);
        IList<Resena> ReadAll();
        void New(Resena entity);
        void Modify(Resena entity);
        void Delete(Resena entity);
        IList<Resena> ReadByFilter(string filter);
    }

    public interface IResenyaRepository
    {
        Resenya ReadById(long id);
        IList<Resenya> ReadAll();
        void New(Resenya entity);
        void Modify(Resenya entity);
        void Delete(Resenya entity);
        IList<Resenya> ReadByFilter(string filter);
    }
}
