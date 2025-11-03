using System;
using System.Linq;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Enums;

namespace ApplicationCore.Domain.CP
{
    public class ManageListasCP
    {
        private readonly ListaCEN _listaCEN;
        private readonly UsuarioCEN _usuarioCEN;
        private readonly PeliculaCEN _peliculaCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IListaRepository _listaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPeliculaRepository _peliculaRepository;

        public ManageListasCP(
            ListaCEN listaCEN,
            UsuarioCEN usuarioCEN,
            PeliculaCEN peliculaCEN,
            IUnitOfWork unitOfWork,
            IListaRepository listaRepository,
            IUsuarioRepository usuarioRepository,
            IPeliculaRepository peliculaRepository)
        {
            _listaCEN = listaCEN;
            _usuarioCEN = usuarioCEN;
            _peliculaCEN = peliculaCEN;
            _unitOfWork = unitOfWork;
            _listaRepository = listaRepository;
            _usuarioRepository = usuarioRepository;
            _peliculaRepository = peliculaRepository;
        }

        public long CrearListaParaUsuario(string nombre, tipoLista tipo, long usuarioId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe el usuario
                var usuario = _usuarioRepository.ReadById(usuarioId);
                if (usuario == null)
                    throw new Exception($"Usuario {usuarioId} no encontrado");

                // Crear la lista
                var lista = new Lista
                {
                    Nombre = nombre,
                    Tipo = tipo,
                    Creador = usuario
                };

                _listaRepository.New(lista);

                _unitOfWork.Commit();
                return lista.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void AgregarPeliculaALista(long listaId, long peliculaId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe la lista
                var lista = _listaRepository.ReadById(listaId);
                if (lista == null)
                    throw new Exception($"Lista {listaId} no encontrada");

                // Verificar que existe la película
                var pelicula = _peliculaRepository.ReadById(peliculaId);
                if (pelicula == null)
                    throw new Exception($"Película {peliculaId} no encontrada");

                // Verificar que la película no está ya en la lista
                if (lista.Peliculas.Any(p => p.Id == peliculaId))
                    throw new Exception($"La película {peliculaId} ya está en la lista {listaId}");

                // Agregar la película a la lista
                lista.Peliculas.Add(pelicula);
                _listaRepository.Modify(lista);

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void QuitarPeliculaDeLista(long listaId, long peliculaId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar que existe la lista
                var lista = _listaRepository.ReadById(listaId);
                if (lista == null)
                    throw new Exception($"Lista {listaId} no encontrada");

                // Buscar la película en la lista
                var pelicula = lista.Peliculas.FirstOrDefault(p => p.Id == peliculaId);
                if (pelicula == null)
                    throw new Exception($"Película {peliculaId} no encontrada en la lista {listaId}");

                // Quitar la película de la lista
                lista.Peliculas.Remove(pelicula);
                _listaRepository.Modify(lista);

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