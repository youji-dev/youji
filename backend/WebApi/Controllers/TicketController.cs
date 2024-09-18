using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage ticket requests.
    /// </summary>
    /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/></param>
    /// <param name="commentRepo">Instance of <see cref="TicketCommentRepository"/></param>
    /// <param name="attachmentRepo">Instance of <see cref="TicketAttachmentRepository"/></param>
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController(
        TicketRepository ticketRepo,
        TicketCommentRepository commentRepo,
        TicketAttachmentRepository attachmentRepo) : Controller
    {
        /// <summary>
        /// Gets a ticket by a specific ticket id.
        /// </summary>
        /// <param name="id">The specific ticket id.</param>
        /// <returns>An <see cref="ObjectResult"/> with specific <see cref="Ticket"/>.</returns>
        [HttpGet]
        public async Task<ActionResult> Get(string id)
        {
            return this.Ok(await ticketRepo.GetAsync(new Guid(id)));
        }

        /// <summary>
        /// Gets a ticket filtert by a specific search term.
        /// </summary>
        /// <param name="searchTerm">The specific search term as a <see langword="string"/>.</param>
        /// <param name="skip">The count of skipped elements as a <see langword="int"/>.</param>
        /// <param name="take">The count of taken elements as a <see langword="int"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of the filtered tickets.</returns>
        [HttpGet("search")]
        public ActionResult Get(string searchTerm, int skip, int take)
        {
            var tickets = ticketRepo.GetAllAsync(tickets =>
                tickets.Where(
                    ticket =>
                    (ticket.Description != null && ticket.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    || (ticket.Building != null && ticket.Building.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    || (ticket.Room != null && ticket.Room.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    || (ticket.Priority != null && ticket.Priority.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    || ticket.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    || ticket.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    || ticket.CreationDate.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    || ticket.State.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Skip(skip)
                .Take(take)
                .Count() > 0)
                .First();

            if (tickets is null)
            {
                return this.NotFound();
            }

            return this.Ok(tickets);
        }

        /// <summary>
        /// Gets the comments of the ticket with the specific id.
        /// </summary>
        /// <param name="ticketId">The specific ticket id</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of <see cref="TicketComment"/> from the specific <see cref="Ticket"/>.</returns>
        [HttpGet("comments")]
        public async Task<ActionResult> GetComments(string ticketId)
        {
            Ticket? ticket = await ticketRepo.GetAsync(new Guid(ticketId));

            TicketComment[]? ticketComments = ticket?.Comments;

            return this.Ok(ticketComments);
        }

        /// <summary>
        /// Gets the attachments of the ticket with the specific id.
        /// </summary>
        /// <param name="ticketId">The specific ticket id</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of <see cref="TicketAttachment"/> from the specific <see cref="Ticket"/>.</returns>
        [HttpGet("attachments")]
        public async Task<ActionResult> GetAttachments(string ticketId)
        {
            Ticket? ticket = await ticketRepo.GetAsync(new Guid(ticketId));

            TicketAttachment[]? ticketAttachments = ticket?.Attachments;

            return this.Ok(ticketAttachments);
        }

        /// <summary>
        /// Adds a new ticket entity.
        /// </summary>
        /// <param name="ticket">Instance of <see cref="Ticket"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added ticket entity.</returns>
        [HttpPost]
        public async Task<ActionResult> Post(Ticket ticket)
        {
            await ticketRepo.AddAsync(ticket);

            return this.Ok(ticket);
        }

        /// <summary>
        /// Adds a new comment entity of a specific ticket.
        /// </summary>
        /// <param name="ticketId">The specific ticket id</param>
        /// <param name="comment">Instance of <see cref="TicketComment"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added comment entity.</returns>
        [HttpPost("comment")]
        public async Task<ActionResult> PostComment(string ticketId, TicketComment comment)
        {
            await commentRepo.AddAsync(comment);

            Ticket? ticket = await ticketRepo.GetAsync(new Guid(ticketId));

            if (ticket is null)
            {
                return this.NotFound();
            }

            ticket.Comments = [.. ticket.Comments, comment];

            await ticketRepo.UpdateAsync(ticket);

            return this.Ok(comment);
        }

        /// <summary>
        /// Adds a new attachment entity of a specific ticket.
        /// </summary>
        /// <param name="ticketId">The specific ticket id</param>
        /// <param name="attachment">Instance of <see cref="TicketAttachment"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added attachment entity.</returns>
        [HttpPost("attachment")]
        public async Task<ActionResult> PostAttachment(string ticketId, TicketAttachment attachment)
        {
            await attachmentRepo.AddAsync(attachment);

            Ticket? ticket = ticketRepo.GetAsync(new Guid(ticketId)).Result;

            if (ticket is null)
            {
                return this.NotFound();
            }

            ticket.Attachments = [.. ticket.Attachments, attachment];

            await ticketRepo.UpdateAsync(ticket);

            return this.Ok(attachment);
        }

        /// <summary>
        /// Updates the specific ticket.
        /// </summary>
        /// <param name="ticket">Instance of <see cref="Ticket"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated ticket.</returns>
        [HttpPut]
        public async Task<ActionResult> Put(Ticket ticket)
        {
            await ticketRepo.UpdateAsync(ticket);
            return this.Ok(ticket);
        }

        /// <summary>
        /// Deletes the ticket with the specific id.
        /// </summary>
        /// <param name="id">The specific id of the ticket that will deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            var deleteTicket = await ticketRepo.GetAsync(new Guid(id));

            if (deleteTicket is null)
            {
                return this.NotFound();
            }

            await ticketRepo.DeleteAsync(deleteTicket);

            return this.Ok($"Das Ticket mit der ID '{id}' wurde gelöscht.");
        }
    }
}
