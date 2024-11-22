using Application.WebApi.Decorators;
using Common.Contracts.Post;
using Common.Contracts.Put;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage priority requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController : ControllerBase
    {
        /// <summary>
        /// Gets all priorities.
        /// </summary>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/></param>
        /// <returns>An <see cref="ObjectResult"/> with all priorities.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public ActionResult<Priority[]> Get(
            [FromServices] PriorityRepository priorityRepo)
        {
            return this.Ok(priorityRepo.GetAll().ToArray());
        }

        /// <summary>
        /// Adds a new priority entity.
        /// </summary>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/></param>
        /// <param name="priorityData">The <see cref="Priority"/> that will be added.</param>
        /// <returns>An <see cref="ObjectResult"/> with the added priority entity.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<Priority>> Post(
            [FromServices] PriorityRepository priorityRepo,
            [FromBody] PriorityPostDTO priorityData)
        {
            var priority = new Priority()
            {
                Id = default,
                Name = priorityData.Name,
                Value = priorityData.Value,
            };

            await priorityRepo.AddAsync(priority);

            return this.Ok(priority);
        }

        /// <summary>
        /// Updates the specific priority.
        /// </summary>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/></param>
        /// <param name="priorityData">The <see cref="Priority"/> that will be updated.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated priority.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<Priority>> Put(
            [FromServices] PriorityRepository priorityRepo,
            [FromBody] Priority priorityData)
        {
            var priority = await priorityRepo.GetAsync(priorityData.Id);

            if (priority is null)
                return this.NotFound($"A priority with the id '{priorityData.Id}' doesn´t exist.");

            priority.Name = priorityData.Name;
            priority.Value = priorityData.Value;

            await priorityRepo.UpdateAsync(priority);

            return this.Ok(priority);
        }

        /// <summary>
        /// Deletes the priority with the specific id.
        /// </summary>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/></param>
        /// <param name="priorityId">The specific id of the priority that will be deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete("{priorityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] PriorityRepository priorityRepo,
            [FromRoute] Guid priorityId)
        {
            var deletePriority = await priorityRepo.GetAsync(priorityId);

            if (deletePriority is null)
                return this.NotFound($"A priority with the value {priorityId} doesn´t exist.");

            await priorityRepo.DeleteAsync(deletePriority);

            return this.Ok($"The priority with the value {priorityId} was deleted.");
        }
    }
}
