using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IPeliculaRepository
    {
        Pelicula ReadById(long id);
        IList<Pelicula> ReadAll();
        void New(Pelicula entity);
        void Modify(Pelicula entity);
        void Delete(Pelicula entity);
        IList<Pelicula> ReadByFilter(string filter);
    }
}
