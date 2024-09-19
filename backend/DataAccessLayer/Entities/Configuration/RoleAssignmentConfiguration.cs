using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PersistenceLayer.DataAccess.Entities.Configuration
{
    /// <summary>
    /// Entity configuration for <see cref="RoleAssignment"/>
    /// </summary>
    internal class RoleAssignmentConfiguration : IEntityTypeConfiguration<RoleAssignment>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<RoleAssignment> builder)
        {
        }
    }
}
