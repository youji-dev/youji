namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the buildings.
    /// </summary>
    public class Building
    {
        /// <summary>
        /// Id of the building.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Name of the building.
        /// </summary>
        public required string Name { get; set; }
    }
}
