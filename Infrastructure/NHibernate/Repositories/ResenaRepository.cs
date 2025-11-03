using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    public class ResenaRepository : BaseRepository, IResenaRepository
    {
        public ResenaRepository(ISession session) : base(session)
        {
        }

        public void Delete(Resena resena)
        {
            _session.Delete(resena);
        }

        public void Modify(Resena resena)
        {
            _session.Update(resena);
        }

        public void New(Resena resena)
        {
            _session.Save(resena);
        }

        public IList<Resena> ReadAll()
        {
            return _session.Query<Resena>().ToList();
        }

        public Resena ReadById(long id)
        {
            return _session.Get<Resena>(id);
        }

        public IList<Resena> ReadByFilter(string filter)
        {
            return _session.Query<Resena>()
                .Where(r => r.Comentario != null && r.Comentario.Contains(filter))
                .ToList();
        }
    }
}