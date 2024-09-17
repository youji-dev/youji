using Microsoft.EntityFrameworkCore;
using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess
{
    /// <summary>
    /// Represents the context of database.
    /// </summary>
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        /// <summary>
        /// Database set of the buildings table.
        /// </summary>
        public DbSet<Building> Buildings { get; set; }

        /// <summary>
        /// Database set of the state table.
        /// </summary>
        public DbSet<State> States { get; set; }

        /// <summary>
        /// Database set of the ticket table.
        /// </summary>
        public DbSet<Ticket> Tickets { get; set; }

        /// <summary>
        /// Database set of the priority table.
        /// </summary>
        public DbSet<Priority> Priorities { get; set; }

        /// <summary>
        /// Database set of the role assignment table.
        /// </summary>
        public DbSet<RoleAssignment> RoleAssignments { get; set; }

        /// <summary>
        /// Database set of the attachment table.
        /// </summary>
        public DbSet<TicketAttachment> Attachments { get; set; }

        /// <summary>
        /// Database set of the comments table.
        /// </summary>
        public DbSet<TicketComment> Comments { get; set; }

        /// <summary>
        /// Database set of the refresh token table.
        /// </summary>
        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}
