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
        /// <param name="ticketId">The specific ticket id.</param>
        /// <returns>An <see cref="ObjectResult"/> with specific <see cref="Ticket"/>.</returns>
        [HttpGet("{ticketId}")]
        public async Task<ActionResult> Get(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] string ticketId)
        {
            return this.Ok(await ticketRepo.GetAsync(new Guid(ticketId)));
        }

        /// <summary>
        /// Gets a ticket filtert by a specific search term.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="searchTerm">The specific search term as a <see langword="string"/>.</param>
        /// <param name="skip">The count of skipped elements as a <see langword="int"/> Default = 0.</param>
        /// <param name="take">The count of taken elements as a <see langword="int"/> Default = 10.</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of the filtered tickets.</returns>
        [HttpGet("search")]
        public ActionResult Get(
            [FromServices] TicketRepository ticketRepo,
            [FromQuery] string searchTerm,
            [FromQuery] int skip = 0,
            [FromQuery] int? take = null)
        {
            searchTerm = searchTerm.ToLower();

            var ticketQuery = ticketRepo.GetAll().Where(ticket =>
                ((ticket.Description != null) && ticket.Description.ToLower().Contains(searchTerm))
                || ((ticket.Building != null) && ticket.Building.Name.ToLower().Contains(searchTerm))
                || ((ticket.Room != null) && ticket.Room.ToLower().Contains(searchTerm))
                || ((ticket.Priority != null) && ticket.Priority.Name.ToLower().Contains(searchTerm))
                || ((ticket.State != null) && ticket.State.Name.ToLower().Contains(searchTerm))
                || ticket.Title.ToLower().Contains(searchTerm)
                || ticket.Author.ToLower().Contains(searchTerm)
                || ticket.CreationDate.ToString().ToLower().Contains(searchTerm))
                .Skip(skip);

            if (take is not null)
            {
                ticketQuery = ticketQuery.Take((int)take);
            }

            Ticket[] tickets = [.. ticketQuery];

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
            var state = await stateRepo.GetAsync(ticketData.StateId)
                ?? throw new BadHttpRequestException("Das zu erstellende Ticket muss einen gültigen Zustand (state) haben!");
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
        /// <param name="commentData">Instance of <see cref="CommentDTO"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added comment entity.</returns>
        [HttpPost("{ticketId}/comment")]
        public async Task<ActionResult> PostComment(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] TicketCommentRepository commentRepo,
            [FromRoute] string ticketId,
            [FromBody] CommentDTO commentData)
        {
            Ticket? ticket = await ticketRepo.GetAsync(new Guid(ticketId));

            if (ticket is null)
                return this.NotFound();

            TicketComment comment = new ()
            {
                Author = commentData.Author,
                Content = commentData.Content,
                CreationDate = commentData.CreationDate,
                TicketId = new Guid(ticketId),
            };

            await commentRepo.AddAsync(comment);

            return this.Ok(comment);
        }

        /// <summary>
        /// Adds a new attachment entity of a specific ticket.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="attachmentRepo">Instance of <see cref="TicketAttachmentRepository"/>.</param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <param name="attachmentFile">Instance of <see cref="IFormFile"/></param>
        /// <returns>An <see cref="ObjectResult"/> with the added attachment entity.</returns>
        [HttpPost("{ticketId}/attachment")]
        public async Task<ActionResult> PostAttachment(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] TicketAttachmentRepository attachmentRepo,
            [FromRoute] string ticketId,
            IFormFile attachmentFile)
        {
            Ticket? ticket = await ticketRepo.GetAsync(new Guid(ticketId));

            if (ticket is null)
                return this.NotFound();

            using MemoryStream stream = new ();
            await attachmentFile.CopyToAsync(stream);

            TicketAttachment attachment = new ()
            {
                Name = attachmentFile.FileName,
                Binary = stream.ToArray(),
                FileType = attachmentFile.FileName.Split(".").Last().ToLower(),
                TicketId = new Guid(ticketId),
            };

            await attachmentRepo.AddAsync(attachment);

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
            var state = await stateRepo.GetAsync(ticketData.StateId)
                ?? throw new BadHttpRequestException("Das zu erstellende Ticket muss einen gültigen Zustand (state) haben!");
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
                return this.NotFound();

            await ticketRepo.DeleteAsync(deleteTicket);

            return this.Ok($"Das Ticket mit der ID '{ticketId}' wurde gelöscht.");
        }
    }
}
