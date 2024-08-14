using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    public class TicketCommentRepository : Repository<TicketComment, Guid>
    {
        public TicketCommentRepository(DataContext context) 
            : base(context)
        {
        }
    }
}
