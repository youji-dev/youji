using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for ticket change e-mail
    /// </summary>
    public record TicketChangedModel
    {
        public required string TicketTitle { get; set; }

        public SimpleChange? TitleChange { get; set; }

        public SimpleChange? DescriptionChange { get; set; }

        public SimpleChange? StateChange { get; set; }

        public SimpleChange? PriorityChange { get; set; }

        public SimpleChange? BuildingChange { get; set; }

        public SimpleChange? RoomChange { get; set; }

        public SimpleChange? ObjectChange { get; set; }

        public IEnumerable<string> NewAttachments { get; set; } = [];

        public IEnumerable<CommentModel> NewComments { get; set; } = [];

        public string LogoSrc { get; set; } = string.Empty;

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

    public struct SimpleChange
    {
        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }

    public struct CommentModel
    {
        public string Author { get; set; }

        public string Content { get; set; }
    }
}
