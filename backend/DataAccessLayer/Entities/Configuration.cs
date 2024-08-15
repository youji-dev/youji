namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the configuration
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The key of the configuration entity.
        /// </summary>
        public required string Key { get; set; }

        /// <summary>
        /// The value of the configuration entity.
        /// </summary>
        public required string Value { get; set; }
    }
}
