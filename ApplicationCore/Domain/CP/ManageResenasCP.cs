using System;
using System.Linq;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP
{
    public class ManageResenyasCP
    {
        private readonly ResenyaCEN _resenaCEN;
        private readonly PeliculaCEN _peliculaCEN;
        private readonly UsuarioCEN _usuarioCEN;
        private readonly NotificacionCEN _notificacionCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationCore.Domain.Repositories.IResenyaRepository _resenaRepository;
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificacionRepository _notificacionRepository;

        public ManageResenyasCP(
            ResenyaCEN resenaCEN,
            PeliculaCEN peliculaCEN,
            UsuarioCEN usuarioCEN,
            NotificacionCEN notificacionCEN,
            IUnitOfWork unitOfWork,
            ApplicationCore.Domain.Repositories.IResenyaRepository resenaRepository,
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

        public virtual long CrearResenyaYNotificar(long peliculaId, long autorId, long punctuation, string comentario)
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

                // Crear la resenya (nuevo nombre)
                var resenya = new EN.Resenya
                {
                    Punctuation = punctuation,
                    Comentario = comentario,
                    Fecha = DateTime.Now,
                    Autor = autor,
                    Pelicula = pelicula
                };

                _resenaRepository.New(resenya);

                // Actualizar la valoración media de la película
                var todasLasResenyas = _resenaRepository.ReadAll()
                    .Where(r => r.Pelicula.Id == peliculaId)
                    .ToList();

                if (todasLasResenyas.Any())
                {
                    decimal nuevaValoracionMedia = (decimal)todasLasResenyas.Average(r => r.Punctuation);
                    pelicula.ValoracionMedia = nuevaValoracionMedia;
                    _peliculaRepository.Modify(pelicula);
                }

                // Notificar a los seguidores del autor
                var notificacion = new Notificacion
                {
                    Mensaje = $"{autor.Nombre} ha publicado una reseña de {pelicula.Titulo}",
                    Fecha = DateTime.Now,
                    Tipo = tipoNotificacion.Otro,
                    IdOrigen = resenya.Id
                };
                _notificacionRepository.New(notificacion);

                _unitOfWork.Commit();
                return resenya.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual void ModificarResenya(long resenyaId, long punctuation, string comentario)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe la reseña
                var resenya = _resenaRepository.ReadById(resenyaId);
                if (resenya == null)
                    throw new Exception($"Reseña {resenyaId} no encontrada");

                // Actualizar la resenya
                resenya.Punctuation = punctuation;
                resenya.Comentario = comentario;
                _resenaRepository.Modify(resenya);

                // Actualizar la valoración media de la película
                var todasLasResenyas = _resenaRepository.ReadAll()
                    .Where(r => r.Pelicula.Id == resenya.Pelicula.Id)
                    .ToList();

                if (todasLasResenyas.Any())
                {
                    decimal nuevaValoracionMedia = (decimal)todasLasResenyas.Average(r => r.Punctuation);
                    resenya.Pelicula.ValoracionMedia = nuevaValoracionMedia;
                    _peliculaRepository.Modify(resenya.Pelicula);
                }

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual void EliminarResenya(long resenyaId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                // Verificar que existe la resenya
                var resenya = _resenaRepository.ReadById(resenyaId);
                if (resenya == null)
                    throw new Exception($"Reseña {resenyaId} no encontrada");

                // Eliminar la resenya
                _resenaRepository.Delete(resenya);

                // Actualizar la valoración media de la película
                var peliculaId = resenya.Pelicula.Id;
                var todasLasResenyas = _resenaRepository.ReadAll()
                    .Where(r => r.Pelicula.Id == peliculaId)
                    .ToList();

                if (todasLasResenyas.Any())
                {
                    decimal nuevaValoracionMedia = (decimal)todasLasResenyas.Average(r => r.Punctuation);
                    resenya.Pelicula.ValoracionMedia = nuevaValoracionMedia;
                    _peliculaRepository.Modify(resenya.Pelicula);
                }

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual decimal calcularValoracionMedia(long peliculaId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var todasLasResenyas = _resenaRepository.ReadAll()
                    .Where(r => r.Pelicula != null && r.Pelicula.Id == peliculaId)
                    .ToList();

                decimal nuevaValoracionMedia = 0m;
                if (todasLasResenyas.Any())
                {
                    nuevaValoracionMedia = Math.Round((decimal)todasLasResenyas.Average(r => r.Punctuation), 2);
                }

                var pelicula = _peliculaRepository.ReadById(peliculaId);
                if (pelicula == null)
                    throw new Exception($"Película {peliculaId} no encontrada");

                pelicula.ValoracionMedia = nuevaValoracionMedia;
                _peliculaRepository.Modify(pelicula);

                _unitOfWork.Commit();
                return nuevaValoracionMedia;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}