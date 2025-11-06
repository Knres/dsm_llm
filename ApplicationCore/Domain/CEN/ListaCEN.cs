using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Enums;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class ListaCEN
    {
        protected readonly IListaRepository _listaRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public ListaCEN(IListaRepository listaRepository, IUnitOfWork unitOfWork)
        {
            _listaRepository = listaRepository;
            _unitOfWork = unitOfWork;
        }

    public virtual long Crear(string nombre, tipoLista tipo)
        {
            var lista = new Lista
            {
                Nombre = nombre,
                Tipo = tipo
            };

            _listaRepository.New(lista);
            _unitOfWork.SaveChanges();
            return lista.Id;
        }

    public virtual void Modificar(long id, string nombre, tipoLista tipo)
        {
            var lista = _listaRepository.ReadById(id);
            if (lista == null)
                throw new Exception($"Lista {id} no encontrada");

            lista.Nombre = nombre;
            lista.Tipo = tipo;

            _listaRepository.Modify(lista);
            _unitOfWork.SaveChanges();
        }

        public virtual void AsignarCreador(long listaId, long creadorId)
        {
            var lista = _listaRepository.ReadById(listaId);
            if (lista == null)
                throw new Exception($"Lista {listaId} no encontrada");

            lista.Creador = new Usuario { Id = creadorId };
            _listaRepository.Modify(lista);
            _unitOfWork.SaveChanges();
        }

        public virtual void AgregarPelicula(long listaId, long peliculaId)
        {
            var lista = _listaRepository.ReadById(listaId);
            if (lista == null)
                throw new Exception($"Lista {listaId} no encontrada");

            if (lista.Peliculas == null)
                lista.Peliculas = new List<Pelicula>();

            lista.Peliculas.Add(new Pelicula { Id = peliculaId });
            _listaRepository.Modify(lista);
            _unitOfWork.SaveChanges();
        }

    public virtual void Eliminar(long id)
        {
            var lista = _listaRepository.ReadById(id);
            if (lista == null)
                throw new Exception($"Lista {id} no encontrada");

            _listaRepository.Delete(lista);
            _unitOfWork.SaveChanges();
        }

    public virtual Lista ObtenerPorId(long id)
        {
            return _listaRepository.ReadById(id);
        }

    public virtual IList<Lista> ObtenerTodas()
        {
            return _listaRepository.ReadAll();
        }
    }
}