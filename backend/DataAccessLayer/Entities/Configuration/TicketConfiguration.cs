using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PersistenceLayer.DataAccess.Entities.Configuration
{
    /// <summary>
    /// Entity configuration for <see cref="Ticket"/>
    /// </summary>
    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.CreationDate)
                .HasDefaultValueSql("select current_timestamp at time zone 'utc';");

            builder.Navigation(x => x.Building).AutoInclude();
            builder.Navigation(x => x.State).AutoInclude();
            builder.Navigation(x => x.Priority).AutoInclude();
            builder.Navigation(x => x.Attachments).AutoInclude();
            builder.Navigation(x => x.Comments).AutoInclude();
        }
    }
}
