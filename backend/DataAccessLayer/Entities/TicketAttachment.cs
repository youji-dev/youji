namespace PersistenceLayer.DataAccess.Entities
{
    public class TicketAttachment
    {
        public string Id { get; set; }

        public byte[] Binary { get; set; }

        public string Name { get; set; }

        public string FileType { get; set; }
    }
}
