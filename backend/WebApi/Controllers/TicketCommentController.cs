using System.Security.Claims;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints to manage ticket comments requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketCommentController : ControllerBase
    {
        /// <summary>
        /// Deletes the comment with the specific id.
        /// </summary>
        /// <param name="commentRepo">Instance of <see cref="TicketCommentRepository"/></param>
        /// <param name="commentId">The specific id of the comment.</param>
        /// <returns>An <see cref="ObjectResult"/> with a result message.</returns>
        [HttpDelete("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Delete(
            [FromServices] TicketCommentRepository commentRepo,
            [FromRoute] Guid commentId)
        {
            var userName = this.User.FindFirst("username")?.Value;
            var userRole = this.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Enum.TryParse(userRole, out Roles role))
                return this.Unauthorized();

            var comment = await commentRepo.GetAsync(commentId);

            if (comment is null)
                return this.NotFound($"A comment with the id '{commentId}' doesn´t exist.");

            bool isAuthor = comment.Author.Equals(userName);
            bool isAdmin = role.HasFlag(Roles.Admin);
            bool isAuthorized = isAdmin || isAuthor;

            if (!isAuthorized)
                return this.Forbid();

            await commentRepo.DeleteAsync(comment);

            return this.Ok($"The comment with the id '{commentId}' was deleted.");
        }
    }
}
