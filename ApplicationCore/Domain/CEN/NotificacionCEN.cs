using System;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Enums;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class NotificacionCEN
    {
        protected readonly INotificacionRepository _notificacionRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public NotificacionCEN(INotificacionRepository notificacionRepository, IUnitOfWork unitOfWork)
        {
            _notificacionRepository = notificacionRepository;
            _unitOfWork = unitOfWork;
        }

        public virtual long Crear(string mensaje, DateTime fecha, tipoNotificacion tipo, long? idOrigen = null)
        {
            var notificacion = new Notificacion
            {
                Mensaje = mensaje,
                Fecha = fecha,
                Tipo = tipo,
                IdOrigen = idOrigen
            };

            _notificacionRepository.New(notificacion);
            _unitOfWork.SaveChanges();
            return notificacion.Id;
        }

        public virtual void Eliminar(long id)
        {
            var notificacion = _notificacionRepository.ReadById(id);
            if (notificacion == null)
                throw new Exception($"Notificación {id} no encontrada");

            _notificacionRepository.Delete(notificacion);
            _unitOfWork.SaveChanges();
        }

        public virtual Notificacion ObtenerPorId(long id)
        {
            return _notificacionRepository.ReadById(id);
        }

        public virtual IList<Notificacion> ObtenerTodas()
        {
            return _notificacionRepository.ReadAll();
        }
    }
}