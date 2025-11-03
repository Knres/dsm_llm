using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    public class ReporteRepository : BaseRepository, IReporteRepository
    {
        public ReporteRepository(ISession session) : base(session)
        {
        }

        public void Delete(Reporte reporte)
        {
            _session.Delete(reporte);
        }

        public void Modify(Reporte reporte)
        {
            _session.Update(reporte);
        }

        public void New(Reporte reporte)
        {
            _session.Save(reporte);
        }

        public IList<Reporte> ReadAll()
        {
            return _session.Query<Reporte>().ToList();
        }

        public Reporte ReadById(long id)
        {
            return _session.Get<Reporte>(id);
        }

        public IList<Reporte> ReadByFilter(string filter)
        {
            return _session.Query<Reporte>()
                .Where(r => r.Motivo.Contains(filter))
                .ToList();
        }
    }
}