using Common.Contracts.Post;
using Common.Contracts.Put;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security.Claims;
using Application.WebApi.Contracts.Request;
using Application.WebApi.Decorators;
using Blurhash.ImageSharp;
using Application.WebApi.Contracts.Response;
using Common.Enums;
using DomainLayer.BusinessLogic.Mailing;
using LinqKit;
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
        public async Task<ActionResult<Ticket>> GetById(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] Guid ticketId)
        {
            var ticket = await ticketRepo.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound($"Ticket with id '{ticketId}' doesn't exist!");

            return this.Ok(ticket);
        }

        /// <summary>
        /// Gets a collection of tickets by the provided search request.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/></param>
        /// <param name="searchRequest">The specified search params, sorting order and pagination</param>
        /// <returns>Actions result with Array of <see cref="Ticket"/></returns>
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public ActionResult<TicketSearchResponseDTO> GetByProperty(
            [FromServices] TicketRepository ticketRepo,
            [FromBody] TicketSearchRequestDTO searchRequest)
        {
            var ticketQuery = ticketRepo.GetAll();

            if (searchRequest.Filters != null)
            {
                var predicate = PredicateBuilder.New<Ticket>(true);

                foreach (var filter in searchRequest.Filters)
                {
                    foreach (var value in filter.Value)
                    {
                        var searchValue = value.ToString();
                        if (searchValue is null)
                            continue;

                        var filterPredicate = filter.Key switch
                        {
                            nameof(Ticket.Id) => PredicateBuilder.New<Ticket>(t => t.Id == Guid.Parse(searchValue)),
                            nameof(Ticket.Title) => PredicateBuilder.New<Ticket>(t => t.Title.ToLower().Contains(searchValue.ToLower())),
                            nameof(Ticket.Description) => PredicateBuilder.New<Ticket>(t => t.Description != null && t.Description.ToLower().Contains(searchValue.ToLower())),
                            nameof(Ticket.Priority) => PredicateBuilder.New<Ticket>(t => t.Priority.Id == Guid.Parse(searchValue)),
                            nameof(Ticket.State) => PredicateBuilder.New<Ticket>(t => t.State.Id == Guid.Parse(searchValue)),
                            nameof(Ticket.Building) => PredicateBuilder.New<Ticket>(t => t.Building != null && t.Building.Id == Guid.Parse(searchValue)),
                            nameof(Ticket.Room) => PredicateBuilder.New<Ticket>(t => t.Room != null && t.Room.ToLower() == searchValue.ToLower()),
                            nameof(Ticket.Object) => PredicateBuilder.New<Ticket>(t => t.Object != null && t.Object.ToLower().Contains(searchValue.ToLower())),
                            _ => PredicateBuilder.New<Ticket>(true),
                        };

                        predicate = searchRequest.UseOr ? predicate.Or(filterPredicate) : predicate.And(filterPredicate);
                    }
                }

                ticketQuery = ticketQuery.Where(predicate);
            }

            if (!string.IsNullOrEmpty(searchRequest.OrderByColumn))
            {
                Expression<Func<Ticket, object>> keySelector = searchRequest.OrderByColumn switch
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    nameof(Ticket.Id) => t => t.Id.ToString(),
                    nameof(Ticket.Title) => t => t.Title,
                    nameof(Ticket.Description) => t => t.Description ?? string.Empty,
                    nameof(Ticket.Priority) => t => t.Priority.Name,
                    nameof(Ticket.State) => t => t.State.Name,
                    nameof(Ticket.Building) => t => t.Building.Name,
                    nameof(Ticket.Room) => t => t.Room ?? string.Empty,
                    nameof(Ticket.Object) => t => t.Object ?? string.Empty,
                    nameof(Ticket.CreationDate) => t => t.CreationDate,
                    nameof(Ticket.Author) => t => t.Author,
                    _ => throw new ArgumentException("Invalid OrderBy column"),
                };
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                ticketQuery = searchRequest.OrderDesc ? ticketQuery.OrderByDescending(keySelector) : ticketQuery.OrderBy(keySelector);
            }

            Ticket[] tickets = [.. ticketQuery];
            var totalCount = tickets.Length;

            tickets = [.. ticketQuery.Skip(searchRequest.Skip).Take(searchRequest.Take)];
            return this.Ok(new TicketSearchResponseDTO
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [AuthorizeRoles(Roles.Teacher | Roles.FacilityManager | Roles.Admin)]
        public async Task<ActionResult<Ticket>> Post(
            [FromServices] TicketRepository ticketRepo,
            [FromServices] StateRepository stateRepo,
            [FromServices] PriorityRepository priorityRepo,
            [FromServices] BuildingRepository buildingRepo,
            [FromBody] TicketPostDTO ticketData)
        {
            var currentUser = this.User;
            var userRole = this.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Enum.TryParse(userRole, out Roles role))
                return this.Unauthorized();

            var ticketState = await stateRepo.GetAsync(ticketData.StateId);

            if (ticketState is null)
                return this.BadRequest("The ticket to be created must have a valid state.");

            var defaultState = stateRepo.Find(state => state.IsDefault).FirstOrDefault();

            // Blocks users from creating tickets with non-default states if there is a default state
            if (!role.HasFlag(Roles.FacilityManager)
                && !role.HasFlag(Roles.Admin)
                && !ticketState.IsDefault
                && defaultState is not null)
                return this.Forbid();

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

            var author = currentUser.FindFirst("username")?.Value;

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
            var mailRecipients = MailRecipient.GetCollectionFromUsers(userRepository.GetMany(mailRecipientIds));

            await mailingService.SendManyLocalized(
                mailRecipients,
                (localizer) => MailGenerator.GenerateNewTicketCommentMail(comment, localizer),
                (localizer) => localizer.Localize($"New comment on ticket '{ticket.Title}'"));

            return this.Ok(comment);
        }

        /// <summary>
        /// Adds a new attachment entity of a specific ticket.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="attachmentRepo">Instance of <see cref="TicketAttachmentRepository"/>.</param>
        /// <param name="mailingService">Instance of <see cref="MailingService"/></param>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <param name="configuration">Instance of <see cref="IConfiguration"/></param>
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
            [FromServices] IConfiguration configuration,
            [FromRoute] Guid ticketId,
            IFormFile attachmentFile)
        {
            Ticket? ticket = await ticketRepo.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound($"A ticket with the id '{ticketId}' doesn´t exist.");

            using MemoryStream stream = new();
            await attachmentFile.CopyToAsync(stream);

            string? blurHash = null;
            string mimeType = attachmentFile.ContentType;
            string unrenderableMimeTypes = configuration.GetValue<string>("Images:UnrenderableMimeTypes") ?? string.Empty;

            if (mimeType.StartsWith("image/") && !unrenderableMimeTypes.Contains(mimeType))
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
                IsRenderableImage = blurHash is not null,
            };

            await attachmentRepo.AddAsync(attachment);

            string performingUser = this.User.FindFirstValue("username") ?? string.Empty;
            var mailRecipientIds = ticketRepo.GetInvolvedUsersIds(ticket, [performingUser]);
            var mailRecipients = MailRecipient.GetCollectionFromUsers(userRepository.GetMany(mailRecipientIds));

            await mailingService.SendManyLocalized(
                mailRecipients,
                (localizer) => MailGenerator.GenerateNewTicketAttachmentMail(attachment, localizer),
                (localizer) => localizer.Localize($"New attachment on ticket '{ticket.Title}'"));

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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
            var userClaim = this.User.FindFirst("username")?.Value;
            var rolesClaim = this.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userClaim is null || rolesClaim is null)
                return this.Unauthorized();

            if (!Enum.TryParse(rolesClaim, out Roles role))
                return this.Unauthorized();

            var ticket = await ticketRepo.GetAsync(ticketData.Id);

            if (ticket is null)
                return this.NotFound($"A ticket with the id '{ticketData.Id}' doesn´t exist.");

            // Only the author, facility manager and admin can change tickets
            if (!ticket.Author.Equals(userClaim)
                && !role.HasFlag(Roles.FacilityManager)
                && !role.HasFlag(Roles.Admin))
                return this.Forbid();

            State ticketState = ticket.State;
            State? defaultState = stateRepo.Find(state => state.IsDefault).FirstOrDefault();

            // Restrict users from changing the state if there is a default state and the user is not a facility manager or admin
            if (!role.HasFlag(Roles.FacilityManager)
                && !role.HasFlag(Roles.Admin)
                && defaultState is not null
                && !ticket.State.Id.Equals(ticketData.StateId))
                return this.Forbid();

            State? state = await stateRepo.GetAsync(ticketData.StateId);

            Building? building = ticketData.BuildingId is null
                ? null
                : await buildingRepo.GetAsync(ticketData.BuildingId.Value);

            Priority? priority = await priorityRepo.GetAsync(ticketData.PriorityId);

            var oldTicket = ticket with { };

            ticket.Title = ticketData.Title ?? ticket.Title;
            ticket.Description = ticketData.Description ?? ticket.Description;
            ticket.State = state ?? ticket.State;
            ticket.LastStateUpdate = !ticketState.Id.Equals(ticket.State.Id)
                ? DateTime.UtcNow
                : ticket.LastStateUpdate.ToUniversalTime();
            ticket.Building = building ?? ticket.Building;
            ticket.Priority = priority ?? ticket.Priority;
            ticket.Object = ticketData.Object ?? ticket.Object;
            ticket.Room = ticketData.Room ?? ticket.Room;

            if (ticket.Equals(oldTicket))
                return this.NoContent();

            await ticketRepo.UpdateAsync(ticket);

            var mailRecipientIds = ticketRepo.GetInvolvedUsersIds(ticket, [userClaim]);
            var mailRecipients = MailRecipient.GetCollectionFromUsers(userRepository.GetMany(mailRecipientIds));

            await mailingService.SendManyLocalized(
                mailRecipients,
                (localizer) => MailGenerator.GenerateTicketChangedMail(ticket, oldTicket, localizer),
                (localizer) => localizer.Localize($"Ticket '{ticket.Title}' was changed"));

            return this.Ok(ticket);
        }

        /// <summary>
        /// Deletes the ticket with the specific id.
        /// </summary>
        /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/>.</param>
        /// <param name="ticketId">The specific id of the ticket that will be deleted.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete("{ticketId}")]
        [AuthorizeRoles(Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] TicketRepository ticketRepo,
            [FromRoute] Guid ticketId)
        {
            var ticket = await ticketRepo.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound($"A ticket with the id '{ticketId}' doesn´t exist.");

            await ticketRepo.DeleteAsync(ticket);

            return this.Ok($"The ticket with the id '{ticketId}' was deleted.");
        }
    }
}