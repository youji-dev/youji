namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represens the ticket priority.
    /// </summary>
    public class Priority
    {
        /// <summary>
        /// The name of the priority.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The value of the priority.
        /// </summary>
        public required int Value { get; set; }
    }
}
