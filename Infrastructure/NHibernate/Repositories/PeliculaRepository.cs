using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    public class PeliculaRepository : BaseRepository, IPeliculaRepository
    {
        public PeliculaRepository(ISession session) : base(session)
        {
        }

        public void Delete(Pelicula pelicula)
        {
            _session.Delete(pelicula);
        }

        public void Modify(Pelicula pelicula)
        {
            _session.Update(pelicula);
        }

        public void New(Pelicula pelicula)
        {
            _session.Save(pelicula);
        }

        public IList<Pelicula> ReadAll()
        {
            return _session.Query<Pelicula>().ToList();
        }

        public Pelicula ReadById(long id)
        {
            return _session.Get<Pelicula>(id);
        }

        public IList<Pelicula> ReadByFilter(string filter)
        {
            return _session.Query<Pelicula>()
                .Where(p => p.Titulo.Contains(filter) || 
                           (p.TituloOriginal != null && p.TituloOriginal.Contains(filter)) ||
                           (p.Director != null && p.Director.Contains(filter)) ||
                           (p.Genero != null && p.Genero.Contains(filter)))
                .ToList();
        }
    }
}