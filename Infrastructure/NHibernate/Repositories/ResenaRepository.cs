using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.NHibernate.Repositories
{
    public class ResenaRepository : BaseRepository<Resena>, IResenaRepository
    {
        public ResenaRepository(ISession session) : base(session) { }

        public IList<Resena> ReadFilter(string filtro)
        {
            return _session.Query<Resena>()
                .Where(r => r.Comentario != null && r.Comentario.Contains(filtro))
                .ToList();
        }
    }
}
