using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IMetricaRepository
    {
        Metrica ReadById(long id);
        IList<Metrica> ReadAll();
        void New(Metrica metrica);
        void Modify(Metrica metrica);
        void Delete(Metrica metrica);
    }
}
