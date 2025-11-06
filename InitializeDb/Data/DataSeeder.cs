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
            var usuario1Id = _usuarioCEN.Crear("Usuario1", "usuario1@example.com", "password123", null, "Amante del cine", false);

            var usuario2Id = _usuarioCEN.Crear("Usuario2", "usuario2@example.com", "password456", null, "Crítico de cine amateur", true);

            // Crear algunas películas de ejemplo
            var pelicula1Id = _peliculaCEN.Crear("El Padrino", "The Godfather", 1972, 175, "Estados Unidos", "Francis Ford Coppola", "Drama, Crimen", "El patriarca de una dinastía del crimen organizado transfiere el control de su imperio clandestino a su reacio hijo.", 9.2m);

            var pelicula2Id = _peliculaCEN.Crear("Pulp Fiction", "Pulp Fiction", 1994, 154, "Estados Unidos", "Quentin Tarantino", "Crimen, Drama", "Las vidas de dos asesinos a sueldo, un boxeador, la esposa de un gángster y dos bandidos se entrelazan en cuatro historias de violencia y redención.", 8.9m);

            // Crear algunas listas de ejemplo y añadir películas
            var listaId1 = _listaCEN.Crear("Favoritas", tipoLista.MeGusta);
            _listaCEN.AsignarCreador(listaId1, usuario1Id);
            _listaCEN.AgregarPelicula(listaId1, pelicula1Id);

            var listaId2 = _listaCEN.Crear("Pendientes", tipoLista.Pendiente);
            _listaCEN.AsignarCreador(listaId2, usuario2Id);
            _listaCEN.AgregarPelicula(listaId2, pelicula2Id);

            // Crear algunas reseñas de ejemplo
            var resenyaId1 = _resenyaCEN.Crear(5, "Una obra maestra del cine clásico.", DateTime.Now.AddDays(-5));
            _resenyaCEN.AsignarAutor(resenyaId1, usuario1Id);
            _resenyaCEN.AsignarPelicula(resenyaId1, pelicula1Id);

            var resenyaId2 = _resenyaCEN.Crear(4, "Narrativa innovadora y actuaciones brillantes.", DateTime.Now.AddDays(-2));
            _resenyaCEN.AsignarAutor(resenyaId2, usuario2Id);
            _resenyaCEN.AsignarPelicula(resenyaId2, pelicula2Id);
        }
    }
}