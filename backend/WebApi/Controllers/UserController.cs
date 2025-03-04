using System.Security.Claims;
using Application.WebApi.Decorators;
using Common.Contracts.Patch;
using Common.Enums;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage user entity requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Gets a collection of users. All users if the request is made by an admin, otherwise only the user itself.
        /// </summary>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <returns>An <see cref="ObjectResult"/> with all users.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<User[]> Get(
            [FromServices] UserRepository userRepository)
        {
            var userClaim = this.User.FindFirst("username")?.Value;
            var rolesClaim = this.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userClaim is null || rolesClaim is null)
                return this.Unauthorized();

            IQueryable<User> query = userRepository.GetAll();

            if (!Enum.TryParse(rolesClaim, out Roles role))
                return this.Unauthorized();

            if (!role.HasFlag(Roles.Admin))
            {
                query = query.Where(user => user.UserId == userClaim);
            }

            return this.Ok(query.ToArray());
        }

        /// <summary>
        /// Adds a new user entity
        /// </summary>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <param name="newUser">The <see cref="User"/> that should be added.</param>
        /// <returns>An <see cref="ObjectResult"/> with the new user entity.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<User>> Post(
            [FromServices] UserRepository userRepository,
            [FromBody] User newUser)
        {
            await userRepository.AddAsync(newUser);
            return this.Ok(newUser);
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
            var userClaim = this.User.FindFirst("username")?.Value;
            var rolesClaim = this.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userClaim is null || rolesClaim is null)
                return this.Unauthorized();

            if (!Enum.TryParse(rolesClaim, out Roles role))
                return this.Unauthorized();

            User? patchingUser = await userRepository.GetAsync(userId);
            if (patchingUser is null)
                return this.NotFound($"No user found for id '{userId}'");

            if (userUpdate.NewRole is not null)
                patchingUser.Type = (Roles)userUpdate.NewRole;

            if (!role.HasFlag(Roles.Admin))
            {
                User? requestingUser = await userRepository.GetAsync(userClaim);
                if (requestingUser is null)
                    return this.NotFound($"No user found for id '{userId}'");

                if (requestingUser.UserId != patchingUser.UserId)
                    return this.Forbid();

                if (userUpdate.NewRole is not null)
                    return this.Forbid();
            }

            if (userUpdate.NewPreferredEmailLcid is not null)
                patchingUser.PreferredEmailLcid = userUpdate.NewPreferredEmailLcid;

            if (userUpdate.NewAreEmailNotificationsAllowed is not null)
                patchingUser.AllowsEmailNotifications = (bool)userUpdate.NewAreEmailNotificationsAllowed;

            await userRepository.UpdateAsync(patchingUser);

            return this.Ok(patchingUser);
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
        [AuthorizeRoles(Roles.Admin)]
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
