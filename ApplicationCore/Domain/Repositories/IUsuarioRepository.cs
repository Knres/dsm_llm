using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario GetById(long id);
        IEnumerable<Usuario> GetAll();
        void New(Usuario entity);
        void Modify(Usuario entity);
        void Destroy(long id);
        IEnumerable<Usuario> FindByFilter(object filter);
    }
}
