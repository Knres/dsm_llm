using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface INotificacionRepository
    {
        Notificacion GetById(long id);
        IEnumerable<Notificacion> GetAll();
        void New(Notificacion entity);
        void Modify(Notificacion entity);
        void Destroy(long id);
    }
}
