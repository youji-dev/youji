using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the configuration entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class ConfigurationRepository(DataContext context) : Repository<Configuration, Guid>(context)
    {
    }
}
