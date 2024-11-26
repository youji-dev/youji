using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the entity of a ticket.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// The id of the ticket.
        /// </summary>
        [Key]
        public required Guid Id { get; set; }

        /// <summary>
        /// The title of the ticket:
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// The description of the ticket.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The priority of the ticket
        /// </summary>
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public required Priority Priority { get; set; }

        /// <summary>
        /// The author of the ticket.
        /// </summary>
        public required string Author { get; set; }

        /// <summary>
        /// The date when the ticket was created
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The state of the ticket.
        /// </summary>
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public required State State { get; set; }

        /// <summary>
        /// An array of all comments on the ticket.
        /// </summary>
        public Collection<TicketComment> Comments { get; set; } = [];

        /// <summary>
        /// An array of all attachements on the ticket.
        /// </summary>
        public Collection<TicketAttachment> Attachments { get; set; } = [];

        /// <summary>
        /// The building where the issue of the ticket was located.
        /// </summary>
        public Building? Building { get; set; }

        /// <summary>
        /// The room where the issue of the ticket was located.
        /// </summary>
        public string? Room { get; set; }

        /// <summary>
        /// The affected object.
        /// </summary>
        public string? Object { get; set; }

        /// <summary>
        /// To be used when comparing an unknown property of Ticket to a search term
        /// </summary>
        /// <param name="value" type="any">The value to compare</param>
        /// <param name="searchTerm" type="string">The search term</param>
        /// <param name="valueProperty" type="string" default="Id">The property name to be used if value is an object</param>
        /// <returns>Boolean</returns>
        public static bool CheckIfSearchMatches(object? value, string? searchTerm, string valueProperty = "Id")
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return true;
            }

            if (value == null)
            {
                return true;
            }
            else
            {
                if (value is string)
                {
                    if (value.ToString() != null)
                    {
                        if (searchTerm.ToLower().Contains(((string)value).ToLower()))
                        {
                            return true;
                        }
                    }
                }
                else if (value.GetType().GetProperty(valueProperty) != null)
                {
                    if (value.GetType().GetProperty(valueProperty)?.ToString() != null)
                    {
                        return value.GetType().GetProperty(valueProperty)?.ToString() == searchTerm;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}