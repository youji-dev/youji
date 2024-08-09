namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Entity of the ticket.
    /// </summary>
    public class Ticket
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Priority Priority { get; set; }

        public string Author { get; set; }

        public DateTime CreationDate { get; set; }

        public State State { get; set; }

        public TicketComment[] Comments { get; set; }

        public TicketAttachment[] Attachments { get; set; }

        public Building Building { get; set; }

        public string Room { get; set; }

        public string Object { get; set; }
    }
}
