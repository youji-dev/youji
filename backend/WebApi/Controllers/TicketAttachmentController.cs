using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage ticket attachments requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TicketAttachmentController : ControllerBase
    {
        /// <summary>
        /// Deletes the attachment with the specific id.
        /// </summary>
        /// <param name="attachmentRepo">Instance of <see cref="TicketAttachmentRepository"/></param>
        /// <param name="attachmentId">The specific id of the attachment.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete("{attachmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] TicketAttachmentRepository attachmentRepo,
            [FromRoute] Guid attachmentId)
        {
            var attachment = await attachmentRepo.GetAsync(attachmentId);

            if (attachment is null)
                return this.NotFound($"An attachment with the id '{attachmentId}' doesn´t exist.");

            await attachmentRepo.DeleteAsync(attachment);

            return this.Ok($"The attachment with the id '{attachmentId}' was deleted.");
        }
    }
}
