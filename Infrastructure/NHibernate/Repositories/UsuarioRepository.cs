using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        public UsuarioRepository(ISession session) : base(session)
        {
        }

        public void Delete(Usuario usuario)
        {
            _session.Delete(usuario);
        }

        public void Modify(Usuario usuario)
        {
            _session.Update(usuario);
        }

        public void New(Usuario usuario)
        {
            _session.Save(usuario);
        }

        public IList<Usuario> ReadAll()
        {
            return _session.Query<Usuario>().ToList();
        }

        public Usuario ReadById(long id)
        {
            return _session.Get<Usuario>(id);
        }

        public IList<Usuario> ReadByFilter(string filter)
        {
            return _session.Query<Usuario>()
                .Where(u => u.Nombre.Contains(filter) || u.Email.Contains(filter))
                .ToList();
        }
    }
}