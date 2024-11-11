using Application.WebApi.Decorators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
