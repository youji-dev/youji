﻿namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the comment at the ticket.
    /// </summary>
    public class TicketComment
    {
        /// <summary>
        /// The id of the comment.
        /// </summary>
        public required string Id { get; set; }

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
    }
}
