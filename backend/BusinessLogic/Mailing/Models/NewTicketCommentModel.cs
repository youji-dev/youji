using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for new comment email
    /// </summary>
    public record NewTicketCommentModel : MailModel
    {
        /// <inheritdoc/>
        public override string TemplateName { get; } = "NewTicketComment";

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
                MailTitle = $"Neuer Kommentar an '{comment.Ticket.Title}'",
                Author = comment.Author,
                Content = comment.Content,
            };
        }
    }
}
