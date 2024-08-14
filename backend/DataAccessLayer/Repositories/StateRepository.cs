using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    public class StateRepository : Repository<Ticket, Guid>
    {
        public StateRepository(DataContext context)
            : base(context)
        {
        }
    }
}
