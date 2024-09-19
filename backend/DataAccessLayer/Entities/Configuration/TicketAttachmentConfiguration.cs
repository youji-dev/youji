using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PersistenceLayer.DataAccess.Entities.Configuration
{
    /// <summary>
    /// Entity configuration for <see cref="TicketAttachment"/>
    /// </summary>
    internal class TicketAttachmentConfiguration : IEntityTypeConfiguration<TicketAttachment>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<TicketAttachment> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
        }
    }
}
