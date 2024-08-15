using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the role assignment entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class RoleAssignmentRepository(DataContext context) : Repository<RoleAssignment, Guid>(context)
    {
    }
}
