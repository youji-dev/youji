using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the priority entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class PriorityRepository(DataContext context) : Repository<Priority, Guid>(context)
    {
    }
}
