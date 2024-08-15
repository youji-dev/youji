using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the building entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class BuildingRepository(DataContext context) : Repository<Building, Guid>(context)
    {
    }
}
