using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the role assignment entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class UserRepository(DataContext context) : Repository<User, string>(context)
    {
        /// <summary>
        /// Get all users from the list of <paramref name="ids"/>
        /// </summary>
        /// <param name="ids">The ids of the users to get</param>
        /// <returns>All found entities that match any of the ids</returns>
        public IQueryable<User> GetMany(IEnumerable<string> ids)
        {
            return this.GetAll().Where(u => ids.Contains(u.UserId));
        }
    }
}
