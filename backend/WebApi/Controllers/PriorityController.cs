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
    public class PriorityController(PriorityRepository priorityRepo) : ControllerBase
    {
        /// <summary>
        /// Gets all priorities.
        /// </summary>
        /// <returns>An <see cref="ObjectResult"/> with all priorities.</returns>
        [HttpGet]
        public ActionResult<Priority[]> Get()
        {
            return this.Ok(priorityRepo.GetAllAsync(priority => priority.Count() > 0));
        }

        /// <summary>
        /// Adds a new priority entity.
        /// </summary>
        /// <param name="priority">Instance of <see cref="Priority"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added priority entity.</returns>
        [HttpPost]
        public async Task<ActionResult<Priority[]>> Post(Priority priority)
        {
            await priorityRepo.AddAsync(priority);
            return this.Ok(priority);
        }

        /// <summary>
        /// Updates the specific priority.
        /// </summary>
        /// <param name="priority">Instance of <see cref="Priority"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated priority.</returns>
        [HttpPut]
        public async Task<ActionResult<Priority[]>> Put(Priority priority)
        {
            await priorityRepo.UpdateAsync(priority);
            return this.Ok(priority);
        }

        /// <summary>
        /// Deletes the priority with the specific id.
        /// </summary>
        /// <param name="id">The specific id of the priority that will deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete]
        public async Task<ActionResult<Priority[]>> Delete(string id)
        {
            var deletePriority = await priorityRepo.GetAsync(new Guid(id));

            if (deletePriority is null)
            {
                return this.BadRequest($"Eine Priorität mit der ID '{id}' existiert nicht.");
            }

            await priorityRepo.DeleteAsync(deletePriority);

            return this.Ok($"Die Priorität mit der ID '{id}' wurde gelöscht.");
        }
    }
}
