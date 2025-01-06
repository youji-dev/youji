using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the ticket entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class TicketRepository(DataContext context) : Repository<Ticket, Guid>(context)
    {
        /// <summary>
        /// Get the ids of all users involved in a ticket
        /// </summary>
        /// <param name="ticket">The ticket to get users from</param>
        /// <param name="exclude">A list of user ids to exclude</param>
        /// <returns>A list containing all user ids</returns>
        public IEnumerable<string> GetInvolvedUsersIds(Ticket ticket, IEnumerable<string>? exclude = null)
        {
            List<string> userIds = [ticket.Author];

            string[] commentAuthorIds = ticket.Comments.Select(c => c.Author).ToArray();
            userIds.AddRange(commentAuthorIds);

            exclude ??= [];
            userIds.RemoveAll(id => exclude.Contains(id));

            return userIds;
        }
    }
}
