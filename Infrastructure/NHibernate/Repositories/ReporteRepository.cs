using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.NHibernate.Repositories
{
    public class ReporteRepository : BaseRepository<Reporte>, IReporteRepository
    {
        public ReporteRepository(ISession session) : base(session) { }

        public IList<Reporte> ReadFilter(string filtro)
        {
            return _session.Query<Reporte>()
                .Where(r => r.Motivo.Contains(filtro))
                .ToList();
        }
    }
}
