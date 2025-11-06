using System;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;
using System.Linq;

namespace ApplicationCore.Domain.CP
{
    public class ManagePeliculasCP
    {
        private readonly PeliculaCEN _peliculaCEN;
    private readonly ResenyaCEN _resenaCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPeliculaRepository _peliculaRepository;
    private readonly ApplicationCore.Domain.Repositories.IResenyaRepository _resena_repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificacionRepository _notificacionRepository;

        public ManagePeliculasCP(
            PeliculaCEN peliculaCEN,
            ResenyaCEN resenaCEN,
            IUnitOfWork unitOfWork,
            IPeliculaRepository peliculaRepository,
            ApplicationCore.Domain.Repositories.IResenyaRepository resenaRepository,
            IUsuarioRepository usuarioRepository,
            INotificacionRepository notificacionRepository)
        {
            _peliculaCEN = peliculaCEN;
            _resenaCEN = resenaCEN;
            _unitOfWork = unitOfWork;
            _peliculaRepository = peliculaRepository;
            _resena_repository = resenaRepository;
            _usuarioRepository = usuarioRepository;
            _notificacionRepository = notificacionRepository;
        }

        public virtual long CrearPeliculaYNotificar(string titulo, string tituloOriginal, long anio, long duracion, 
            string pais, string director, string genero, string sinopsis)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Crear la película
                var pelicula = new Pelicula 
                {
                    Titulo = titulo,
                    TituloOriginal = tituloOriginal,
                    Anio = anio,
                    Duracion = duracion,
                    Pais = pais,
                    Director = director,
                    Genero = genero,
                    Sinopsis = sinopsis,
                    ValoracionMedia = null
                };
                _peliculaCEN.Crear(pelicula);
                var peliculaId = pelicula.Id;

                // Crear notificación para todos los usuarios
                var usuarios = _usuarioRepository.ReadAll();
                foreach (var usuario in usuarios)
                {
                    var notificacion = new Notificacion
                    {
                        Mensaje = $"Nueva película agregada: {titulo}",
                        Fecha = DateTime.Now,
                        Tipo = Enums.tipoNotificacion.Anuncio,
                        IdOrigen = peliculaId
                    };
                    _notificacionRepository.New(notificacion);
                }

                _unitOfWork.Commit();
                return peliculaId;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public virtual void AgregarResenyaYActualizarValoracion(long peliculaId, long usuarioId, long punctuation, string comentario)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe la película
                var pelicula = _peliculaRepository.ReadById(peliculaId);
                if (pelicula == null)
                    throw new Exception($"Película {peliculaId} no encontrada");

                // Verificar que existe el usuario
                var usuario = _usuarioRepository.ReadById(usuarioId);
                if (usuario == null)
                    throw new Exception($"Usuario {usuarioId} no encontrado");

                // Crear la reseña
                var resenyaId = _resenaCEN.Crear(
                    punctuation,
                    comentario,
                    DateTime.Now
                );

                // Actualizar la valoración media de la película
                var todasLasResenyas = _resena_repository.ReadAll()
                    .Where(r => r.Pelicula.Id == peliculaId)
                    .ToList();

                if (todasLasResenyas.Any())
                {
                    decimal nuevaValoracionMedia = (decimal)todasLasResenyas.Average(r => r.Punctuation);
                    pelicula.ValoracionMedia = nuevaValoracionMedia;
                    _peliculaRepository.Modify(pelicula);
                }

                // Crear notificación para el usuario que hizo la reseña
                var notificacion = new Notificacion
                {
                    Mensaje = $"Has publicado una reseña para {pelicula.Titulo}",
                    Fecha = DateTime.Now,
                    Tipo = Enums.tipoNotificacion.Otro,
                    IdOrigen = resenyaId
                };
                _notificacionRepository.New(notificacion);

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