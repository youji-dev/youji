using System.ComponentModel.DataAnnotations;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the ticket priority.
    /// </summary>
    public class Priority
    {
        /// <summary>
        /// The id of the priority.
        /// </summary>
        [Key]
        public required Guid Id { get; set; }

        /// <summary>
        /// The value of the priority.
        /// </summary>
        public required int Value { get; set; }

        /// <summary>
        /// The name of the priority.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The color of the priority.
        /// </summary>
        public required string Color { get; set; }
    }
}
