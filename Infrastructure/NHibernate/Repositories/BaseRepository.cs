using NHibernate;

namespace Infrastructure.NHibernate.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ISession _session;

        protected BaseRepository(ISession session)
        {
            _session = session;
        }
    }
}