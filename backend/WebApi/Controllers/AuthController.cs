using Application.WebApi.Contracts.Request;
using Common.Enums;
using DomainLayer.BusinessLogic.Authentication;
using DomainLayer.BusinessLogic.Authentication.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Controller used for authentication
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Route used to exchange login credentials for an access and refresh token pair
        /// </summary>
        /// <param name="loginRequestDto">Credentials provided by user</param>
        /// <param name="authenticationService">Instance of <see cref="AuthenticationService"/></param>
        /// <param name="userRepository">Instance of <see cref="UserRepository"/></param>
        /// <returns>A token pair if authentication succeeds</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login(
            [FromBody] LoginRequestDto loginRequestDto,
            [FromServices] AuthenticationService authenticationService,
            [FromServices] UserRepository userRepository)
        {
            try
            {
                User roleAssignment = await authenticationService.LdapLogin(
                    loginRequestDto.Username,
                    loginRequestDto.Password);
                var accessToken = authenticationService.CreateAccessToken(roleAssignment);
                var refreshToken = await authenticationService.CreateRefreshToken(roleAssignment);
                var doesAdminUserExist = userRepository.GetAll().Any(user => user.Type.HasFlag(Roles.Admin));
                return this.Ok(new LoginResponseDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    IsSystemReady = doesAdminUserExist,
                });
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Route used to refresh a session by exchanging a refresh token for a new token pair
        /// </summary>
        /// <param name="refreshRequestDto">Refresh token provided by client</param>
        /// <param name="authenticationService">Instance of <see cref="AuthenticationService"/></param>
        /// <returns>A token pair if refresh token validation succeeds</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Refresh")]
        public async Task<ActionResult<LoginResponseDto>> Refresh(
            [FromBody] RefreshRequestDto refreshRequestDto,
            [FromServices] AuthenticationService authenticationService)
        {
            try
            {
                User roleAssignment = await authenticationService.VerifyRefreshToken(refreshRequestDto.RefreshToken);

                var accessToken = authenticationService.CreateAccessToken(roleAssignment);
                var refreshToken = await authenticationService.CreateRefreshToken(roleAssignment);

                return this.Ok(new LoginResponseDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    IsSystemReady = true,
                });
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Route used to verify if the accessToken is valid
        /// </summary>
        /// <returns>204 no content</returns>
        [Authorize]
        [HttpGet("VerifyToken")]
        public ActionResult VerifyToken()
        {
            return this.NoContent();
        }

        /// <summary>
        /// Route used to promote a user to admin if there is no admin user
        /// </summary>
        /// <param name="promotionUserRequestDto">Instance of <see cref="PromotionUserRequestDto"/></param>
        /// <param name="systemSetupService">Instance of <see cref="SystemSetupService"/></param>
        /// <returns>204 no content if promotion is successful</returns>
        [Authorize]
        [HttpPost("PromoteToAdmin")]
        public async Task<ActionResult> PromoteUser(
            [FromBody] PromotionUserRequestDto promotionUserRequestDto,
            [FromServices] SystemSetupService systemSetupService)
        {
            var userClaim = this.User.FindFirst("username")?.Value;
            if (string.IsNullOrEmpty(userClaim))
            {
                return this.Unauthorized();
            }

            if (string.IsNullOrEmpty(promotionUserRequestDto.PromotionToken))
            {
                return this.BadRequest("Promotion token is required");
            }

            await systemSetupService.PromoteToAdmin(userClaim, promotionUserRequestDto.PromotionToken);
            return this.NoContent();
        }
    }
}