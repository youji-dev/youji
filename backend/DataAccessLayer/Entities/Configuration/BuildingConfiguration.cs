using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PersistenceLayer.DataAccess.Entities.Configuration
{
    /// <summary>
    /// Entity configuration for <see cref="Building"/>
    /// </summary>
    internal class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
        }
    }
}
