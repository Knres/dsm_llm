using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    public class ListaRepository : BaseRepository, IListaRepository
    {
        public ListaRepository(ISession session) : base(session)
        {
        }

        public void Delete(Lista lista)
        {
            _session.Delete(lista);
        }

        public void Modify(Lista lista)
        {
            _session.Update(lista);
        }

        public void New(Lista lista)
        {
            _session.Save(lista);
        }

        public IList<Lista> ReadAll()
        {
            return _session.Query<Lista>().ToList();
        }

        public Lista ReadById(long id)
        {
            return _session.Get<Lista>(id);
        }
    }
}