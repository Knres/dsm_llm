using System;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP
{
    public class ManageListasYReportesCP
    {
        private readonly ListaCEN _listaCEN;
        private readonly ReporteCEN _reporteCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IListaRepository _listaRepository;
        private readonly IReporteRepository _reporteRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificacionRepository _notificacionRepository;

        public ManageListasYReportesCP(
            ListaCEN listaCEN,
            ReporteCEN reporteCEN,
            IUnitOfWork unitOfWork,
            IListaRepository listaRepository,
            IReporteRepository reporteRepository,
            IUsuarioRepository usuarioRepository,
            INotificacionRepository notificacionRepository)
        {
            _listaCEN = listaCEN;
            _reporteCEN = reporteCEN;
            _unitOfWork = unitOfWork;
            _listaRepository = listaRepository;
            _reporteRepository = reporteRepository;
            _usuarioRepository = usuarioRepository;
            _notificacionRepository = notificacionRepository;
        }

        public virtual long CrearListaPersonalizada(string nombre, long usuarioId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe el usuario
                var usuario = _usuarioRepository.ReadById(usuarioId);
                if (usuario == null)
                    throw new Exception($"Usuario {usuarioId} no encontrado");

                // Crear la lista
                var listaId = _listaCEN.Crear(nombre, tipoLista.Otra);

                // Crear notificación
                var notificacion = new Notificacion
                {
                    Mensaje = $"Has creado una nueva lista: {nombre}",
                    Fecha = DateTime.Now,
                    Tipo = tipoNotificacion.Otro,
                    IdOrigen = listaId
                };
                _notificacionRepository.New(notificacion);

                _unitOfWork.Commit();
                return listaId;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual long CrearYProcesarReporte(string motivo, long usuarioReportadoId, long usuarioReportanteId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existen los usuarios
                var usuarioReportado = _usuarioRepository.ReadById(usuarioReportadoId);
                if (usuarioReportado == null)
                    throw new Exception($"Usuario reportado {usuarioReportadoId} no encontrado");

                var usuarioReportante = _usuarioRepository.ReadById(usuarioReportanteId);
                if (usuarioReportante == null)
                    throw new Exception($"Usuario reportante {usuarioReportanteId} no encontrado");

                // Crear el reporte
                var reporteId = _reporteCEN.Crear(
                    motivo,
                    estadoReporte.Pendiente,
                    DateTime.Now
                );

                // Notificar al usuario que ha sido reportado
                var notificacion = new Notificacion
                {
                    Mensaje = "Tu cuenta ha sido reportada y está bajo revisión",
                    Fecha = DateTime.Now,
                    Tipo = tipoNotificacion.Reporte,
                    IdOrigen = reporteId
                };
                _notificacionRepository.New(notificacion);

                _unitOfWork.Commit();
                return reporteId;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual void ProcesarReporte(long reporteId, estadoReporte nuevoEstado)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var reporte = _reporteRepository.ReadById(reporteId);
                if (reporte == null)
                    throw new Exception($"Reporte {reporteId} no encontrado");

                // Actualizar estado del reporte
                reporte.Estado = nuevoEstado;
                _reporteRepository.Modify(reporte);

                // Crear notificación de resolución
                var mensaje = nuevoEstado switch
                {
                    estadoReporte.Resuelto => "Tu reporte ha sido procesado y resuelto",
                    estadoReporte.Rechazado => "Tu reporte ha sido revisado y rechazado",
                    _ => "Tu reporte está siendo revisado"
                };

                var notificacion = new Notificacion
                {
                    Mensaje = mensaje,
                    Fecha = DateTime.Now,
                    Tipo = tipoNotificacion.Reporte,
                    IdOrigen = reporteId
                };
                _notificacionRepository.New(notificacion);

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