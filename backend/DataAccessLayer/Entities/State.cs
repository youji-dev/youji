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
        public Guid? Id { get; set; }

        /// <summary>
        /// The name of the state.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The color of the state.
        /// </summary>
        public required string Color { get; set; }
    }
}
