using System;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
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