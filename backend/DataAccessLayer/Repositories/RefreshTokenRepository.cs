using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the RefreshToken entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class RefreshTokenRepository(DataContext context) : Repository<RefreshToken, Guid>(context)
    {
    }
}