﻿namespace Common.Contracts
{
    public class TicketDTO
    {
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
        public required int PriorityValue { get; set; }

        /// <summary>
        /// The author of the ticket.
        /// </summary>
        public required string Author { get; set; }

        /// <summary>
        /// The date when the ticket was created
        /// </summary>
        public required DateTime CreationDate { get; set; }

        /// <summary>
        /// The state of the ticket.
        /// </summary>
        public required Guid StateId { get; set; }

        /// <summary>
        /// The building where the issue of the ticket was located.
        /// </summary>
        public Guid BuildingId { get; set; }

        /// <summary>
        /// The room where the issue of the ticket was located.
        /// </summary>
        public string? Room { get; set; }

        /// <summary>
        /// The affected object.
        /// </summary>
        public string? Object { get; set; }
    }
}
