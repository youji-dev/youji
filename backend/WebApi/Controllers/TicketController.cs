using Microsoft.AspNetCore.Mvc;
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
    [ApiController]
    public class TicketController(
        TicketRepository ticketRepo,
        TicketCommentRepository commentRepo,
        TicketAttachmentRepository attachmentRepo) : Controller
    {
        /// <summary>
        /// Gets a ticket by a specific ticket id.
        /// </summary>
        /// <param name="id">Id of a ticket as <see langword="string"/></param>
        /// <returns>An action result with the specific ticket./></returns>
        [HttpGet]
        public async Task<ActionResult<Ticket>> Get(string id)
        {
            return this.Ok(await ticketRepo.GetAsync(new Guid(id)));
        }

        [HttpGet]
        public async Task<ActionResult<Ticket[]>> Get(string searchTerm, int skip, int take)
        {
            var tickets = await ticketRepo.GetAllAsync(
                tickets =>
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
                .Count() > 0);

            if (tickets is null)
            {
                return this.BadRequest("Keine Tickets mit diesem Suchinhalt gefunden.");
            }

            return this.Ok(tickets);
        }

        [HttpGet("comments")]
        public async Task<ActionResult<TicketComment[]>> GetComments(string id)
        {
            Ticket? ticket = await ticketRepo.GetAsync(new Guid(id));

            TicketComment[]? ticketComments = ticket?.Comments;

            return this.Ok(ticketComments);
        }

        [HttpGet("attachments")]
        public async Task<ActionResult<TicketAttachment[]>> GetAttachments(string id)
        {
            Ticket? ticket = await ticketRepo.GetAsync(new Guid(id));

            TicketAttachment[]? ticketAttachments = ticket?.Attachments;

            return this.Ok(ticketAttachments);
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> Post(Ticket ticket)
        {
            await ticketRepo.AddAsync(ticket);

            return this.Ok(ticket);
        }

        [HttpPost("comment")]
        public async Task<ActionResult<TicketComment>> PostComment(string ticketId, TicketComment comment)
        {
            await commentRepo.AddAsync(comment);

            Ticket? ticket = ticketRepo.GetAsync(new Guid(ticketId)).Result;

            if (ticket is null)
            {
                return this.BadRequest("Es konnte kein Ticket mit dieser ID gefunden werden");
            }

            ticket.Comments = [.. ticket.Comments, comment];

            await ticketRepo.UpdateAsync(ticket);

            return this.Ok($"Kommantar '{comment}' wurde zu Ticket {ticket.Id} hinzugefügt.");
        }

        [HttpPost("attachment")]
        public async Task<ActionResult<TicketAttachment>> PostAttachment(string ticketId, TicketAttachment attachment)
        {
            await attachmentRepo.AddAsync(attachment);

            Ticket? ticket = ticketRepo.GetAsync(new Guid(ticketId)).Result;

            if (ticket is null)
            {
                return this.BadRequest("Es konnte kein Ticket mit dieser ID gefunden werden");
            }

            ticket.Attachments = [.. ticket.Attachments, attachment];

            await ticketRepo.UpdateAsync(ticket);

            return this.Ok($"Anhang '{attachment.Name}' wurde zu Ticket {ticket.Id} hinzugefügt.");
        }

        [HttpPut]
        public async Task<ActionResult<Ticket>> Put(Ticket ticket)
        {
            await ticketRepo.UpdateAsync(ticket);
            return this.Ok(ticket);
        }
    }
}
