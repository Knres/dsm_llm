using System;
using System.Linq;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP
{
    public class ManageResenasCP
    {
        private readonly ResenaCEN _resenaCEN;
        private readonly PeliculaCEN _peliculaCEN;
        private readonly UsuarioCEN _usuarioCEN;
        private readonly NotificacionCEN _notificacionCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResenaRepository _resenaRepository;
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificacionRepository _notificacionRepository;

        public ManageResenasCP(
            ResenaCEN resenaCEN,
            PeliculaCEN peliculaCEN,
            UsuarioCEN usuarioCEN,
            NotificacionCEN notificacionCEN,
            IUnitOfWork unitOfWork,
            IResenaRepository resenaRepository,
            IPeliculaRepository peliculaRepository,
            IUsuarioRepository usuarioRepository,
            INotificacionRepository notificacionRepository)
        {
            _resenaCEN = resenaCEN;
            _peliculaCEN = peliculaCEN;
            _usuarioCEN = usuarioCEN;
            _notificacionCEN = notificacionCEN;
            _unitOfWork = unitOfWork;
            _resenaRepository = resenaRepository;
            _peliculaRepository = peliculaRepository;
            _usuarioRepository = usuarioRepository;
            _notificacionRepository = notificacionRepository;
        }

        public virtual long CrearResenaYNotificar(long peliculaId, long autorId, decimal valoracion, string comentario)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe la película
                var pelicula = _peliculaRepository.ReadById(peliculaId);
                if (pelicula == null)
                    throw new Exception($"Película {peliculaId} no encontrada");

                // Verificar que existe el autor
                var autor = _usuarioRepository.ReadById(autorId);
                if (autor == null)
                    throw new Exception($"Usuario {autorId} no encontrado");

                // Crear la reseña
                var resena = new Resena
                {
                    Valoracion = valoracion,
                    Comentario = comentario,
                    FechaPublicacion = DateTime.Now,
                    Autor = autor,
                    Pelicula = pelicula
                };

                _resenaRepository.New(resena);

                // Actualizar la valoración media de la película
                var todasLasResenas = _resenaRepository.ReadAll()
                    .Where(r => r.Pelicula.Id == peliculaId)
                    .ToList();

                if (todasLasResenas.Any())
                {
                    decimal nuevaValoracionMedia = todasLasResenas.Average(r => r.Valoracion);
                    pelicula.ValoracionMedia = nuevaValoracionMedia;
                    _peliculaRepository.Modify(pelicula);
                }

                // Notificar a los seguidores del autor
                var notificacion = new Notificacion
                {
                    Mensaje = $"{autor.Nombre} ha publicado una reseña de {pelicula.Titulo}",
                    Fecha = DateTime.Now,
                    Tipo = tipoNotificacion.Otro,
                    IdOrigen = resena.Id
                };
                _notificacionRepository.New(notificacion);

                _unitOfWork.Commit();
                return resena.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual void ModificarResena(long resenaId, decimal valoracion, string comentario)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe la reseña
                var resena = _resenaRepository.ReadById(resenaId);
                if (resena == null)
                    throw new Exception($"Reseña {resenaId} no encontrada");

                // Actualizar la reseña
                resena.Valoracion = valoracion;
                resena.Comentario = comentario;
                _resenaRepository.Modify(resena);

                // Actualizar la valoración media de la película
                var todasLasResenas = _resenaRepository.ReadAll()
                    .Where(r => r.Pelicula.Id == resena.Pelicula.Id)
                    .ToList();

                if (todasLasResenas.Any())
                {
                    decimal nuevaValoracionMedia = todasLasResenas.Average(r => r.Valoracion);
                    resena.Pelicula.ValoracionMedia = nuevaValoracionMedia;
                    _peliculaRepository.Modify(resena.Pelicula);
                }

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual void EliminarResena(long resenaId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe la reseña
                var resena = _resenaRepository.ReadById(resenaId);
                if (resena == null)
                    throw new Exception($"Reseña {resenaId} no encontrada");

                // Eliminar la reseña
                _resenaRepository.Delete(resena);

                // Actualizar la valoración media de la película
                var peliculaId = resena.Pelicula.Id;
                var todasLasResenas = _resenaRepository.ReadAll()
                    .Where(r => r.Pelicula.Id == peliculaId)
                    .ToList();

                if (todasLasResenas.Any())
                {
                    decimal nuevaValoracionMedia = todasLasResenas.Average(r => r.Valoracion);
                    resena.Pelicula.ValoracionMedia = nuevaValoracionMedia;
                    _peliculaRepository.Modify(resena.Pelicula);
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