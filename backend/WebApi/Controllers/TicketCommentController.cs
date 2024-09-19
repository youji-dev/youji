using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage ticket comments requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCommentController : ControllerBase
    {
        /// <summary>
        /// Deletes the comment with the specific id.
        /// </summary>
        /// <param name="commentRepo">Instance of <see cref="TicketCommentRepository"/></param>
        /// <param name="id">The specific id of the comment.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(
            [FromServices] TicketCommentRepository commentRepo,
            [FromBody] string id)
        {
            var comment = await commentRepo.GetAsync(new Guid(id));

            if (comment == null)
            {
                return this.NotFound();
            }

            await commentRepo.DeleteAsync(comment);

            return this.Ok($"Der Kommentar mit der ID '{id}' wurde gelöscht.");
        }
    }
}
