using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    [System.Obsolete("Esta clase está obsoleta. Use ResenyaRepository en su lugar.", true)]
    public class ResenaRepository : BaseRepository, IResenaRepository, IResenyaRepository
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

        // IResenyaRepository implementations (work on Resenya entity)
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

        public IList<Resenya> ReadAllResenya()
        {
            return _session.Query<Resenya>().ToList();
        }

        public Resenya ReadByIdResenya(long id)
        {
            return _session.Get<Resenya>(id);
        }

        public IList<Resenya> ReadByFilterResenya(string filter)
        {
            return _session.Query<Resenya>()
                .Where(r => r.Comentario != null && r.Comentario.Contains(filter))
                .ToList();
        }

        // Explicit interface implementations to disambiguate method names
        // when implementing both IResenaRepository and IResenyaRepository
        ApplicationCore.Domain.EN.Resenya ApplicationCore.Domain.Repositories.IResenyaRepository.ReadById(long id)
        {
            return ReadByIdResenya(id);
        }

        System.Collections.Generic.IList<ApplicationCore.Domain.EN.Resenya> ApplicationCore.Domain.Repositories.IResenyaRepository.ReadAll()
        {
            return ReadAllResenya();
        }

        System.Collections.Generic.IList<ApplicationCore.Domain.EN.Resenya> ApplicationCore.Domain.Repositories.IResenyaRepository.ReadByFilter(string filter)
        {
            return ReadByFilterResenya(filter);
        }
    }
}