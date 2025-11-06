using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.NHibernate.Repositories
{
    public class ResenyaRepository : BaseRepository, ApplicationCore.Domain.Repositories.IResenyaRepository
    {
        public ResenyaRepository(ISession session) : base(session)
        {
        }

        public void New(Resenya resenya)
        {
            _session.Save(resenya);
        }

        public void Modify(Resenya resenya)
        {
            _session.Update(resenya);
        }

        public void Delete(Resenya resenya)
        {
            _session.Delete(resenya);
        }

        public IList<Resenya> ReadAll()
        {
            return _session.Query<Resenya>().ToList();
        }

        public Resenya ReadById(long id)
        {
            return _session.Get<Resenya>(id);
        }

        public IList<Resenya> ReadByFilter(string filter)
        {
            return _session.Query<Resenya>()
                .Where(r => r.Comentario != null && r.Comentario.Contains(filter))
                .ToList();
        }
    }
}