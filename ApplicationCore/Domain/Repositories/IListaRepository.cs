using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IListaRepository : IRepository<Lista, long>
    {
        IList<Lista> ReadFilter(string filtro);
    }
}
