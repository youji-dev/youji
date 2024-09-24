﻿using Microsoft.AspNetCore.Mvc;
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
        /// <param name="id">The specific id of the attachment.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] TicketAttachmentRepository attachmentRepo,
            [FromBody] string id)
        {
            var attachment = await attachmentRepo.GetAsync(new Guid(id));

            if (attachment is null)
                return this.NotFound($"An attachment with the id '{id}' doesn´t exist.");

            await attachmentRepo.DeleteAsync(attachment);

            return this.Ok($"The attachment with the id '{id}' was deleted.");
        }
    }
}
