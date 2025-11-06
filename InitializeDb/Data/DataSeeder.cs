using System;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Enums;

namespace InitializeDb.Data
{
    public class DataSeeder
    {
        private readonly UsuarioCEN _usuarioCEN;
        private readonly PeliculaCEN _peliculaCEN;
        private readonly ListaCEN _listaCEN;
        private readonly ResenyaCEN _resenyaCEN;

        public DataSeeder(
            UsuarioCEN usuarioCEN,
            PeliculaCEN peliculaCEN,
            ListaCEN listaCEN,
            ResenyaCEN resenyaCEN)
        {
            _usuarioCEN = usuarioCEN;
            _peliculaCEN = peliculaCEN;
            _listaCEN = listaCEN;
            _resenyaCEN = resenyaCEN;
        }

        public void SeedData()
        {
            // Crear algunos usuarios de ejemplo
            var usuario1 = new ApplicationCore.Domain.EN.Usuario
            {
                Nombre = "Usuario1",
                Email = "usuario1@example.com",
                Contrasena = "password123",
                FotoPerfil = null,
                Biografia = "Amante del cine",
                ModoBlancoYNegro = false
            };
            _usuarioCEN.Crear(usuario1);

            var usuario2 = new ApplicationCore.Domain.EN.Usuario
            {
                Nombre = "Usuario2",
                Email = "usuario2@example.com",
                Contrasena = "password456",
                FotoPerfil = null,
                Biografia = "Crítico de cine amateur",
                ModoBlancoYNegro = true
            };
            _usuarioCEN.Crear(usuario2);

            // Crear algunas películas de ejemplo
            var pelicula1 = new ApplicationCore.Domain.EN.Pelicula
            {
                Titulo = "El Padrino",
                TituloOriginal = "The Godfather",
                Anio = 1972,
                Duracion = 175,
                Pais = "Estados Unidos",
                Director = "Francis Ford Coppola",
                Genero = "Drama, Crimen",
                Sinopsis = "El patriarca de una dinastía del crimen organizado transfiere el control de su imperio clandestino a su reacio hijo.",
                ValoracionMedia = 9.2m
            };
            _peliculaCEN.Crear(pelicula1);

            var pelicula2 = new ApplicationCore.Domain.EN.Pelicula
            {
                Titulo = "Pulp Fiction",
                TituloOriginal = "Pulp Fiction",
                Anio = 1994,
                Duracion = 154,
                Pais = "Estados Unidos",
                Director = "Quentin Tarantino",
                Genero = "Crimen, Drama",
                Sinopsis = "Las vidas de dos asesinos a sueldo, un boxeador, la esposa de un gángster y dos bandidos se entrelazan en cuatro historias de violencia y redención.",
                ValoracionMedia = 8.9m
            };
            _peliculaCEN.Crear(pelicula2);

            // Crear algunas listas de ejemplo
            var listaId1 = _listaCEN.Crear("Favoritas", tipoLista.MeGusta);
            var listaId2 = _listaCEN.Crear("Pendientes", tipoLista.Pendiente);

            // Crear algunas reseñas de ejemplo
            var resenyaId1 = _resenyaCEN.Crear(5, "Una obra maestra del cine clásico.", DateTime.Now.AddDays(-5));
            var resenyaId2 = _resenyaCEN.Crear(4, "Narrativa innovadora y actuaciones brillantes.", DateTime.Now.AddDays(-2));
        }
    }
}