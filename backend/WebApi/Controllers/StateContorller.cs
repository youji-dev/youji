using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage state requests.
    /// </summary>
    /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
    [Route("api/[controller]")]
    [ApiController]
    public class StateContorller : ControllerBase
    {
        /// <summary>
        /// Gets all states.
        /// </summary>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
        /// <returns>An <see cref="ObjectResult"/> with all states.</returns>
        [HttpGet]
        public ActionResult<State[]> Get(
            [FromServices] StateRepository stateRepo)
        {
            return this.Ok(stateRepo.GetAllAsync(state => state != null));
        }

        /// <summary>
        /// Adds a new state entity.
        /// </summary>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
        /// <param name="state">Instance of <see cref="State"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added state entity.</returns>
        [HttpPost]
        public async Task<ActionResult<State[]>> Post(
            [FromServices] StateRepository stateRepo,
            [FromBody] State state)
        {
            await stateRepo.AddAsync(state);
            return this.Ok(state);
        }

        /// <summary>
        /// Updates the specific state.
        /// </summary>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
        /// <param name="state">Instance of <see cref="State"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated state.</returns>
        [HttpPut]
        public async Task<ActionResult<State[]>> Put(
            [FromServices] StateRepository stateRepo,
            [FromBody] State state)
        {
            await stateRepo.UpdateAsync(state);
            return this.Ok(state);
        }

        /// <summary>
        /// Deletes the state with the specific id.
        /// </summary>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
        /// <param name="id">The specific id of the state that will deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete]
        public async Task<ActionResult<State[]>> Delete(
            [FromServices] StateRepository stateRepo,
            [FromBody] string id)
        {
            var deleteState = await stateRepo.GetAsync(new Guid(id));

            if (deleteState is null)
            {
                return this.BadRequest($"Ein State mit der ID '{id}' existiert nicht.");
            }

            await stateRepo.DeleteAsync(deleteState);

            return this.Ok($"Der State mit der ID '{id}' wurde gelöscht.");
        }
    }
}
