using Common.Contracts;
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
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        /// <summary>
        /// Gets a ticket by a specific ticket id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="id">The specific ticket id.</param>
        /// <returns>An <see cref="ObjectResult"/> with specific <see cref="Ticket"/>.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] string id)
        {
            return this.Ok(await ticketRepo.GetAsync(new Guid(id)));
        }

        /// <summary>
        /// Gets a ticket filtert by a specific search term.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="searchTerm">The specific search term as a <see langword="string"/>.</param>
        /// <param name="skip">The count of skipped elements as a <see langword="int"/>.</param>
        /// <param name="take">The count of taken elements as a <see langword="int"/>.</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of the filtered tickets.</returns>
        [HttpGet("search")]
        public ActionResult Get(
            [FromServices] TicketRepository ticketRepo,
            [FromBody] string[] searchTerm,
            [FromQuery] int skip,
            [FromQuery] int take)
        {
            var tickets = ticketRepo.GetAllAsync(tickets =>
                tickets.Where(
                    ticket =>
                    ((ticket.Description != null) && searchTerm.Any(term => ticket.Description.Contains(term, StringComparison.OrdinalIgnoreCase)))
                    || ((ticket.Building != null) && searchTerm.Any(term => ticket.Building.Name.Contains(term, StringComparison.OrdinalIgnoreCase)))
                    || ((ticket.Room != null) && searchTerm.Any(term => ticket.Room.Contains(term, StringComparison.OrdinalIgnoreCase)))
                    || ((ticket.Priority != null) && searchTerm.Any(term => ticket.Priority.Name.Contains(term, StringComparison.OrdinalIgnoreCase)))
                    || searchTerm.Any(term => ticket.Title.Contains(term, StringComparison.OrdinalIgnoreCase))
                    || searchTerm.Any(term => ticket.Author.Contains(term, StringComparison.OrdinalIgnoreCase))
                    || searchTerm.Any(term => ticket.CreationDate.ToString().Contains(term, StringComparison.OrdinalIgnoreCase))
                    || searchTerm.Any(term => ticket.State.Name.Contains(term, StringComparison.OrdinalIgnoreCase)))
                .Skip(skip)
                .Take(take) != null)
                .Single();

            if (tickets is null)
            {
                return this.NotFound();
            }

            return this.Ok(tickets);
        }

        /// <summary>
        /// Gets the comments of the ticket with the specific id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of <see cref="TicketComment"/> from the specific <see cref="Ticket"/>.</returns>
        [HttpGet("{ticketId}/comments")]
        public async Task<ActionResult> GetComments(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] string ticketId)
        {
            Ticket? ticket = await ticketRepo.GetAsync(new Guid(ticketId));

            ICollection<TicketComment>? ticketComments = ticket?.Comments;

            return this.Ok(ticketComments);
        }

        /// <summary>
        /// Gets the attachments of the ticket with the specific id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of <see cref="TicketAttachment"/> from the specific <see cref="Ticket"/>.</returns>
        [HttpGet("{ticketId}/attachments")]
        public async Task<ActionResult> GetAttachments(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] string ticketId)
        {
            Ticket? ticket = await ticketRepo.GetAsync(new Guid(ticketId));

            ICollection<TicketAttachment>? ticketAttachments = ticket?.Attachments;

            return this.Ok(ticketAttachments);
        }

        /// <summary>
        /// Adds a new ticket entity.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/>.</param>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/>.</param>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/>.</param>
        /// <param name="ticketData">Instance of <see cref="TicketDTO"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added ticket entity.</returns>
        [HttpPost]
        public async Task<ActionResult> Post(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] StateRepository stateRepo,
            [FromServices] PriorityRepository priorityRepo,
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] TicketDTO ticketData)
        {
            var state = await stateRepo.GetAsync(ticketData.StateId);
            var building = await buildingRepo.GetAsync(ticketData.BuildingId);
            var priority = await priorityRepo.GetAsync(ticketData.PriorityValue);

            Ticket ticket = new ()
            {
                Title = ticketData.Title,
                Author = ticketData.Author,
                CreationDate = ticketData.CreationDate,
                State = state,
                Description = ticketData.Description,
                Priority = priority,
                Building = building,
                Object = ticketData.Object,
                Room = ticketData.Room,
            };

            await ticketRepo.AddAsync(ticket);

            return this.Ok(ticket);
        }

        /// <summary>
        /// Adds a new comment entity of a specific ticket.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="commentRepo">Instance of <see cref="TicketCommentRepository"/>.</param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <param name="comment">Instance of <see cref="TicketComment"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added comment entity.</returns>
        [HttpPost("{ticketId}/comment")]
        public async Task<ActionResult> PostComment(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] TicketCommentRepository commentRepo,
            [FromRoute] string ticketId,
            [FromBody] TicketComment comment)
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
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="attachmentRepo">Instance of <see cref="TicketAttachmentRepository"/>.</param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <param name="attachment">Instance of <see cref="TicketAttachment"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added attachment entity.</returns>
        [HttpPost("{ticketId}/attachment")]
        public async Task<ActionResult> PostAttachment(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] TicketAttachmentRepository attachmentRepo,
            [FromRoute] string ticketId,
            [FromBody] TicketAttachment attachment)
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
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/>.</param>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/>.</param>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/>.</param>
        /// <param name="ticketData">Instance of <see cref="TicketDTO"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the updated ticket.</returns>
        [HttpPut]
        public async Task<ActionResult> Put(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] StateRepository stateRepo,
            [FromServices] PriorityRepository priorityRepo,
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] TicketDTO ticketData)
        {
            var state = await stateRepo.GetAsync(ticketData.StateId);
            var building = await buildingRepo.GetAsync(ticketData.BuildingId);
            var priority = await priorityRepo.GetAsync(ticketData.PriorityValue);

            Ticket ticket = new ()
            {
                Title = ticketData.Title,
                Author = ticketData.Author,
                CreationDate = ticketData.CreationDate,
                State = state,
                Description = ticketData.Description,
                Priority = priority,
                Building = building,
                Object = ticketData.Object,
                Room = ticketData.Room,
            };

            await ticketRepo.UpdateAsync(ticket);

            return this.Ok(ticket);
        }

        /// <summary>
        /// Deletes the ticket with the specific id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="ticketId">The specific id of the ticket that will deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete("{ticketId}")]
        public async Task<ActionResult> Delete(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] string ticketId)
        {
            var deleteTicket = await ticketRepo.GetAsync(new Guid(ticketId));

            if (deleteTicket is null)
            {
                return this.NotFound();
            }

            await ticketRepo.DeleteAsync(deleteTicket);

            return this.Ok($"Das Ticket mit der ID '{ticketId}' wurde gelöscht.");
        }
    }
}
