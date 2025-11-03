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
    }
}
