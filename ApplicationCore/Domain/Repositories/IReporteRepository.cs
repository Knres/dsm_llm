using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IReporteRepository : IRepository<Reporte, long>
    {
        IList<Reporte> ReadFilter(string filtro);
    }
}
