﻿namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the assignment of the roles.
    /// </summary>
    public class RoleAssignment
    {
        /// <summary>
        /// The the id of the user.
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        /// The type of the user.
        /// </summary>
        public required int Type { get; set; }
    }
}
