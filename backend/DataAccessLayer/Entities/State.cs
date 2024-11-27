using System.ComponentModel.DataAnnotations;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the state of a ticket.
    /// </summary>
    public class State
    {
        /// <summary>
        /// The id of the state.
        /// </summary>
        [Key]
        public required Guid Id { get; set; }

        /// <summary>
        /// The name of the state.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The color of the state.
        /// </summary>
        public required string Color { get; set; }

        /// <summary>
        /// Marks the status so that tickets with it can be purged automatically.
        /// </summary>
        public required bool HasAutoPurge { get; set; }

        /// <summary>
        /// Days from when tickets will purged.
        /// </summary>
        public int? AutoPurgeDays { get; set; }
    }
}
