using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IPeliculaRepository
    {
        Pelicula GetById(long id);
        IEnumerable<Pelicula> GetAll();
        void New(Pelicula entity);
        void Modify(Pelicula entity);
        void Destroy(long id);
        IEnumerable<Pelicula> FindByFilter(object filter);
    }
}
