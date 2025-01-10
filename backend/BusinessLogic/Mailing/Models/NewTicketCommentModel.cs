using I18N.DotNet;
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

        /// <summary>
        /// Creates a new instance of <see cref="NewTicketCommentModel"/> from a <see cref="TicketComment"/>
        /// </summary>
        /// <param name="comment">The comment</param>
        /// <param name="localizer">Localizer to use for mail generation</param>
        /// <returns>The generated model</returns>
        public static NewTicketCommentModel FromComment(TicketComment comment, Localizer localizer)
        {
            return new NewTicketCommentModel()
            {
                MailTitle = localizer.Localize($"New comment on ticket '{comment.Ticket?.Title ?? "unbekannt"}'"),
                Author = comment.Author,
                Content = comment.Content,
                Localizer = localizer,
            };
        }
    }
}
