using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    public class TicketAttachmentRepository : Repository<Ticket, Guid>
    {
        public TicketAttachmentRepository(DataContext context) 
            : base(context)
        {
        }
    }
}
