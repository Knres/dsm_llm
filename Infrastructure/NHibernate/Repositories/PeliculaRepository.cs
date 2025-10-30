using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.NHibernate.Repositories
{
    public class PeliculaRepository : BaseRepository<Pelicula>, IPeliculaRepository
    {
        public PeliculaRepository(ISession session) : base(session) { }

        public IList<Pelicula> ReadFilter(string filtro)
        {
            return _session.Query<Pelicula>()
                .Where(p => p.Titulo.Contains(filtro) || (p.TituloOriginal != null && p.TituloOriginal.Contains(filtro)))
                .ToList();
        }
    }
}
