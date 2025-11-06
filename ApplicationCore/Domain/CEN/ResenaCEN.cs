using System;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    [System.Obsolete("Esta clase está obsoleta. Use ResenyaCEN en su lugar.", true)]
    public class ResenaCEN
    {
        protected readonly IResenaRepository _resenaRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public ResenaCEN(IResenaRepository resenaRepository, IUnitOfWork unitOfWork)
        {
            _resenaRepository = resenaRepository;
            _unitOfWork = unitOfWork;
        }

        public virtual long Crear(decimal valoracion, string comentario, DateTime fechaPublicacion)
        {
            var resena = new Resena
            {
                Valoracion = valoracion,
                Comentario = comentario,
                FechaPublicacion = fechaPublicacion
            };

            _resenaRepository.New(resena);
            _unitOfWork.SaveChanges();
            return resena.Id;
        }

        public virtual void Modificar(long id, decimal valoracion, string comentario)
        {
            var resena = _resenaRepository.ReadById(id);
            if (resena == null)
                throw new Exception($"Reseña {id} no encontrada");

            resena.Valoracion = valoracion;
            resena.Comentario = comentario;

            _resenaRepository.Modify(resena);
            _unitOfWork.SaveChanges();
        }

        public virtual void Eliminar(long id)
        {
            var resena = _resenaRepository.ReadById(id);
            if (resena == null)
                throw new Exception($"Reseña {id} no encontrada");

            _resenaRepository.Delete(resena);
            _unitOfWork.SaveChanges();
        }

            public virtual Resena ObtenerPorId(long id)
        {
            return _resenaRepository.ReadById(id);
        }

            public virtual IList<Resena> ObtenerTodas()
        {
            return _resenaRepository.ReadAll();
        }
    }
}

// New CEN aligned with new.puml naming: Resenya
namespace ApplicationCore.Domain.CEN
{
    public class ResenyaCEN
    {
        protected readonly ApplicationCore.Domain.Repositories.IResenyaRepository _resenyaRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public ResenyaCEN(ApplicationCore.Domain.Repositories.IResenyaRepository resenyaRepository, IUnitOfWork unitOfWork)
        {
            _resenyaRepository = resenyaRepository;
            _unitOfWork = unitOfWork;
        }

        public virtual long Crear(long punctuation, string? comentario, DateTime fecha)
        {
            var resenya = new EN.Resenya
            {
                Punctuation = punctuation,
                Comentario = comentario,
                Fecha = fecha
            };

            _resenyaRepository.New(resenya);
            _unitOfWork.SaveChanges();
            return resenya.Id;
        }

        public virtual void AsignarAutor(long resenyaId, long autorId)
        {
            var resenya = _resenyaRepository.ReadById(resenyaId);
            if (resenya == null)
                throw new Exception($"Reseña {resenyaId} no encontrada");

            resenya.Autor = new EN.Usuario { Id = autorId };
            _resenyaRepository.Modify(resenya);
            _unitOfWork.SaveChanges();
        }

        public virtual void AsignarPelicula(long resenyaId, long peliculaId)
        {
            var resenya = _resenyaRepository.ReadById(resenyaId);
            if (resenya == null)
                throw new Exception($"Reseña {resenyaId} no encontrada");

            resenya.Pelicula = new EN.Pelicula { Id = peliculaId };
            _resenyaRepository.Modify(resenya);
            _unitOfWork.SaveChanges();
        }

        public virtual void Modificar(long id, long punctuation, string? comentario)
        {
            var resenya = _resenyaRepository.ReadById(id);
            if (resenya == null)
                throw new Exception($"Reseña {id} no encontrada");

            resenya.Punctuation = punctuation;
            resenya.Comentario = comentario;

            _resenyaRepository.Modify(resenya);
            _unitOfWork.SaveChanges();
        }

        public virtual void Eliminar(long id)
        {
            var resenya = _resenyaRepository.ReadById(id);
            if (resenya == null)
                throw new Exception($"Reseña {id} no encontrada");

            _resenyaRepository.Delete(resenya);
            _unitOfWork.SaveChanges();
        }

        public virtual EN.Resenya ObtenerPorId(long id)
        {
            return _resenyaRepository.ReadById(id);
        }

        public virtual IList<EN.Resenya> ObtenerTodas()
        {
            return _resenyaRepository.ReadAll();
        }
    }
}