using System.Collections.Generic;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class UsuarioCEN
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioCEN(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public void Crear(Usuario u)
        {
            // Validaciones de negocio ligeras pueden ir aquí
            _repo.New(u);
        }

        public void Modificar(Usuario u)
        {
            _repo.Modify(u);
        }

        public void Eliminar(long id)
        {
            _repo.Destroy(id);
        }

        public IEnumerable<Usuario> LeerTodos() => _repo.GetAll();
        public Usuario LeerPorId(long id) => _repo.GetById(id);
    }
}
