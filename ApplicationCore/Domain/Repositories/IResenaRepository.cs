using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IResenaRepository : IRepository<Resena, long>
    {
        IList<Resena> ReadFilter(string filtro);
    }
}
