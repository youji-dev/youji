using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PersistenceLayer.DataAccess.Entities.Configuration
{
    /// <summary>
    /// Entity configuration for <see cref="TicketComment"/>
    /// </summary>
    internal class TicketCommentConfiguration : IEntityTypeConfiguration<TicketComment>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<TicketComment> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
        }
    }
}
