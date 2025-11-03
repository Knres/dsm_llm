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
        private readonly ResenaCEN _resenaCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IResenaRepository _resenaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificacionRepository _notificacionRepository;

        public ManagePeliculasCP(
            PeliculaCEN peliculaCEN,
            ResenaCEN resenaCEN,
            IUnitOfWork unitOfWork,
            IPeliculaRepository peliculaRepository,
            IResenaRepository resenaRepository,
            IUsuarioRepository usuarioRepository,
            INotificacionRepository notificacionRepository)
        {
            _peliculaCEN = peliculaCEN;
            _resenaCEN = resenaCEN;
            _unitOfWork = unitOfWork;
            _peliculaRepository = peliculaRepository;
            _resenaRepository = resenaRepository;
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

        public virtual void AgregarResenaYActualizarValoracion(long peliculaId, long usuarioId, decimal valoracion, string comentario)
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
                var resenaId = _resenaCEN.Crear(
                    valoracion,
                    comentario,
                    DateTime.Now
                );

                // Actualizar la valoración media de la película
                var todasLasResenas = _resenaRepository.ReadAll()
                    .Where(r => r.Id == peliculaId)
                    .ToList();

                if (todasLasResenas.Any())
                {
                    decimal nuevaValoracionMedia = todasLasResenas.Average(r => r.Valoracion);
                    pelicula.ValoracionMedia = nuevaValoracionMedia;
                    _peliculaRepository.Modify(pelicula);
                }

                // Crear notificación para el usuario que hizo la reseña
                var notificacion = new Notificacion
                {
                    Mensaje = $"Has publicado una reseña para {pelicula.Titulo}",
                    Fecha = DateTime.Now,
                    Tipo = Enums.tipoNotificacion.Otro,
                    IdOrigen = resenaId
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