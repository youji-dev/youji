namespace PersistenceLayer.DataAccess.Entities
{
    public class TicketComment
    {
        public string Id { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
