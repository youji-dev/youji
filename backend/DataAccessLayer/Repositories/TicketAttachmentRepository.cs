using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the ticket attachment entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class TicketAttachmentRepository(DataContext context) : Repository<TicketAttachment, Guid>(context)
    {
    }
}
