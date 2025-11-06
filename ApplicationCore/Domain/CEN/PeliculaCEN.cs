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

        public virtual IList<Pelicula> ReadFilterGenero(string genero)
        {
            if (string.IsNullOrWhiteSpace(genero))
                return new List<Pelicula>();

            return _repo.ReadByFilter(genero)
                .Where(p => p.Genero != null && 
                           p.Genero.Contains(genero, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Filtra películas por rango de valoración.
        /// </summary>
        /// <param name="valoracionMinima">Valoración mínima (0-10)</param>
        /// <param name="valoracionMaxima">Valoración máxima (0-10)</param>
        /// <returns>Lista de películas dentro del rango de valoración especificado</returns>
        public virtual IList<Pelicula> ReadFilterValoracion(decimal valoracionMinima, decimal valoracionMaxima)
        {
            if (valoracionMinima < 0 || valoracionMaxima > 10 || valoracionMinima > valoracionMaxima)
                return new List<Pelicula>();

            return _repo.ReadAll()
                .Where(p => p.ValoracionMedia >= valoracionMinima && 
                           p.ValoracionMedia <= valoracionMaxima)
                .ToList();
        }

        /// <summary>
        /// Filtra películas por año de lanzamiento.
        /// </summary>
        /// <param name="anyo">Año a buscar</param>
        /// <returns>Lista de películas del año especificado</returns>
        public virtual IList<Pelicula> ReadFilterAnyo(long anyo)
        {
            if (anyo <= 0)
                return new List<Pelicula>();

            return _repo.ReadAll()
                .Where(p => p.Anio == anyo)
                .ToList();
        }

        /// <summary>
        /// Filtra películas por título (original o traducido).
        /// La búsqueda es parcial y no distingue mayúsculas/minúsculas.
        /// </summary>
        /// <param name="titulo">Texto a buscar en el título</param>
        /// <returns>Lista de películas que contienen el texto en su título</returns>
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
