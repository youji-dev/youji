﻿using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Repositories;
using PersistenceLayer.DataAccess.Entities;

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
        /// <param name="id">The specific id of the attachment.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(
            [FromServices] TicketAttachmentRepository attachmentRepo,
            [FromBody] string id)
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
