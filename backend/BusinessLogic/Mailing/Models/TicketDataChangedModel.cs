using I18N.DotNet;
using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for ticket master-/meta-data changes e-mail
    /// </summary>
    public record TicketDataChangedModel : MailModel
    {
        /// <inheritdoc/>
        public override string TemplateName { get; } = "TicketDataChanged";

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
        /// Creates a new instance of <see cref="TicketDataChangedModel"/> from the differences of two versions of a ticket
        /// </summary>
        /// <param name="newTicket">New version of the ticket</param>
        /// <param name="oldTicket">Old version of the ticket</param>
        /// <param name="localizer">Localizer to use for mail generation</param>
        /// <returns>The generated model</returns>
        public static TicketDataChangedModel FromTickets(Ticket newTicket, Ticket oldTicket, Localizer localizer)
        {
            TicketDataChangedModel result = new()
            {
                MailTitle = localizer.Localize($"Ticket '{newTicket.Title}' was changed"),
                Localizer = localizer,
            };

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

            return result;
        }
    }

    /// <summary>
    /// Struct for simple string property change
    /// </summary>
    public struct SimpleChange
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
}