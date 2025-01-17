using Application.WebApi.Decorators;
using Common.Contracts.Patch;
using Common.Enums;
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
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Gets all role assignments.
        /// </summary>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <returns>An <see cref="ObjectResult"/> with all users.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<User[]> Get(
            [FromServices] UserRepository userRepository)
        {
            return this.Ok(userRepository.GetAll().ToArray());
        }

        /// <summary>
        /// Adds a new role assignment entity.
        /// </summary>
        /// <param name="roleAssignmentRepo">Instance of <see cref="UserRepository"/></param>
        /// <param name="roleAssignment">The <see cref="User"/> that should be added.</param>
        /// <returns>An <see cref="ObjectResult"/> with the new user entity.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<User>> Post(
            [FromServices] UserRepository roleAssignmentRepo,
            [FromBody] User roleAssignment)
        {
            await roleAssignmentRepo.AddAsync(roleAssignment);
            return this.Ok(roleAssignment);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <param name="userId">The id of the user to update</param>
        /// <param name="userUpdate">An update </param>
        /// <returns>The updated user</returns>
        [HttpPatch("{userId}")]
        public async Task<ActionResult<User>> Patch(
            [FromServices] UserRepository userRepository,
            [FromRoute] string userId,
            [FromBody] UserPatch userUpdate)
        {
            User? user = await userRepository.GetAsync(userId);
            if (user is null)
                return this.NotFound($"No user found for id '{userId}'");

            if (userUpdate.NewRole is not null)
                user.Type = (Roles)userUpdate.NewRole;

            if (userUpdate.NewPreferredEmailLcid is not null)
                user.PreferredEmailLcid = userUpdate.NewPreferredEmailLcid;

            if (userUpdate.NewAreEmailNotificationsAllowed is not null)
                user.AreEmailNotificationsAllowed = (bool)userUpdate.NewAreEmailNotificationsAllowed;

            await userRepository.UpdateAsync(user);

            return this.Ok(user);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <param name="userId">The id of the user.</param>
        /// <returns>The result of the action with descriptive text.</returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] UserRepository userRepository,
            [FromRoute] string userId)
        {
            var deleteRoleAssignment = await userRepository.GetAsync(userId);

            if (deleteRoleAssignment is null)
                return this.NotFound($"A role assignment with de id '{userId}' doesn´t exist.");

            await userRepository.DeleteAsync(deleteRoleAssignment);

            return this.Ok($"The role assignment with the id '{userId}' was deleted.");
        }
    }
}
