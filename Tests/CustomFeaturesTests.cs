using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.CP;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.Enums;
using Moq;
using Xunit;

namespace Tests
{
    public class CustomFeaturesTests
    {
        [Fact]
        public void UsuarioCEN_ModoBlancoYnegro_TogglesAndModifiesWithoutSaveChanges()
        {
            var usuarioRepo = new Mock<IUsuarioRepository>();
            var uow = new Mock<IUnitOfWork>();

            var usuario = new Usuario { Id = 1, ModoBlancoYNegro = false };
            usuarioRepo.Setup(r => r.ReadById(1L)).Returns(usuario);

            var cen = new UsuarioCEN(usuarioRepo.Object, uow.Object);

            var result = cen.modoBlancoYnegro(1L);

            Assert.True(result);
            usuarioRepo.Verify(r => r.Modify(It.Is<Usuario>(u => u.ModoBlancoYNegro == true)), Times.Once);
            uow.Verify(u => u.SaveChanges(), Times.Never);
        }

        [Fact]
        public void ManageResenyasCP_CalcularValoracionMedia_ComputesRoundedValueAndCommits()
        {
            var resenaRepo = new Mock<ApplicationCore.Domain.Repositories.IResenyaRepository>();
            var peliculaRepo = new Mock<IPeliculaRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var pelicula = new Pelicula { Id = 10, Titulo = "P", ValoracionMedia = null };
            var resenyas = new List<Resenya>
            {
                new Resenya { Id = 1, Pelicula = pelicula, Punctuation = 7 },
                new Resenya { Id = 2, Pelicula = pelicula, Punctuation = 8 }
            };

            resenaRepo.Setup(r => r.ReadAll()).Returns(resenyas);
            peliculaRepo.Setup(r => r.ReadById(10L)).Returns(pelicula);

            var resenyaCENMock = new Mock<ResenyaCEN>(resenaRepo.Object, unitOfWork.Object);
            var peliculaCENMock = new Mock<PeliculaCEN>(peliculaRepo.Object, unitOfWork.Object);
            var usuarioCENMock = new Mock<UsuarioCEN>(new Mock<IUsuarioRepository>().Object, unitOfWork.Object);
            var notificacionCENMock = new Mock<NotificacionCEN>(new Mock<INotificacionRepository>().Object, unitOfWork.Object);

            var cp = new ManageResenyasCP(
                resenyaCENMock.Object,
                peliculaCENMock.Object,
                usuarioCENMock.Object,
                notificacionCENMock.Object,
                unitOfWork.Object,
                resenaRepo.Object,
                peliculaRepo.Object,
                new Mock<IUsuarioRepository>().Object,
                new Mock<INotificacionRepository>().Object
            );

            var resultado = cp.calcularValoracionMedia(10L);

            Assert.Equal(Math.Round((7m + 8m) / 2m, 2), resultado);
            peliculaRepo.Verify(r => r.Modify(It.Is<Pelicula>(p => p.ValoracionMedia == resultado)), Times.Once);
            unitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void ManagePeliculasCP_ContarResenyas_ReturnsCountAndCommits()
        {
            var peliculaRepo = new Mock<IPeliculaRepository>();
            var resenaRepo = new Mock<ApplicationCore.Domain.Repositories.IResenyaRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var pelicula = new Pelicula { Id = 5, Titulo = "X", Resenyas = new List<Resenya> { new Resenya(), new Resenya(), new Resenya() } };
            peliculaRepo.Setup(r => r.ReadById(5L)).Returns(pelicula);

            var peliculaCENMock = new Mock<PeliculaCEN>(peliculaRepo.Object, unitOfWork.Object);
            var resenyaCENMock = new Mock<ResenyaCEN>(resenaRepo.Object, unitOfWork.Object);

            var cp = new ManagePeliculasCP(
                peliculaCENMock.Object,
                resenyaCENMock.Object,
                unitOfWork.Object,
                peliculaRepo.Object,
                resenaRepo.Object,
                new Mock<IUsuarioRepository>().Object,
                new Mock<INotificacionRepository>().Object
            );

            var contador = cp.ContarResenyas(5L);

            Assert.Equal(3, contador);
            unitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void NotificacionCEN_CambiarEstadoNotificacion_UpdatesAndModifiesWithoutSave()
        {
            var notiRepo = new Mock<INotificacionRepository>();
            var uow = new Mock<IUnitOfWork>();

            var noti = new Notificacion { Id = 7, EstadoNotificacion = estadoNotificacion.NoLeida, Leida = false, FechaLeida = null };
            notiRepo.Setup(r => r.ReadById(7L)).Returns(noti);

            var cen = new NotificacionCEN(notiRepo.Object, uow.Object);

            var nuevo = cen.cambiarEstadoNotificacion(7L, estadoNotificacion.Leida);

            Assert.Equal(estadoNotificacion.Leida, nuevo);
            notiRepo.Verify(r => r.Modify(It.Is<Notificacion>(n => n.Leida == true && n.EstadoNotificacion == estadoNotificacion.Leida)), Times.Once);
            uow.Verify(u => u.SaveChanges(), Times.Never);
        }

        [Fact]
        public void ManageListasCP_AnyadirPelicula_AddsMultipleAndSkipsDuplicates()
        {
            var listaRepo = new Mock<IListaRepository>();
            var peliculaRepo = new Mock<IPeliculaRepository>();
            var uow = new Mock<IUnitOfWork>();

            var pelicula1 = new Pelicula { Id = 2, Titulo = "A" };
            var pelicula2 = new Pelicula { Id = 3, Titulo = "B" };
            var lista = new Lista { Id = 11, Peliculas = new List<Pelicula> { pelicula1 } };

            listaRepo.Setup(r => r.ReadById(11L)).Returns(lista);
            peliculaRepo.Setup(r => r.ReadById(2L)).Returns(pelicula1);
            peliculaRepo.Setup(r => r.ReadById(3L)).Returns(pelicula2);

            var listaCENMock = new Mock<ListaCEN>(listaRepo.Object, uow.Object);
            var usuarioCENMock = new Mock<UsuarioCEN>(new Mock<IUsuarioRepository>().Object, uow.Object);
            var peliculaCENMock = new Mock<PeliculaCEN>(peliculaRepo.Object, uow.Object);

            var cp = new ManageListasCP(
                listaCENMock.Object,
                usuarioCENMock.Object,
                peliculaCENMock.Object,
                uow.Object,
                listaRepo.Object,
                new Mock<IUsuarioRepository>().Object,
                peliculaRepo.Object
            );

            cp.AnyadirPelicula(11L, 2L, 3L, 2L); // 2L duplicated

            Assert.Equal(2, lista.Peliculas.Count); // pelicula1 and pelicula2
            listaRepo.Verify(r => r.Modify(It.IsAny<Lista>()), Times.Once);
            uow.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void ManageReportesCP_ResolverReporteYenviarMensaje_MarksResolvedAndCreatesNotification()
        {
            var reporteRepo = new Mock<IReporteRepository>();
            var resenaRepo = new Mock<ApplicationCore.Domain.Repositories.IResenyaRepository>();
            var usuarioRepo = new Mock<IUsuarioRepository>();
            var notiRepo = new Mock<INotificacionRepository>();
            var uow = new Mock<IUnitOfWork>();

            var autor = new Usuario { Id = 99, Nombre = "Reporter" };
            var reporte = new Reporte { Id = 50, Autor = autor, Estado = estadoReporte.Pendiente };

            reporteRepo.Setup(r => r.ReadById(50L)).Returns(reporte);

            var reporteCENMock = new Mock<ReporteCEN>(reporteRepo.Object, uow.Object);
            var resenyaCENMock = new Mock<ResenyaCEN>(resenaRepo.Object, uow.Object);
            var usuarioCENMock = new Mock<UsuarioCEN>(usuarioRepo.Object, uow.Object);
            var notificacionCENMock = new Mock<NotificacionCEN>(notiRepo.Object, uow.Object);

            var cp = new ManageReportesCP(
                reporteCENMock.Object,
                resenyaCENMock.Object,
                usuarioCENMock.Object,
                notificacionCENMock.Object,
                uow.Object,
                reporteRepo.Object,
                resenaRepo.Object,
                usuarioRepo.Object,
                notiRepo.Object
            );

            cp.ResolverReporteYenviarMensaje(50L, "Reporte resuelto: acción tomada");

            reporteRepo.Verify(r => r.Modify(It.Is<Reporte>(rp => rp.Estado == estadoReporte.Resuelto)), Times.Once);
            notiRepo.Verify(n => n.New(It.Is<Notificacion>(no => no.Destinatario == autor && no.Mensaje.Contains("Reporte resuelto"))), Times.Once);
            uow.Verify(u => u.Commit(), Times.Once);
        }
    }
}
