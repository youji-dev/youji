using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PersistenceLayer.DataAccess.Entities.Configuration
{
    /// <summary>
    /// Entity configuration for <see cref="User"/>
    /// </summary>
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<User> builder)
        {
        }
    }
}
