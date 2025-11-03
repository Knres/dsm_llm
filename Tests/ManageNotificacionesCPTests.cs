using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.CP;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Enums;
using ApplicationCore.Domain.Repositories;
using Moq;
using Xunit;

namespace Tests
{
    public class ManageNotificacionesCPTests
    {
        private readonly Mock<INotificacionRepository> _notificacionRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<NotificacionCEN> _notificacionCENMock;
        private readonly Mock<UsuarioCEN> _usuarioCENMock;
        private readonly ManageNotificacionesCP _manageNotificacionesCP;

        public ManageNotificacionesCPTests()
        {
            _notificacionRepositoryMock = new Mock<INotificacionRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _notificacionCENMock = new Mock<NotificacionCEN>(_notificacionRepositoryMock.Object, _unitOfWorkMock.Object);
            _usuarioCENMock = new Mock<UsuarioCEN>(_usuarioRepositoryMock.Object, _unitOfWorkMock.Object);

            _manageNotificacionesCP = new ManageNotificacionesCP(
                _notificacionCENMock.Object,
                _usuarioCENMock.Object,
                _unitOfWorkMock.Object,
                _notificacionRepositoryMock.Object,
                _usuarioRepositoryMock.Object
            );
        }

        [Fact]
        public void EnviarNotificacionMasiva_DeberiaCrearNotificacionesParaDestinatariosValidos()
        {
            // Arrange
            var mensaje = "Notificación de prueba";
            var tipo = tipoNotificacion.Anuncio;
            var destinatariosIds = new List<long> { 1L, 2L, 3L };
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 1L },
                new Usuario { Id = 2L }
                // Usuario 3 no existe
            };

            _usuarioRepositoryMock.Setup(r => r.ReadById(1L)).Returns(usuarios[0]);
            _usuarioRepositoryMock.Setup(r => r.ReadById(2L)).Returns(usuarios[1]);
            _usuarioRepositoryMock.Setup(r => r.ReadById(3L)).Returns((Usuario)null);

            // Act
            _manageNotificacionesCP.EnviarNotificacionMasiva(mensaje, tipo, destinatariosIds);

            // Assert
            _notificacionRepositoryMock.Verify(r => r.New(It.Is<Notificacion>(n =>
                n.Mensaje == mensaje &&
                n.Tipo == tipo &&
                n.Leida == false)), Times.Exactly(2));

            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void MarcarNotificacionesComoLeidas_DeberiaActualizarNotificacionesDelUsuario()
        {
            // Arrange
            var usuarioId = 1L;
            var notificacionesIds = new List<long> { 1L, 2L, 3L };
            var usuario = new Usuario { Id = usuarioId };
            var notificaciones = new List<Notificacion>
            {
                new Notificacion { Id = 1L, Destinatario = usuario },
                new Notificacion { Id = 2L, Destinatario = usuario },
                new Notificacion { Id = 3L, Destinatario = new Usuario { Id = 2L } } // Notificación de otro usuario
            };

            _usuarioRepositoryMock.Setup(r => r.ReadById(usuarioId)).Returns(usuario);
            _notificacionRepositoryMock.Setup(r => r.ReadById(1L)).Returns(notificaciones[0]);
            _notificacionRepositoryMock.Setup(r => r.ReadById(2L)).Returns(notificaciones[1]);
            _notificacionRepositoryMock.Setup(r => r.ReadById(3L)).Returns(notificaciones[2]);

            // Act
            _manageNotificacionesCP.MarcarNotificacionesComoLeidas(usuarioId, notificacionesIds);

            // Assert
            _notificacionRepositoryMock.Verify(r => r.Modify(It.Is<Notificacion>(n =>
                n.Leida == true && n.FechaLeida != null)), Times.Exactly(2));

            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void EliminarNotificacionesAnterioresA_DeberiaEliminarNotificacionesLeidas()
        {
            // Arrange
            var fechaLimite = DateTime.Now.AddDays(-7);
            var notificaciones = new List<Notificacion>
            {
                new Notificacion { Id = 1L, Fecha = fechaLimite.AddDays(-1), Leida = true },
                new Notificacion { Id = 2L, Fecha = fechaLimite.AddDays(-2), Leida = true },
                new Notificacion { Id = 3L, Fecha = fechaLimite.AddDays(-3), Leida = false }, // No leída
                new Notificacion { Id = 4L, Fecha = fechaLimite.AddDays(1), Leida = true } // Más reciente
            };

            _notificacionRepositoryMock.Setup(r => r.ReadAll()).Returns(notificaciones);

            // Act
            _manageNotificacionesCP.EliminarNotificacionesAnterioresA(fechaLimite);

            // Assert
            _notificacionRepositoryMock.Verify(r => r.Delete(It.Is<Notificacion>(n =>
                n.Fecha < fechaLimite && n.Leida)), Times.Exactly(2));

            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void MarcarNotificacionesComoLeidas_CuandoUsuarioNoExiste_DeberiaLanzarExcepcion()
        {
            // Arrange
            var usuarioId = 999L;
            _usuarioRepositoryMock.Setup(r => r.ReadById(usuarioId)).Returns((Usuario)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                _manageNotificacionesCP.MarcarNotificacionesComoLeidas(usuarioId, new List<long> { 1L }));
            Assert.Contains("no encontrado", ex.Message);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
        }
    }
}