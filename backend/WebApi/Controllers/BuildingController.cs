using Application.WebApi.Decorators;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage building requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        /// <summary>
        /// Gets all buildings.
        /// </summary>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/></param>
        /// <returns>An <see cref="ObjectResult"/> with all buildings.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public ActionResult<Building[]> Get(
            [FromServices] BuildingRepository buildingRepo)
        {
            return this.Ok(buildingRepo.GetAll().ToArray());
        }

        /// <summary>
        /// Adds a new building entity.
        /// </summary>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/></param>
        /// <param name="buildingName">A <see langword="string"/> with the new name of building</param>
        /// <returns>An <see cref="ObjectResult"/> with the added building entity.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<Building>> Post(
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] string buildingName)
        {
            var building = new Building()
            {
                Id = default,
                Name = buildingName,
            };

            await buildingRepo.AddAsync(building);

            return this.Ok(building);
        }

        /// <summary>
        /// Updates the specific building.
        /// </summary>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/></param>
        /// <param name="buildingData">The <see cref="Building"/> that will updated.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated building.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<Building>> Put(
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] Building buildingData)
        {
            var building = await buildingRepo.GetAsync(buildingData.Id);

            if (building is null)
                return this.NotFound($"A building with the id '{buildingData.Id}' doesn´t exist.");

            building.Name = buildingData.Name;

            await buildingRepo.UpdateAsync(building);
            return this.Ok(buildingData);
        }

        /// <summary>
        /// Deletes the building with the specific id.
        /// </summary>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/></param>
        /// <param name="buildingId">The specific id of the building that will be deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete("{buildingId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] BuildingRepository buildingRepo,
            [FromRoute] Guid buildingId)
        {
            var deleteBuilding = await buildingRepo.GetAsync(buildingId);

            if (deleteBuilding is null)
                return this.NotFound($"A building with the id '{buildingId}' doesn´t exist.");

            await buildingRepo.DeleteAsync(deleteBuilding);

            return this.Ok($"The building with the id '{buildingId}' was deleted.");
        }
    }
}
