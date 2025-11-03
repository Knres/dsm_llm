using System.Collections.Generic;
using ApplicationCore.Domain.EN;

namespace ApplicationCore.Domain.Repositories
{
    public interface INotificacionRepository
    {
        Notificacion ReadById(long id);
        IList<Notificacion> ReadAll();
        void New(Notificacion entity);
        void Modify(Notificacion entity);
        void Delete(Notificacion entity);
    }
}
