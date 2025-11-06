using System.Collections.Generic;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class UsuarioCEN
    {
        private readonly IUsuarioRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioCEN(IUsuarioRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public virtual void Crear(Usuario u)
        {
            // Validaciones de negocio ligeras pueden ir aquí
            _repo.New(u);
            _unitOfWork.SaveChanges();
        }

        public virtual void Modificar(Usuario u)
        {
            _repo.Modify(u);
            _unitOfWork.SaveChanges();
        }

        public virtual void Eliminar(long id)
        {
            var usuario = _repo.ReadById(id);
            if (usuario != null)
                _repo.Delete(usuario);
            _unitOfWork.SaveChanges();
        }

        public virtual IList<Usuario> LeerTodos() => _repo.ReadAll();
        public virtual Usuario LeerPorId(long id) => _repo.ReadById(id);

        public virtual bool modoBlancoYnegro(long userId)
        {
            var usuario = _repo.ReadById(userId);
            if (usuario == null)
                throw new System.Exception($"Usuario con ID {userId} no encontrado");

            usuario.ModoBlancoYNegro = !usuario.ModoBlancoYNegro;

            // Actualizamos el repositorio objeto (opcional) pero NO realizamos SaveChanges aquí
            _repo.Modify(usuario);

            return usuario.ModoBlancoYNegro;
        }
    }
}
