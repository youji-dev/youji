using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage priority requests.
    /// </summary>
    /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/></param>
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
        public ActionResult<Priority[]> Get(
            [FromServices] PriorityRepository priorityRepo)
        {
            return this.Ok(priorityRepo.GetAll().ToArray());
        }

        /// <summary>
        /// Adds a new priority entity.
        /// </summary>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/></param>
        /// <param name="priority">Instance of <see cref="Priority"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added priority entity.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Priority>> Post(
            [FromServices] PriorityRepository priorityRepo,
            [FromBody] Priority priority)
        {
            await priorityRepo.AddAsync(priority);
            return this.Ok(priority);
        }

        /// <summary>
        /// Updates the specific priority.
        /// </summary>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/></param>
        /// <param name="priority">Instance of <see cref="Priority"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated priority.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Priority>> Put(
            [FromServices] PriorityRepository priorityRepo,
            [FromBody] Priority priority)
        {
            await priorityRepo.UpdateAsync(priority);
            return this.Ok(priority);
        }

        /// <summary>
        /// Deletes the priority with the specific id.
        /// </summary>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/></param>
        /// <param name="value">The specific id of the priority that will deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] PriorityRepository priorityRepo,
            [FromBody] int value)
        {
            var deletePriority = await priorityRepo.GetAsync(value);

            if (deletePriority is null)
                return this.NotFound($"A priority with the value {value} doesn´t exist.");

            await priorityRepo.DeleteAsync(deletePriority);

            return this.Ok($"The priority with the value {value} was deleted.");
        }
    }
}
