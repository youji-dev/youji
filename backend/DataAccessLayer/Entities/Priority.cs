namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Entitiy of the priority.
    /// </summary>
    public class Priority
    {
        /// <summary>
        /// Name of the priority.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Value of the priority.
        /// </summary>
        public required int Value { get; set; }
    }
}
