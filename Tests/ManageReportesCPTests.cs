using System;
using System.Collections.Generic;
using ApplicationCore.Domain.CP;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Enums;
using ApplicationCore.Domain.Repositories;
using Moq;
using Xunit;

namespace Tests
{
    public class ManageReportesCPTests
    {
        private readonly Mock<IReporteRepository> _reporteRepositoryMock;
        private readonly Mock<IResenyaRepository> _resenaRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<INotificacionRepository> _notificacionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ReporteCEN> _reporteCENMock;
        private readonly Mock<ResenyaCEN> _resenaCENMock;
        private readonly Mock<UsuarioCEN> _usuarioCENMock;
        private readonly Mock<NotificacionCEN> _notificacionCENMock;
        private readonly ManageReportesCP _manageReportesCP;

        public ManageReportesCPTests()
        {
            _reporteRepositoryMock = new Mock<IReporteRepository>();
            _resenaRepositoryMock = new Mock<IResenyaRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _notificacionRepositoryMock = new Mock<INotificacionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _reporteCENMock = new Mock<ReporteCEN>(_reporteRepositoryMock.Object, _unitOfWorkMock.Object);
            _resenaCENMock = new Mock<ResenyaCEN>(_resenaRepositoryMock.Object, _unitOfWorkMock.Object);
            _usuarioCENMock = new Mock<UsuarioCEN>(_usuarioRepositoryMock.Object, _unitOfWorkMock.Object);
            _notificacionCENMock = new Mock<NotificacionCEN>(_notificacionRepositoryMock.Object, _unitOfWorkMock.Object);

            _manageReportesCP = new ManageReportesCP(
                _reporteCENMock.Object,
                _resenaCENMock.Object,
                _usuarioCENMock.Object,
                _notificacionCENMock.Object,
                _unitOfWorkMock.Object,
                _reporteRepositoryMock.Object,
                _resenaRepositoryMock.Object,
                _usuarioRepositoryMock.Object,
                _notificacionRepositoryMock.Object
            );
        }

        [Fact]
        public void CrearReporteYNotificar_DeberiaCrearReporteYNotificacion()
        {
            // Arrange
            var resenaId = 1L;
            var autorId = 2L;
            var motivo = "Contenido inapropiado";

            var resenya = new Resenya { Id = resenaId };
            var autor = new Usuario { Id = autorId };

            _resenaRepositoryMock.Setup(r => r.ReadById(resenaId)).Returns(resenya);
            _usuarioRepositoryMock.Setup(r => r.ReadById(autorId)).Returns(autor);

            // Act
            var reporteId = _manageReportesCP.CrearReporteYNotificar(resenaId, autorId, motivo);

            // Assert
            _reporteRepositoryMock.Verify(r => r.New(It.Is<Reporte>(rep =>
                rep.Motivo == motivo &&
                rep.Estado == estadoReporte.Pendiente &&
                rep.Autor == autor &&
                rep.SobreResena == resenya)), Times.Once);

            _notificacionRepositoryMock.Verify(r => r.New(It.Is<Notificacion>(n =>
                n.Tipo == tipoNotificacion.Reporte &&
                n.Mensaje.Contains(motivo))), Times.Once);

            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void ProcesarReporte_DeberiaActualizarEstadoYNotificar()
        {
            // Arrange
            var reporteId = 1L;
            var nuevoEstado = estadoReporte.Resuelto;
            var respuesta = "Reporte validado, reseña eliminada";

            var autor = new Usuario { Id = 2L };
            var resenya = new Resenya { Id = 3L };
            var reporte = new Reporte
            {
                Id = reporteId,
                Autor = autor,
                SobreResena = resenya,
                Estado = estadoReporte.Pendiente
            };

            _reporteRepositoryMock.Setup(r => r.ReadById(reporteId)).Returns(reporte);

            // Act
            _manageReportesCP.ProcesarReporte(reporteId, nuevoEstado, respuesta);

            // Assert
            _reporteRepositoryMock.Verify(r => r.Modify(It.Is<Reporte>(rep =>
                rep.Estado == nuevoEstado)), Times.Once);

            _notificacionRepositoryMock.Verify(r => r.New(It.Is<Notificacion>(n =>
                n.Tipo == tipoNotificacion.Reporte &&
                n.Destinatario == autor &&
                n.Mensaje.Contains(respuesta))), Times.Once);

            _resenaRepositoryMock.Verify(r => r.Delete(resenya), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void CrearReporteYNotificar_CuandoResenaNoExiste_DeberiaLanzarExcepcion()
        {
            // Arrange
            var resenaId = 999L;
            var autorId = 1L;
            _resenaRepositoryMock.Setup(r => r.ReadById(resenaId)).Returns((Resenya)null);
            var autor = new Usuario { Id = autorId };
            _usuarioRepositoryMock.Setup(r => r.ReadById(autorId)).Returns(autor);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => 
                _manageReportesCP.CrearReporteYNotificar(resenaId, autorId, "motivo"));
            Assert.Contains("no encontrada", ex.Message);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
        }

        [Fact]
        public void CrearReporteYNotificar_CuandoAutorNoExiste_DeberiaLanzarExcepcion()
        {
            // Arrange
            var resenaId = 1L;
            var autorId = 999L;
            var resenya = new Resenya { Id = resenaId };
            _resenaRepositoryMock.Setup(r => r.ReadById(resenaId)).Returns(resenya);
            _usuarioRepositoryMock.Setup(r => r.ReadById(autorId)).Returns((Usuario)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => 
                _manageReportesCP.CrearReporteYNotificar(resenaId, autorId, "motivo"));
            Assert.Contains("no encontrado", ex.Message);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
        }
    }
}