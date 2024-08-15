using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the ticket comment entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class TicketCommentRepository(DataContext context) : Repository<TicketComment, Guid>(context)
    {
    }
}
