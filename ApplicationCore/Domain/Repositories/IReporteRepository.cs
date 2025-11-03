using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IReporteRepository
    {
        Reporte ReadById(long id);
        IList<Reporte> ReadAll();
        void New(Reporte entity);
        void Modify(Reporte entity);
        void Delete(Reporte entity);
        IList<Reporte> ReadByFilter(string filter);
    }
}
