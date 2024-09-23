using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage role assignment requests.
    /// </summary>
    /// <param name="roleAssignmentRepo">Instance of <see cref="RoleAssignmentRepository"/></param>
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAssignmentController : ControllerBase
    {
        /// <summary>
        /// Gets all role assignments.
        /// </summary>
        /// <param name="roleAssignmentRepo">Instance of <see cref="RoleAssignmentRepository"/></param>
        /// <returns>An <see cref="ObjectResult"/> with all role assignments.</returns>
        [HttpGet]
        public ActionResult<RoleAssignment[]> Get(
            [FromServices] RoleAssignmentRepository roleAssignmentRepo)
        {
            return this.Ok(roleAssignmentRepo.GetAllAsync(roleAssignment => roleAssignment != null));
        }

        /// <summary>
        /// Adds a new role assignment entity.
        /// </summary>
        /// <param name="roleAssignmentRepo">Instance of <see cref="RoleAssignmentRepository"/></param>
        /// <param name="roleAssignment">Instance of <see cref="RoleAssignment"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added role assignment entity.</returns>
        [HttpPost]
        public async Task<ActionResult<RoleAssignment[]>> Post(
            [FromServices] RoleAssignmentRepository roleAssignmentRepo,
            [FromBody] RoleAssignment roleAssignment)
        {
            await roleAssignmentRepo.AddAsync(roleAssignment);
            return this.Ok(roleAssignment);
        }

        /// <summary>
        /// Updates the specific role assignment.
        /// </summary>
        /// <param name="roleAssignmentRepo">Instance of <see cref="RoleAssignmentRepository"/></param>
        /// <param name="id">Instance of <see cref="RoleAssignment"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated role assignment.</returns>
        [HttpDelete]
        public async Task<ActionResult<RoleAssignment[]>> Delete(
            [FromServices] RoleAssignmentRepository roleAssignmentRepo,
            string id)
        {
            var deleteRoleAssignment = await roleAssignmentRepo.GetAsync(new Guid(id));

            if (deleteRoleAssignment is null)
            {
                return this.BadRequest($"Eine Priorität mit der ID '{id}' existiert nicht.");
            }

            await roleAssignmentRepo.DeleteAsync(deleteRoleAssignment);

            return this.Ok($"Die Priorität mit der ID '{id}' wurde gelöscht.");
        }
    }
}
