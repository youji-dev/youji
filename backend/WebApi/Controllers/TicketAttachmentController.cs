using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Repositories;
using PersistenceLayer.DataAccess.Entities;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage ticket attachments requests.
    /// </summary>
    /// <param name="attachmentRepo">Instance of <see cref="TicketAttachment"/></param>
    [Route("api/[controller]")]
    [ApiController]
    public class TicketAttachmentController(TicketAttachmentRepository attachmentRepo) : ControllerBase
    {
        /// <summary>
        /// Deletes the attachment with the specific id.
        /// </summary>
        /// <param name="id">The specific id of the attachment.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var attachment = await attachmentRepo.GetAsync(new Guid(id));

            if (attachment == null)
            {
                return this.NotFound();
            }

            await attachmentRepo.DeleteAsync(attachment);

            return this.Ok($"Der Anhang mit der ID '{id}' wurde gelöscht.");
        }
    }
}
