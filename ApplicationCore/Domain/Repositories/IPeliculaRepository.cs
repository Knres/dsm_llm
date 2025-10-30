using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IPeliculaRepository : IRepository<Pelicula, long>
    {
        IList<Pelicula> ReadFilter(string filtro);
    }
}
