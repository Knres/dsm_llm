using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.CP;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using Moq;
using Xunit;

namespace Tests
{
    public class ManageMetricasCPTests
    {
        private readonly Mock<IMetricaRepository> _metricaRepositoryMock;
        private readonly Mock<IPeliculaRepository> _peliculaRepositoryMock;
        private readonly Mock<IResenaRepository> _resenaRepositoryMock;
        private readonly Mock<IListaRepository> _listaRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<MetricaCEN> _metricaCENMock;
        private readonly Mock<PeliculaCEN> _peliculaCENMock;
        private readonly Mock<ResenaCEN> _resenaCENMock;
        private readonly Mock<ListaCEN> _listaCENMock;
        private readonly ManageMetricasCP _manageMetricasCP;

        public ManageMetricasCPTests()
        {
            _metricaRepositoryMock = new Mock<IMetricaRepository>();
            _peliculaRepositoryMock = new Mock<IPeliculaRepository>();
            _resenaRepositoryMock = new Mock<IResenaRepository>();
            _listaRepositoryMock = new Mock<IListaRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _metricaCENMock = new Mock<MetricaCEN>(_metricaRepositoryMock.Object, _unitOfWorkMock.Object);
            _peliculaCENMock = new Mock<PeliculaCEN>(_peliculaRepositoryMock.Object, _unitOfWorkMock.Object);
            _resenaCENMock = new Mock<ResenaCEN>(_resenaRepositoryMock.Object, _unitOfWorkMock.Object);
            _listaCENMock = new Mock<ListaCEN>(_listaRepositoryMock.Object, _unitOfWorkMock.Object);

            _manageMetricasCP = new ManageMetricasCP(
                _metricaCENMock.Object,
                _peliculaCENMock.Object,
                _resenaCENMock.Object,
                _listaCENMock.Object,
                _unitOfWorkMock.Object,
                _metricaRepositoryMock.Object,
                _peliculaRepositoryMock.Object,
                _resenaRepositoryMock.Object,
                _listaRepositoryMock.Object
            );
        }

        [Fact]
        public void ActualizarMetricasPelicula_DeberiaCalcularMetricasCorrectamente()
        {
            // Arrange
            var peliculaId = 1L;
            var pelicula = new Pelicula { Id = peliculaId, Titulo = "Test Movie" };
            var resenas = new List<Resena>
            {
                new Resena { Id = 1, Pelicula = pelicula, Valoracion = 8 },
                new Resena { Id = 2, Pelicula = pelicula, Valoracion = 6 }
            };
            var listas = new List<Lista>
            {
                new Lista { Id = 1, Peliculas = new List<Pelicula> { pelicula } },
                new Lista { Id = 2, Peliculas = new List<Pelicula> { pelicula } }
            };

            _peliculaRepositoryMock.Setup(r => r.ReadById(peliculaId)).Returns(pelicula);
            _resenaRepositoryMock.Setup(r => r.ReadAll()).Returns(resenas);
            _listaRepositoryMock.Setup(r => r.ReadAll()).Returns(listas);
            _metricaRepositoryMock.Setup(r => r.ReadAll()).Returns(new List<Metrica>());

            // Act
            _manageMetricasCP.ActualizarMetricasPelicula(peliculaId);

            // Assert
            _metricaRepositoryMock.Verify(r => r.New(It.Is<Metrica>(m =>
                m.Pelicula == pelicula &&
                m.ValoracionMedia == 7 && // (8 + 6) / 2
                m.NumeroResenas == 2 &&
                m.AparicionesEnListas == 2)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public void ObtenerPeliculasMasPopulares_DeberiaRetornarPeliculasOrdenadas()
        {
            // Arrange
            var peliculas = new List<Pelicula>
            {
                new Pelicula { Id = 1, Titulo = "Movie 1" },
                new Pelicula { Id = 2, Titulo = "Movie 2" },
                new Pelicula { Id = 3, Titulo = "Movie 3" }
            };

            var metricas = new List<Metrica>
            {
                new Metrica { Id = 1, Pelicula = peliculas[0], Popularidad = 0.8 },
                new Metrica { Id = 2, Pelicula = peliculas[1], Popularidad = 0.9 },
                new Metrica { Id = 3, Pelicula = peliculas[2], Popularidad = 0.7 }
            };

            _metricaRepositoryMock.Setup(r => r.ReadAll()).Returns(metricas);

            // Act
            var resultado = _manageMetricasCP.ObtenerPeliculasMasPopulares(2).ToList();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(peliculas[1], resultado[0]); // Movie 2 (0.9)
            Assert.Equal(peliculas[0], resultado[1]); // Movie 1 (0.8)
        }

        [Fact]
        public void ActualizarMetricasPelicula_CuandoPeliculaNoExiste_DeberiaLanzarExcepcion()
        {
            // Arrange
            long peliculaId = 999;
            _peliculaRepositoryMock.Setup(r => r.ReadById(peliculaId)).Returns((Pelicula)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _manageMetricasCP.ActualizarMetricasPelicula(peliculaId));
            Assert.Contains("no encontrada", ex.Message);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
        }
    }
}