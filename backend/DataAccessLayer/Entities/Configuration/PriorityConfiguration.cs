using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PersistenceLayer.DataAccess.Entities.Configuration
{
    /// <summary>
    /// Entity configuration for <see cref="Priority"/>
    /// </summary>
    internal class PriorityConfiguration : IEntityTypeConfiguration<Priority>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Priority> builder)
        {
        }
    }
}
