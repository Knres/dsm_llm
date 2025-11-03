using System.Collections.Generic;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class PeliculaCEN
    {
        private readonly IPeliculaRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public PeliculaCEN(IPeliculaRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public virtual void Crear(Pelicula p)
        {
            _repo.New(p);
            _unitOfWork.SaveChanges();
        }

        public virtual void Modificar(Pelicula p)
        {
            _repo.Modify(p);
            _unitOfWork.SaveChanges();
        }

        public virtual void Eliminar(long id)
        {
            var pelicula = _repo.ReadById(id);
            if (pelicula != null)
                _repo.Delete(pelicula);
            _unitOfWork.SaveChanges();
        }

        public virtual IList<Pelicula> LeerTodos() => _repo.ReadAll();
        public virtual Pelicula LeerPorId(long id) => _repo.ReadById(id);
    }
}
