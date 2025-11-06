using System;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP
{
    public class ManageReportesCP
    {
        private readonly ReporteCEN _reporteCEN;
    private readonly ResenyaCEN _resenaCEN;
        private readonly UsuarioCEN _usuarioCEN;
        private readonly NotificacionCEN _notificacionCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReporteRepository _reporteRepository;
    private readonly ApplicationCore.Domain.Repositories.IResenyaRepository _resena_repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificacionRepository _notificacionRepository;

        public ManageReportesCP(
            ReporteCEN reporteCEN,
            ResenyaCEN resenaCEN,
            UsuarioCEN usuarioCEN,
            NotificacionCEN notificacionCEN,
            IUnitOfWork unitOfWork,
            IReporteRepository reporteRepository,
            ApplicationCore.Domain.Repositories.IResenyaRepository resenaRepository,
            IUsuarioRepository usuarioRepository,
            INotificacionRepository notificacionRepository)
        {
            _reporteCEN = reporteCEN;
            _resenaCEN = resenaCEN;
            _usuarioCEN = usuarioCEN;
            _notificacionCEN = notificacionCEN;
            _unitOfWork = unitOfWork;
            _reporteRepository = reporteRepository;
            _resena_repository = resenaRepository;
            _usuarioRepository = usuarioRepository;
            _notificacionRepository = notificacionRepository;
        }

        public virtual long CrearReporteYNotificar(long resenaId, long autorId, string motivo)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe la reseña
                var resenya = _resena_repository.ReadById(resenaId);
                if (resenya == null)
                    throw new Exception($"Reseña {resenaId} no encontrada");

                // Verificar que existe el autor del reporte
                var autor = _usuarioRepository.ReadById(autorId);
                if (autor == null)
                    throw new Exception($"Usuario {autorId} no encontrado");

                // Crear el reporte
                var reporte = new Reporte
                {
                    Motivo = motivo,
                    Estado = estadoReporte.Pendiente,
                    Fecha = DateTime.Now,
                    Autor = autor,
                    SobreResena = resenya
                };

                _reporteRepository.New(reporte);

                // Notificar a los administradores
                var notificacion = new Notificacion
                {
                    Mensaje = $"Nuevo reporte sobre la reseña #{resenaId}: {motivo}",
                    Fecha = DateTime.Now,
                    Tipo = tipoNotificacion.Reporte,
                    IdOrigen = reporte.Id
                };
                _notificacionRepository.New(notificacion);

                _unitOfWork.Commit();
                return reporte.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual void ProcesarReporte(long reporteId, estadoReporte nuevoEstado, string respuesta)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe el reporte
                var reporte = _reporteRepository.ReadById(reporteId);
                if (reporte == null)
                    throw new Exception($"Reporte {reporteId} no encontrado");

                // Actualizar el estado del reporte
                reporte.Estado = nuevoEstado;
                _reporteRepository.Modify(reporte);

                // Notificar al autor del reporte
                var notificacion = new Notificacion
                {
                    Mensaje = $"Tu reporte ha sido {nuevoEstado.ToString().ToLower()}: {respuesta}",
                    Fecha = DateTime.Now,
                    Tipo = tipoNotificacion.Reporte,
                    IdOrigen = reporteId,
                    Destinatario = reporte.Autor
                };
                _notificacionRepository.New(notificacion);

                // Si el reporte fue resuelto y se determinó eliminar la resenya
                if (nuevoEstado == estadoReporte.Resuelto && respuesta.Contains("reseña eliminada"))
                {
                    _resena_repository.Delete(reporte.SobreResena);
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