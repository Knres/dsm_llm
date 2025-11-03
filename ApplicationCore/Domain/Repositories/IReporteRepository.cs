using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IReporteRepository
    {
        Reporte GetById(long id);
        IEnumerable<Reporte> GetAll();
        void New(Reporte entity);
        void Modify(Reporte entity);
        void Destroy(long id);
        IEnumerable<Reporte> FindByFilter(object filter);
    }
}
