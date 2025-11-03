using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    public class MetricaRepository : BaseRepository, IMetricaRepository
    {
        public MetricaRepository(ISession session) : base(session)
        {
        }

        public IList<Metrica> ReadAll()
        {
            return _session.Query<Metrica>().ToList();
        }

        public Metrica ReadById(long id)
        {
            return _session.Get<Metrica>(id);
        }

        public void New(Metrica metrica)
        {
            _session.Save(metrica);
        }

        public void Delete(Metrica metrica)
        {
            _session.Delete(metrica);
        }

        public void Modify(Metrica metrica)
        {
            _session.Update(metrica);
        }
    }
}