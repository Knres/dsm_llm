using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System.Collections.Generic;

namespace ApplicationCore.Domain.CEN
{
    public class PeliculaCEN
    {
        private readonly IPeliculaRepository _repo;
        private readonly IUnitOfWork _uow;

        public PeliculaCEN(IPeliculaRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Pelicula Create(string titulo, string? tituloOriginal = null, long? ano = null, long? duracion = null, string? pais = null, string? director = null, string? genero = null, string? sinopsis = null)
        {
            var p = new Pelicula
            {
                Titulo = titulo,
                TituloOriginal = tituloOriginal,
                Ano = ano,
                Duracion = duracion,
                Pais = pais,
                Director = director,
                Genero = genero,
                Sinopsis = sinopsis
            };
            _repo.New(p);
            _uow.SaveChanges();
            return p;
        }

        public void Modify(Pelicula pelicula)
        {
            _repo.Modify(pelicula);
            _uow.SaveChanges();
        }

        public void Destroy(Pelicula pelicula)
        {
            _repo.Destroy(pelicula);
            _uow.SaveChanges();
        }

        public IList<Pelicula> ReadAll() => _repo.DameTodos();
        public Pelicula? ReadById(long id) => _repo.DamePorOID(id);
        public IList<Pelicula> ReadFilter(string filtro) => _repo.ReadFilter(filtro);
    }
}
