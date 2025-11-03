using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IMetricaRepository
    {
        Metrica GetById(long id);
        IEnumerable<Metrica> GetAll();
    }
}
