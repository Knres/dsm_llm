using System.Collections.Generic;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class PeliculaCEN
    {
        private readonly IPeliculaRepository _repo;

        public PeliculaCEN(IPeliculaRepository repo)
        {
            _repo = repo;
        }

        public void Crear(Pelicula p)
        {
            _repo.New(p);
        }

        public void Modificar(Pelicula p)
        {
            _repo.Modify(p);
        }

        public void Eliminar(long id)
        {
            _repo.Destroy(id);
        }

        public IEnumerable<Pelicula> LeerTodos() => _repo.GetAll();
        public Pelicula LeerPorId(long id) => _repo.GetById(id);
    }
}
