using Common.Contracts.Post;
using Common.Contracts.Put;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Security.Claims;
using Application.WebApi.Decorators;
using Blurhash.ImageSharp;
using Application.WebApi.Contracts.Response;
using Common.Enums;
using Microsoft.EntityFrameworkCore;
using DomainLayer.BusinessLogic.Mailing;
using MimeKit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage ticket requests.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        /// <summary>
        /// Gets a ticket by a specific ticket id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="ticketId">The specific ticket id.</param>
        /// <returns>An <see cref="ObjectResult"/> with specific <see cref="Ticket"/>.</returns>
        [HttpGet("{ticketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<Ticket>> Get(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] Guid ticketId)
        {
            var ticket = await ticketRepo.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound($"Ticket with id '{ticketId}' doesn't exist!");

            return this.Ok(ticket);
        }

        /// <summary>
        /// Gets a ticket filtert by a specific search term and the max amount of results used for pagination.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="searchTerm">The specific search term as a <see langword="string"/>.</param>
        /// <param name="orderByColumn">The column that should be used for returning ordered results <see langword="string"/>.</param>
        /// <param name="orderDesc">The direction the results should be ordered in (true for descending, false for ascending) <see langword="string"/>.</param>
        /// <param name="skip">The count of skipped elements as a <see langword="int"/> Default = 0.</param>
        /// <param name="take">The count of taken elements as a <see langword="int"/> Default = 10.</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of the filtered tickets.</returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public ActionResult<TicketSearchDTO> Get(
            [FromServices] TicketRepository ticketRepo,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string orderByColumn = "CreationDate",
            [FromQuery] bool orderDesc = false,
            [FromQuery] int skip = 0,
            [FromQuery] int? take = null)
        {
            var ticketQuery = ticketRepo.GetAll();

            if (searchTerm is not null)
            {
                searchTerm = searchTerm.ToLower();
                ticketQuery = ticketQuery.Where(ticket =>
                    ((ticket.Description != null) && ticket.Description.ToLower().Contains(searchTerm))
                    || ((ticket.Building != null) && ticket.Building.Name.ToLower().Contains(searchTerm))
                    || ((ticket.Room != null) && ticket.Room.ToLower().Contains(searchTerm))
                    || ticket.Priority.Name.ToLower().Contains(searchTerm)
                    || ticket.State.Name.ToLower().Contains(searchTerm)
                    || ticket.Title.ToLower().Contains(searchTerm)
                    || ticket.Author.ToLower().Contains(searchTerm));
            }

            Ticket[] tickets = [.. ticketQuery];
            var totalCount = tickets.Length;
            ticketQuery =
                orderDesc
                    ? ticketQuery.OrderByDescending(ticket => EF.Property<Ticket>(ticket, orderByColumn)).Skip(skip)
                    : ticketQuery.OrderBy(ticket => EF.Property<Ticket>(ticket, orderByColumn)).Skip(skip);

            if (take is not null)
            {
                ticketQuery = (IOrderedQueryable<Ticket>)ticketQuery.Take((int)take);
            }

            tickets = [.. ticketQuery];
            return this.Ok(new TicketSearchDTO
            {
                Total = totalCount, Results = tickets,
            });
        }

        /// <summary>
        /// Fetches tickets by a given property. Property can be a subclass, classPropertyName has to be specified as the property to be compared in the subclass.
        /// Supports String|Int|Boolean|DateTime|Guid fields.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="searchTerm">The specific search term as a <see langword="string"/>.</param>
        /// <param name="property">The property to be searched</param>
        /// <param name="classPropertyName">The name of the property to be compared in case the ticket property is a class</param>
        /// <param name="orderByColumn">The column that should be used for returning ordered results <see langword="string"/>.</param>
        /// <param name="orderDesc">The direction the results should be ordered in (true for descending, false for ascending) <see langword="string"/>.</param>
        /// <param name="skip">The count of skipped elements as a <see langword="int"/> Default = 0.</param>
        /// <param name="take">The count of taken elements as a <see langword="int"/> Default = 10.</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of the filtered tickets.</returns>
        [HttpGet("searchByProperty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public ActionResult<TicketSearchDTO> GetByProperty(
            [FromServices] TicketRepository ticketRepo,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string property = "Title",
            [FromQuery] string? classPropertyName = null,
            [FromQuery] string orderByColumn = "CreationDate",
            [FromQuery] bool orderDesc = false,
            [FromQuery] int skip = 0,
            [FromQuery] int? take = null)
        {
            var ticketQuery = searchTerm is null
                ? ticketRepo.GetAll()
                : ticketRepo.GetByPropertyValue(searchTerm, property, classPropertyName);

            Ticket[] tickets = [.. ticketQuery];
            var totalCount = tickets.Length;
            ticketQuery =
                orderDesc
                    ? ticketQuery.OrderByDescending(ticket => EF.Property<Ticket>(ticket, orderByColumn)).Skip(skip)
                    : ticketQuery.OrderBy(ticket => EF.Property<Ticket>(ticket, orderByColumn)).Skip(skip);

            if (take is not null)
            {
                ticketQuery = (IOrderedQueryable<Ticket>)ticketQuery.Take((int)take);
            }

            tickets = [.. ticketQuery];
            return this.Ok(new TicketSearchDTO
            {
                Total = totalCount, Results = tickets,
            });
        }

        /// <summary>
        /// Gets the comments of the ticket with the specific id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of <see cref="TicketComment"/> from the specific <see cref="Ticket"/>.</returns>
        [HttpGet("{ticketId}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<Collection<TicketComment>>> GetComments(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] Guid ticketId)
        {
            Ticket? ticket = await ticketRepo.GetAsync(ticketId);

            Collection<TicketComment>? ticketComments = ticket?.Comments;

            return this.Ok(ticketComments);
        }

        /// <summary>
        /// Gets the attachments of the ticket with the specific id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <returns>An <see cref="ObjectResult"/> with an <see cref="Array"/> of <see cref="TicketAttachment"/> from the specific <see cref="Ticket"/>.</returns>
        [HttpGet("{ticketId}/attachments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<Collection<TicketAttachment>>> GetAttachments(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] Guid ticketId)
        {
            Ticket? ticket = await ticketRepo.GetAsync(ticketId);

            Collection<TicketAttachment>? ticketAttachments = ticket?.Attachments;

            return this.Ok(ticketAttachments);
        }

        /// <summary>
        /// Adds a new ticket entity.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/>.</param>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/>.</param>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/>.</param>
        /// <param name="ticketData">The ticket data that will be added.</param>
        /// <returns>An <see cref="ObjectResult"/> with the added ticket entity.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AuthorizeRoles(Roles.Teacher | Roles.FacilityManager | Roles.Admin)]
        public async Task<ActionResult<Ticket>> Post(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] StateRepository stateRepo,
            [FromServices] PriorityRepository priorityRepo,
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] TicketPostDTO ticketData)
        {
            var ticketState = await stateRepo.GetAsync(ticketData.StateId);
            var defaultState = stateRepo.Find(state => state.IsDefault).FirstOrDefault();

            if (ticketState is null)
                return this.BadRequest("The ticket to be created must have a valid state.");

            var priority = await priorityRepo.GetAsync(ticketData.PriorityId);

            if (priority is null)
                return this.BadRequest("The ticket to be created must have a valid priority.");

            Building? building = null;

            if (ticketData.BuildingId is not null)
            {
                building = await buildingRepo.GetAsync((Guid)ticketData.BuildingId);

                if (building is null)
                    return this.BadRequest("The given building id doesn´t exist.");
            }

            var author = this.User.FindFirst("username")?.Value;

            if (author is null)
                return this.Unauthorized();

            Ticket ticket = new()
            {
                Id = default,
                Title = ticketData.Title,
                Author = author,
                CreationDate = DateTime.UtcNow,
                State = ticketState,
                LastStateUpdate = DateTime.UtcNow,
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
        /// <remarks>
        /// Also sends mails to involved and available users
        /// </remarks>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="commentRepo">Instance of <see cref="TicketCommentRepository"/>.</param>
        /// <param name="mailingService">Instance of <see cref="MailingService"/></param>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <param name="commentContent">The comment content that will be added.</param>
        /// <returns>An <see cref="ObjectResult"/> with the added comment entity.</returns>
        [HttpPost("{ticketId}/comment")]
        [Consumes("text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AuthorizeRoles(Roles.Teacher | Roles.FacilityManager | Roles.Admin)]
        public async Task<ActionResult<TicketComment>> PostComment(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] TicketCommentRepository commentRepo,
            [FromServices] MailingService mailingService,
            [FromServices] UserRepository userRepository,
            [FromRoute] Guid ticketId,
            [FromBody] string commentContent)
        {
            Ticket? ticket = await ticketRepo.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound($"A ticket with the id '{ticketId}' doesn´t exist.");

            var author = this.User.FindFirst("username")?.Value;

            if (author is null)
                return this.Unauthorized();

            TicketComment comment = new()
            {
                Id = default,
                Author = author,
                Content = commentContent,
                CreationDate = DateTime.UtcNow,
                TicketId = ticketId,
            };

            await commentRepo.AddAsync(comment);

            var mailRecipientIds = ticketRepo.GetInvolvedUsersIds(ticket, [author]);
            var mailAddresses = userRepository.GetMany(mailRecipientIds)
                .Where(u => !string.IsNullOrWhiteSpace(u.Email))
                .Select(u => new MailboxAddress(u.UserId, u.Email));

            var mail = mailingService.GenerateNewTicketCommentMail(comment);
            await mailingService.SendMany(mailAddresses, mailingService.FormatMailSubject($"Neuer Kommentar an Ticket '{ticket.Title}'"), mail);

            return this.Ok(comment);
        }

        /// <summary>
        /// Adds a new attachment entity of a specific ticket.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="attachmentRepo">Instance of <see cref="TicketAttachmentRepository"/>.</param>
        /// <param name="mailingService">Instance of <see cref="MailingService"/></param>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <param name="ticketId">The specific ticket id</param>
        /// <param name="attachmentFile">The file that will be uploaded.</param>
        /// <returns>An <see cref="ObjectResult"/> with the added attachment entity.</returns>
        [HttpPost("{ticketId}/attachment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AuthorizeRoles(Roles.Teacher | Roles.FacilityManager | Roles.Admin)]
        public async Task<ActionResult<TicketAttachment>> PostAttachment(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] TicketAttachmentRepository attachmentRepo,
            [FromServices] MailingService mailingService,
            [FromServices] UserRepository userRepository,
            [FromRoute] Guid ticketId,
            IFormFile attachmentFile)
        {
            Ticket? ticket = await ticketRepo.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound($"A ticket with the id '{ticketId}' doesn´t exist.");

            using MemoryStream stream = new();
            await attachmentFile.CopyToAsync(stream);

            string? blurHash = null;
            if (attachmentFile.ContentType.StartsWith("image/"))
            {
                using var image = Image.Load<Rgba32>(stream.ToArray());
                blurHash = Blurhasher.Encode(image, 5, 5);
            }

            TicketAttachment attachment = new()
            {
                Id = default,
                Name = attachmentFile.FileName,
                Binary = stream.ToArray(),
                FileType = attachmentFile.FileName.Split(".").Last().ToLower(),
                TicketId = ticketId,
                BlurHash = blurHash,
            };

            await attachmentRepo.AddAsync(attachment);

            string performingUser = this.User.FindFirstValue("username") ?? string.Empty;
            var mailRecipientIds = ticketRepo.GetInvolvedUsersIds(ticket, [performingUser]);
            var mailAddresses = userRepository.GetMany(mailRecipientIds)
                .Where(u => !string.IsNullOrWhiteSpace(u.Email))
                .Select(u => new MailboxAddress(u.UserId, u.Email));

            var mail = mailingService.GenerateNewTicketAttachmentMail(attachment);
            await mailingService.SendMany(mailAddresses, mailingService.FormatMailSubject($"Neuer Anhang an Ticket '{ticket.Title}'"), mail);

            return this.Ok(attachment);
        }

        /// <summary>
        /// Updates a specific ticket.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="stateRepo">Instance of <see cref="StateRepository"/>.</param>
        /// <param name="priorityRepo">Instance of <see cref="PriorityRepository"/>.</param>
        /// <param name="buildingRepo">Instance of <see cref="BuildingRepository"/>.</param>
        /// <param name="mailingService">Instance of <see cref="MailingService"/></param>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <param name="ticketData">The ticket data that will update the ticket.</param>
        /// <returns>An <see cref="ObjectResult"/> with the updated ticket.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AuthorizeRoles(Roles.Teacher | Roles.FacilityManager | Roles.Admin)]
        public async Task<ActionResult<Ticket>> Put(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] StateRepository stateRepo,
            [FromServices] PriorityRepository priorityRepo,
            [FromServices] BuildingRepository buildingRepo,
            [FromServices] MailingService mailingService,
            [FromServices] UserRepository userRepository,
            [FromBody] TicketPutDTO ticketData)
        {
            var ticket = await ticketRepo.GetAsync(ticketData.Id);

            if (ticket is null)
                return this.NotFound($"A ticket with the id '{ticketData.Id}' doesn´t exist.");

            var userClaim = this.User.FindFirst("username")?.Value;
            var rolesClaim = this.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userClaim is null || rolesClaim is null)
                return this.Unauthorized();

            if (!Enum.TryParse(rolesClaim, out Roles role))
                return this.Unauthorized();

            if (!ticket.Author.Equals(userClaim) && !role.HasFlag(Roles.FacilityManager) && !role.HasFlag(Roles.Admin))
            {
                return this.Forbid();
            }

            State? state = await stateRepo.GetAsync(ticketData.StateId);
            Building? building = ticketData.BuildingId is null
                ? null
                : await buildingRepo.GetAsync(ticketData.BuildingId.Value);

            Priority? priority = await priorityRepo.GetAsync(ticketData.PriorityId);

            var oldTicket = ticket with { };

            ticket.Title = ticketData.Title ?? ticket.Title;
            ticket.Description = ticketData.Description ?? ticket.Description;
            ticket.State = state ?? ticket.State;
            ticket.LastStateUpdate = state is not null && !state.Id.Equals(ticket.State.Id)
                ? DateTime.UtcNow
                : ticket.LastStateUpdate;
            ticket.Building = building ?? ticket.Building;
            ticket.Priority = priority ?? ticket.Priority;
            ticket.Object = ticketData.Object ?? ticket.Object;
            ticket.Room = ticketData.Room ?? ticket.Room;

            await ticketRepo.UpdateAsync(ticket);

            var mailRecipientIds = ticketRepo.GetInvolvedUsersIds(ticket, [userClaim]);
            var mailAddresses = userRepository.GetMany(mailRecipientIds)
                .Where(u => !string.IsNullOrWhiteSpace(u.Email))
                .Select(u => new MailboxAddress(u.UserId, u.Email));

            var mail = mailingService.GenerateTicketChangedMail(ticket, oldTicket);
            await mailingService.SendMany(mailAddresses, mailingService.FormatMailSubject($"Ticket '{ticket.Title}' wurde geändert"), mail);

            return this.Ok(ticket);
        }

        /// <summary>
        /// Deletes the ticket with the specific id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="ticketId">The specific id of the ticket that will be deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete("{ticketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] Guid ticketId)
        {
            var ticket = await ticketRepo.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound($"A ticket with the id '{ticketId}' doesn´t exist.");

            var userClaim = this.User.FindFirst("username")?.Value;
            var rolesClaim = this.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userClaim is null || rolesClaim is null)
                return this.Unauthorized();

            if (!Enum.TryParse(rolesClaim, out Roles role))
                return this.Unauthorized();

            if (!ticket.Author.Equals(userClaim) && !role.HasFlag(Roles.FacilityManager) && !role.HasFlag(Roles.Admin))
            {
                return this.Forbid();
            }

            await ticketRepo.DeleteAsync(ticket);

            return this.Ok($"The ticket with the id '{ticketId}' was deleted.");
        }
    }
}