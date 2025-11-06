using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using Moq;
using Xunit;

namespace Tests
{
    public class PeliculaCENTests
    {
        private readonly Mock<IPeliculaRepository> _peliculaRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly PeliculaCEN _peliculaCEN;

        public PeliculaCENTests()
        {
            _peliculaRepositoryMock = new Mock<IPeliculaRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _peliculaCEN = new PeliculaCEN(_peliculaRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public void ReadFilterGenero_DeberiaFiltrarPorGenero()
        {
            // Arrange
            var peliculas = new List<Pelicula>
            {
                new Pelicula { Id = 1, Titulo = "El Padrino", Genero = "Drama, Crimen" },
                new Pelicula { Id = 2, Titulo = "Matrix", Genero = "Ciencia ficción, Acción" },
                new Pelicula { Id = 3, Titulo = "Casablanca", Genero = "Drama, Romance" },
                new Pelicula { Id = 4, Titulo = "Terminator", Genero = "Acción, Ciencia ficción" }
            };

            _peliculaRepositoryMock.Setup(r => r.ReadByFilter("drama"))
                .Returns(peliculas);

            // Act
            var resultado = _peliculaCEN.ReadFilterGenero("drama");

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, p => p.Titulo == "El Padrino");
            Assert.Contains(resultado, p => p.Titulo == "Casablanca");
            Assert.DoesNotContain(resultado, p => p.Titulo == "Matrix");
        }

        [Fact]
        public void ReadFilterGenero_ConGeneroVacio_DeberiaRetornarListaVacia()
        {
            // Act
            var resultado = _peliculaCEN.ReadFilterGenero("");

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public void ReadFilterGenero_ConGeneroNulo_DeberiaRetornarListaVacia()
        {
            // Act
            var resultado = _peliculaCEN.ReadFilterGenero(null);

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public void ReadFilterGenero_DeberiaSercaseInsensitive()
        {
            // Arrange
            var peliculas = new List<Pelicula>
            {
                new Pelicula { Id = 1, Titulo = "El Padrino", Genero = "Drama, Crimen" },
                new Pelicula { Id = 2, Titulo = "Casablanca", Genero = "DRAMA, Romance" }
            };

            _peliculaRepositoryMock.Setup(r => r.ReadByFilter("drama"))
                .Returns(peliculas);

            // Act
            var resultado = _peliculaCEN.ReadFilterGenero("drama");

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, p => p.Genero.Contains("Drama"));
            Assert.Contains(resultado, p => p.Genero.Contains("DRAMA"));
        }

        [Fact]
        public void ReadFilterValoracion_DeberiaFiltrarPorRangoValoracion()
        {
            // Arrange
            var peliculas = new List<Pelicula>
            {
                new Pelicula { Id = 1, Titulo = "Película 1", ValoracionMedia = 8.5m },
                new Pelicula { Id = 2, Titulo = "Película 2", ValoracionMedia = 7.0m },
                new Pelicula { Id = 3, Titulo = "Película 3", ValoracionMedia = 9.0m },
                new Pelicula { Id = 4, Titulo = "Película 4", ValoracionMedia = 6.5m }
            };

            _peliculaRepositoryMock.Setup(r => r.ReadAll())
                .Returns(peliculas);

            // Act
            var resultado = _peliculaCEN.ReadFilterValoracion(7.0m, 8.5m);

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, p => p.ValoracionMedia == 8.5m);
            Assert.Contains(resultado, p => p.ValoracionMedia == 7.0m);
        }

        [Fact]
        public void ReadFilterValoracion_ConRangoInvalido_DeberiaRetornarListaVacia()
        {
            // Act & Assert
            Assert.Empty(_peliculaCEN.ReadFilterValoracion(10, 5));
            Assert.Empty(_peliculaCEN.ReadFilterValoracion(-1, 5));
            Assert.Empty(_peliculaCEN.ReadFilterValoracion(5, 11));
        }

        [Fact]
        public void ReadFilterAnyo_DeberiaFiltrarPorAnyo()
        {
            // Arrange
            var peliculas = new List<Pelicula>
            {
                new Pelicula { Id = 1, Titulo = "Película 1", Anio = 1994 },
                new Pelicula { Id = 2, Titulo = "Película 2", Anio = 2000 },
                new Pelicula { Id = 3, Titulo = "Película 3", Anio = 1994 },
            };

            _peliculaRepositoryMock.Setup(r => r.ReadAll())
                .Returns(peliculas);

            // Act
            var resultado = _peliculaCEN.ReadFilterAnyo(1994);

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, p => Assert.Equal(1994, p.Anio));
        }

        [Fact]
        public void ReadFilterAnyo_ConAnyoInvalido_DeberiaRetornarListaVacia()
        {
            // Act & Assert
            Assert.Empty(_peliculaCEN.ReadFilterAnyo(0));
            Assert.Empty(_peliculaCEN.ReadFilterAnyo(-1));
        }

        [Fact]
        public void ReadFilterTitulo_DeberiaFiltrarPorTituloYTituloOriginal()
        {
            // Arrange
            var peliculas = new List<Pelicula>
            {
                new Pelicula { Id = 1, Titulo = "El Padrino", TituloOriginal = "The Godfather" },
                new Pelicula { Id = 2, Titulo = "The Batman", TituloOriginal = "The Batman" },
                new Pelicula { Id = 3, Titulo = "El Caballero de la Noche", TituloOriginal = "The Dark Knight" }
            };

            _peliculaRepositoryMock.Setup(r => r.ReadByFilter("the"))
                .Returns(peliculas);

            // Act
            var resultado = _peliculaCEN.ReadFilterTitulo("the");

            // Assert
            Assert.Equal(3, resultado.Count);
            Assert.Contains(resultado, p => p.TituloOriginal == "The Godfather");
            Assert.Contains(resultado, p => p.Titulo == "The Batman");
            Assert.Contains(resultado, p => p.TituloOriginal == "The Dark Knight");
        }

        [Fact]
        public void ReadFilterTitulo_ConTituloVacio_DeberiaRetornarListaVacia()
        {
            // Act & Assert
            Assert.Empty(_peliculaCEN.ReadFilterTitulo(""));
            Assert.Empty(_peliculaCEN.ReadFilterTitulo(null));
            Assert.Empty(_peliculaCEN.ReadFilterTitulo("   "));
        }
    }
}