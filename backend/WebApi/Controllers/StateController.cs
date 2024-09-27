using Application.WebApi.Decorators;
using Common.Contracts.Post;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage state requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        /// <summary>
        /// Gets all states.
        /// </summary>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
        /// <returns>An <see cref="ObjectResult"/> with all states.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult<State[]> Get(
            [FromServices] StateRepository stateRepo)
        {
            return this.Ok(stateRepo.GetAll().ToArray());
        }

        /// <summary>
        /// Adds a new state entity.
        /// </summary>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
        /// <param name="stateData">The state data that will be Sadded.</param>
        /// <returns>An <see cref="ObjectResult"/> with the added state entity.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<State>> Post(
            [FromServices] StateRepository stateRepo,
            [FromBody] StatePostDTO stateData)
        {
            var state = new State()
            {
                Id = default,
                Name = stateData.Name,
                Color = stateData.Color,
            };

            await stateRepo.AddAsync(state);

            return this.Ok(state);
        }

        /// <summary>
        /// Updates the specific state.
        /// </summary>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
        /// <param name="state">The <see cref="State"/> that will be updated.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated state.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<State>> Put(
            [FromServices] StateRepository stateRepo,
            [FromBody] State state)
        {
            if (state.Id == default)
                return this.BadRequest("The state id is missing");

            if (await stateRepo.GetAsync(state.Id) is null)
                return this.NotFound($"A state with the id '{state.Id}' doesn´t exist.");

            await stateRepo.UpdateAsync(state);
            return this.Ok(state);
        }

        /// <summary>
        /// Deletes the state with the specific id.
        /// </summary>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/></param>
        /// <param name="stateId">The specific id of the state that will be deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete("{stateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] StateRepository stateRepo,
            [FromRoute] Guid stateId)
        {
            var deleteState = await stateRepo.GetAsync(stateId);

            if (deleteState is null)
                return this.NotFound($"A state with the id '{stateId}' doesn´t exist.");

            await stateRepo.DeleteAsync(deleteState);

            return this.Ok($"The state with the id '{stateId}' was deleted.");
        }
    }
}
