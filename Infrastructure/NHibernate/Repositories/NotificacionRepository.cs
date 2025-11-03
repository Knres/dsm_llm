using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using NHibernate;
using System.Collections.Generic;

namespace Infrastructure.NHibernate.Repositories
{
    public class NotificacionRepository : BaseRepository, INotificacionRepository
    {
        public NotificacionRepository(ISession session) : base(session)
        {
        }

        public void Delete(Notificacion notificacion)
        {
            _session.Delete(notificacion);
        }

        public void New(Notificacion notificacion)
        {
            _session.Save(notificacion);
        }

        public IList<Notificacion> ReadAll()
        {
            return _session.Query<Notificacion>().ToList();
        }

        public Notificacion ReadById(long id)
        {
            return _session.Get<Notificacion>(id);
        }

        public void Modify(Notificacion notificacion)
        {
            _session.Update(notificacion);
        }
    }
}