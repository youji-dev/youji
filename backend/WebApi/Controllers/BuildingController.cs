using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage building requests.
    /// </summary>
    /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/></param>
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
        public async Task<ActionResult<Building>> Post(
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] string buildingName)
        {
            var building = new Building()
            {
                Name = buildingName,
            };

            await buildingRepo.AddAsync(building);

            return this.Ok(building);
        }

        /// <summary>
        /// Updates the specific building.
        /// </summary>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/></param>
        /// <param name="building">Instance of <see cref="Building"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated building.</returns>
        [HttpPut]
        public async Task<ActionResult<Building>> Put(
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] Building building)
        {
            await buildingRepo.UpdateAsync(building);
            return this.Ok(building);
        }

        /// <summary>
        /// Deletes the building with the specific id.
        /// </summary>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/></param>
        /// <param name="buildingId">The specific id of the building that will deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete]
        public async Task<ActionResult<string>> Delete(
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] string buildingId)
        {
            var deleteBuilding = await buildingRepo.GetAsync(new Guid(buildingId));

            if (deleteBuilding is null)
                return this.NotFound($"Eine Priorität mit der ID '{buildingId}' existiert nicht.");

            await buildingRepo.DeleteAsync(deleteBuilding);

            return this.Ok($"Die Priorität mit der ID '{buildingId}' wurde gelöscht.");
        }
    }
}
