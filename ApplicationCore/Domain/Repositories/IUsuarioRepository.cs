using ApplicationCore.Domain.EN;
using System.Collections.Generic;

namespace ApplicationCore.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario, long>
    {
        IList<Usuario> ReadFilter(string filtro);
    }
}
