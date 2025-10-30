using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.EN;
using System;

namespace ApplicationCore.Domain.CP
{
    /// <summary>
    /// Caso de proceso de ejemplo para orquestar operaciones relacionadas con Pelicula.
    /// Usa PeliculaCEN y la IUnitOfWork para realizar una operación transaccional.
    /// </summary>
    public class PeliculaManagementCP
    {
        private readonly PeliculaCEN _peliculaCEN;
        private readonly IUnitOfWork _uow;

        public PeliculaManagementCP(PeliculaCEN peliculaCEN, IUnitOfWork uow)
        {
            _peliculaCEN = peliculaCEN;
            _uow = uow;
        }

        public Pelicula CreatePeliculaAndPublish(string titulo, string? tituloOriginal, long? ano, long? duracion, long administradorId)
        {
            // Crear la pelicula
            var pelicula = _peliculaCEN.Create(titulo, tituloOriginal, ano, duracion);

            // Lógica de publicación (ejemplo): asignar Administrador como autor/publicador.
            pelicula.Administrador = new ApplicationCore.Domain.EN.Administrador { Id = administradorId };

            // Persistir cambios y mantener la operación transaccional
            _peliculaCEN.Modify(pelicula);
            // Si se necesitara orquestar otros repos, se haría antes de SaveChanges.
            _uow.SaveChanges();

            return pelicula;
        }
    }
}
