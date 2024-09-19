using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the priority entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class PriorityRepository(DataContext context) : Repository<Priority, int>(context)
    {
        /// <summary>
        /// Gets the priority entity with the specific value.
        /// </summary>
        /// <param name="value">The int value of the priority</param>
        /// <returns>The priority entity with the specific value</returns>
        public override Task<Priority?> GetAsync(int value)
        {
            return base.GetAsync(value);
        }
    }
}
