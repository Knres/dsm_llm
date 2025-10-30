using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.NHibernate.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ISession session) : base(session) { }

        public IList<Usuario> ReadFilter(string filtro)
        {
            // Simple example: filter by Nombre or Email
            return _session.Query<Usuario>()
                .Where(u => u.Nombre.Contains(filtro) || u.Email.Contains(filtro))
                .ToList();
        }
    }
}
