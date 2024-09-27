using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for new comment email
    /// </summary>
    public record NewTicketCommentModel : MailModel
    {
        /// <summary>
        /// The comment author
        /// </summary>
        public required string Author { get; set; }

        /// <summary>
        /// The comments content
        /// </summary>
        public required string Content { get; set; }

        public static NewTicketCommentModel FromComment(TicketComment comment)
        {
            return new NewTicketCommentModel()
            {
#warning TODO: Platzhalter mit Ticket Titel ersetzten
                Title = $"Neuer Kommentar an {"N/A"}",
                Author = comment.Author,
                Content = comment.Content,
            };
        }
    }
}
