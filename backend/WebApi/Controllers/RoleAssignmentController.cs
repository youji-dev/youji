using Application.WebApi.Decorators;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage role assignment requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeRoles(Roles.Admin)]
    public class RoleAssignmentController : ControllerBase
    {
        /// <summary>
        /// Gets all role assignments.
        /// </summary>
        /// <param name="roleAssignmentRepo">Instance of <see cref="RoleAssignmentRepository"/></param>
        /// <returns>An <see cref="ObjectResult"/> with all role assignments.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<RoleAssignment[]> Get(
            [FromServices] RoleAssignmentRepository roleAssignmentRepo)
        {
            return this.Ok(roleAssignmentRepo.GetAll().ToArray());
        }

        /// <summary>
        /// Adds a new role assignment entity.
        /// </summary>
        /// <param name="roleAssignmentRepo">Instance of <see cref="RoleAssignmentRepository"/></param>
        /// <param name="roleAssignment">The <see cref="RoleAssignment"/> that will be added.</param>
        /// <returns>An <see cref="ObjectResult"/> with the added role assignment entity.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<RoleAssignment>> Post(
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
        /// <param name="roleAssignmentId">Instance of <see cref="RoleAssignment"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated role assignment.</returns>
        [HttpDelete("{roleAssignmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] RoleAssignmentRepository roleAssignmentRepo,
            [FromRoute] string roleAssignmentId)
        {
            var deleteRoleAssignment = await roleAssignmentRepo.GetAsync(roleAssignmentId);

            if (deleteRoleAssignment is null)
                return this.NotFound($"A role assignment with de id '{roleAssignmentId}' doesn´t exist.");

            await roleAssignmentRepo.DeleteAsync(deleteRoleAssignment);

            return this.Ok($"The role assignment with the id '{roleAssignmentId}' was deleted.");
        }
    }
}
