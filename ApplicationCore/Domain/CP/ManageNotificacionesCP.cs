using System;
using System.Collections.Generic;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP
{
    public class ManageNotificacionesCP
    {
        private readonly NotificacionCEN _notificacionCEN;
        private readonly UsuarioCEN _usuarioCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificacionRepository _notificacionRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ManageNotificacionesCP(
            NotificacionCEN notificacionCEN,
            UsuarioCEN usuarioCEN,
            IUnitOfWork unitOfWork,
            INotificacionRepository notificacionRepository,
            IUsuarioRepository usuarioRepository)
        {
            _notificacionCEN = notificacionCEN;
            _usuarioCEN = usuarioCEN;
            _unitOfWork = unitOfWork;
            _notificacionRepository = notificacionRepository;
            _usuarioRepository = usuarioRepository;
        }

    public virtual void EnviarNotificacionMasiva(string mensaje, tipoNotificacion tipo, IEnumerable<long> destinatariosIds)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                foreach (var destinatarioId in destinatariosIds)
                {
                    var destinatario = _usuarioRepository.ReadById(destinatarioId);
                    if (destinatario == null)
                        continue; // Saltamos usuarios que no existan

                    var notificacion = new Notificacion
                    {
                        Mensaje = mensaje,
                        Fecha = DateTime.Now,
                        Tipo = tipo,
                        Destinatario = destinatario,
                        Leida = false
                    };

                    _notificacionRepository.New(notificacion);
                }

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

    public virtual void MarcarNotificacionesComoLeidas(long usuarioId, IEnumerable<long> notificacionesIds)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var usuario = _usuarioRepository.ReadById(usuarioId);
                if (usuario == null)
                    throw new Exception($"Usuario {usuarioId} no encontrado");

                foreach (var notificacionId in notificacionesIds)
                {
                    var notificacion = _notificacionRepository.ReadById(notificacionId);
                    if (notificacion == null || notificacion.Destinatario?.Id != usuarioId)
                        continue; // Saltamos notificaciones que no existan o no pertenezcan al usuario

                    notificacion.Leida = true;
                    notificacion.FechaLeida = DateTime.Now;
                    _notificacionRepository.Modify(notificacion);
                }

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

    public virtual void EliminarNotificacionesAnterioresA(DateTime fecha)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var notificaciones = _notificacionRepository.ReadAll();
                foreach (var notificacion in notificaciones)
                {
                    if (notificacion.Fecha < fecha && notificacion.Leida)
                    {
                        _notificacionRepository.Delete(notificacion);
                    }
                }

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}