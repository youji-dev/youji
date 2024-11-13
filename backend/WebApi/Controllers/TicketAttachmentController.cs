using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage ticket attachments requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketAttachmentController : ControllerBase
    {
        /// <summary>
        /// Serves an attachment from a specific ID.
        /// </summary>
        /// <param name="attachmentRepo">Instance of <see cref="TicketAttachmentRepository"/>.</param>
        /// <param name="attachmentId">The specific ID of the attachment.</param>
        /// <returns>A <see cref="FileResult"/> with the attachment data.</returns>
        [HttpGet("{attachmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Serve(
            [FromServices] TicketAttachmentRepository attachmentRepo,
            [FromRoute] Guid attachmentId)
        {
            var attachment = await attachmentRepo
                .Find(x => x.Id == attachmentId)
                .FirstOrDefaultAsync();

            if (attachment is null)
                return this.NotFound($"An attachment with the id '{attachmentId}' doesn´t exist.");

            var mimetype = MimeTypes.GetMimeType($".{attachment.FileType.ToLowerInvariant()}");

            return this.File(attachment.Binary, mimetype, attachment.Name);
        }


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
            var userClaim = this.User.FindFirst("username")?.Value;

            var attachment = await attachmentRepo
                .Find(x => x.Id == attachmentId)
                .Include(x => x.Ticket)
                .FirstOrDefaultAsync();

            if (attachment is null)
                return this.NotFound($"An attachment with the id '{attachmentId}' doesn´t exist.");

            if (attachment.Ticket == null || !attachment.Ticket.Author.Equals(userClaim))
                return this.Forbid();

            await attachmentRepo.DeleteAsync(attachment);

            return this.Ok($"The attachment with the id '{attachmentId}' was deleted.");
        }
    }
}
