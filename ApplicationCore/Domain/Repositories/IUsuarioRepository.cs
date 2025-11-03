using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario ReadById(long id);
        IList<Usuario> ReadAll();
        void New(Usuario entity);
        void Modify(Usuario entity);
        void Delete(Usuario entity);
        IList<Usuario> ReadByFilter(string filter);
    }
}
