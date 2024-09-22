using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the comment at the ticket.
    /// </summary>
    public class TicketComment
    {
        /// <summary>
        /// The id of the comment.
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// The author of the comment.
        /// </summary>
        public required string Author { get; set; }

        /// <summary>
        /// The content of the comment.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// The date and time when the comment was created.
        /// </summary>
        public required DateTime CreationDate { get; set; }

        /// <summary>
        /// The id of the related ticket.
        /// </summary>
        public required Guid TicketId { get; set; }

        /// <summary>
        /// The refernce type of the relatet tickets.
        /// </summary>
        public Ticket? Ticket { get; set; }
    }
}
