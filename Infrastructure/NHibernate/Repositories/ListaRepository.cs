using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.NHibernate.Repositories
{
    public class ListaRepository : BaseRepository<Lista>, IListaRepository
    {
        public ListaRepository(ISession session) : base(session) { }

        public IList<Lista> ReadFilter(string filtro)
        {
            return _session.Query<Lista>()
                .Where(l => l.Nombre.Contains(filtro))
                .ToList();
        }
    }
}
