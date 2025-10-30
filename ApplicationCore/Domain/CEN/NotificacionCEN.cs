using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System.Collections.Generic;
using System;

namespace ApplicationCore.Domain.CEN
{
    public class NotificacionCEN
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsuarioRepository _usuarioRepo;

        public NotificacionCEN(IUsuarioRepository usuarioRepo, IUnitOfWork uow)
        {
            _usuarioRepo = usuarioRepo;
            _uow = uow;
        }

        public Notificacion Create(long usuarioId, string mensaje, DateTime fecha, ApplicationCore.Domain.Enums.TipoNotificacion tipo, long? idOrigen = null)
        {
            var n = new Notificacion
            {
                Mensaje = mensaje,
                Fecha = fecha,
                Tipo = tipo,
                IdOrigen = idOrigen
            };
            // repository for Notificacion not created earlier; would be similar to others. For now, assume Usuario repo will handle adding notification via relation
            // This is a placeholder to show the intended CEN surface.
            _uow.SaveChanges();
            return n;
        }

        // read methods would be implemented when NotificacionRepository exists
    }
}
