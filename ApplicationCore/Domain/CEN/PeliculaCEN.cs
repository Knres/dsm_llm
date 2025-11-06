using System.Collections.Generic;
using System.Linq;
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

        public virtual long Crear(string titulo, string tituloOriginal, long anio, long duracion, string pais, string director, string genero, string sinopsis, decimal valoracionMedia)
        {
            var pelicula = new Pelicula
            {
                Titulo = titulo,
                TituloOriginal = tituloOriginal,
                Anio = anio,
                Duracion = duracion,
                Pais = pais,
                Director = director,
                Genero = genero,
                Sinopsis = sinopsis,
                ValoracionMedia = valoracionMedia
            };

            _repo.New(pelicula);
            _unitOfWork.SaveChanges();
            return pelicula.Id;
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

        public virtual IList<Pelicula> ReadFilterGenero(string genero)
        {
            if (string.IsNullOrWhiteSpace(genero))
                return new List<Pelicula>();

            return _repo.ReadByFilter(genero)
                .Where(p => p.Genero != null && 
                           p.Genero.Contains(genero, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public virtual IList<Pelicula> ReadFilterValoracion(decimal valoracionMinima, decimal valoracionMaxima)
        {
            if (valoracionMinima < 0 || valoracionMaxima > 10 || valoracionMinima > valoracionMaxima)
                return new List<Pelicula>();

            return _repo.ReadAll()
                .Where(p => p.ValoracionMedia >= valoracionMinima && 
                           p.ValoracionMedia <= valoracionMaxima)
                .ToList();
        }

        public virtual IList<Pelicula> ReadFilterAnyo(long anyo)
        {
            if (anyo <= 0)
                return new List<Pelicula>();

            return _repo.ReadAll()
                .Where(p => p.Anio == anyo)
                .ToList();
        }

        public virtual IList<Pelicula> ReadFilterTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return new List<Pelicula>();

            return _repo.ReadByFilter(titulo)
                .Where(p => (p.Titulo != null && 
                            p.Titulo.Contains(titulo, System.StringComparison.OrdinalIgnoreCase)) ||
                           (p.TituloOriginal != null && 
                            p.TituloOriginal.Contains(titulo, System.StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }
    }
}
