using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System.Collections.Generic;

namespace ApplicationCore.Domain.CEN
{
    public class ListaCEN
    {
        private readonly IListaRepository _repo;
        private readonly IUnitOfWork _uow;

        public ListaCEN(IListaRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Lista Create(string nombre, ApplicationCore.Domain.Enums.TipoLista tipo, long usuarioId)
        {
            var l = new Lista
            {
                Nombre = nombre,
                Tipo = tipo
            };
            _repo.New(l);
            _uow.SaveChanges();
            return l;
        }

        public void Modify(Lista lista)
        {
            _repo.Modify(lista);
            _uow.SaveChanges();
        }

        public void Destroy(Lista lista)
        {
            _repo.Destroy(lista);
            _uow.SaveChanges();
        }

        public IList<Lista> ReadAll() => _repo.DameTodos();
        public Lista? ReadById(long id) => _repo.DamePorOID(id);
        public IList<Lista> ReadFilter(string filtro) => _repo.ReadFilter(filtro);
    }
}
