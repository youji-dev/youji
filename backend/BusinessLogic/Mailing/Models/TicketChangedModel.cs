using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for ticket change e-mail
    /// </summary>
    internal record TicketChangedModel : MailModel
    {
        /// <summary>
        /// The current title of the ticket
        /// </summary>
        public required string TicketTitle { get; set; }

        /// <summary>
        /// Change of the ticket title
        /// </summary>
        public SimpleChange? TitleChange { get; set; }

        /// <summary>
        /// Change of the description
        /// </summary>
        public SimpleChange? DescriptionChange { get; set; }

        /// <summary>
        /// Change of the state
        /// </summary>
        public SimpleChange? StateChange { get; set; }

        /// <summary>
        /// Change of the priority
        /// </summary>
        public SimpleChange? PriorityChange { get; set; }

        /// <summary>
        /// Change of the building
        /// </summary>
        public SimpleChange? BuildingChange { get; set; }

        /// <summary>
        /// Change of the room
        /// </summary>
        public SimpleChange? RoomChange { get; set; }

        /// <summary>
        /// Change of the affected object
        /// </summary>
        public SimpleChange? ObjectChange { get; set; }

        /// <summary>
        /// New attachments
        /// </summary>
        public IEnumerable<string> NewAttachments { get; set; } = [];

        /// <summary>
        /// New comments
        /// </summary>
        public IEnumerable<CommentModel> NewComments { get; set; } = [];

        /// <summary>
        /// Creates a new instance of <see cref="TicketChangedModel"/> from the differences of two versions of a ticket
        /// </summary>
        /// <param name="newTicket">New version of the ticket</param>
        /// <param name="oldTicket">Old version of the ticket</param>
        /// <returns>The generated model</returns>
        public static TicketChangedModel FromTickets(Ticket newTicket, Ticket oldTicket)
        {
            TicketChangedModel result = new() { TicketTitle = newTicket.Title };

            if (newTicket.Title != oldTicket.Title)
            {
                result.TitleChange = new SimpleChange
                {
                    NewValue = newTicket.Title,
                    OldValue = oldTicket.Title,
                };
            }

            if (newTicket.Description != oldTicket.Description)
            {
                result.DescriptionChange = new SimpleChange
                {
                    NewValue = newTicket.Description ?? "-",
                    OldValue = oldTicket.Description ?? "-",
                };
            }

            if (newTicket.Priority != oldTicket.Priority)
            {
                result.PriorityChange = new SimpleChange
                {
                    NewValue = newTicket.Priority?.Name ?? "-",
                    OldValue = oldTicket.Priority?.Name ?? "-",
                };
            }

            if (newTicket.State != oldTicket.State)
            {
                result.StateChange = new SimpleChange
                {
                    NewValue = newTicket.State.Name,
                    OldValue = oldTicket.State.Name,
                };
            }

            if (newTicket.Building != oldTicket.Building)
            {
                result.BuildingChange = new SimpleChange
                {
                    NewValue = newTicket.Building?.Name ?? "-",
                    OldValue = oldTicket.Building?.Name ?? "-",
                };
            }

            if (newTicket.Room != oldTicket.Room)
            {
                result.RoomChange = new SimpleChange
                {
                    NewValue = newTicket.Room ?? "-",
                    OldValue = oldTicket.Room ?? "-",
                };
            }

            if (newTicket.Object != oldTicket.Object)
            {
                result.ObjectChange = new SimpleChange
                {
                    NewValue = newTicket.Object ?? "-",
                    OldValue = oldTicket.Object ?? "-",
                };
            }

            if (newTicket.Comments.Count > oldTicket.Comments.Count)
            {
                var newComments = newTicket.Comments.Skip(oldTicket.Comments.Count);
                result.NewComments = newComments.Select(c => new CommentModel
                {
                    Author = c.Author,
                    Content = c.Content,
                });
            }

            if (newTicket.Attachments.Count != oldTicket.Attachments.Count)
            {
                var newAttachments = newTicket.Attachments.Skip(oldTicket.Attachments.Count);
                result.NewAttachments = newAttachments.Select(a => a.Name);
            }

            return result;
        }
    }

    /// <summary>
    /// Struct for simple string property change
    /// </summary>
    internal struct SimpleChange
    {
        /// <summary>
        /// New value of the property
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Old value of the property
        /// </summary>
        public string NewValue { get; set; }
    }

    /// <summary>
    /// Comment DTO for the <see cref="TicketChangedModel"/>
    /// </summary>
    internal struct CommentModel
    {
        /// <summary>
        /// The comment author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The comments content
        /// </summary>
        public string Content { get; set; }
    }
}
