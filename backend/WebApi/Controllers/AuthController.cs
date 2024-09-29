using DomainLayer.BusinessLogic.Authentication;
using DomainLayer.BusinessLogic.Authentication.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;

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
        /// <returns>A token pair if authentication succeeds</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(
            [FromBody] LoginRequestDto loginRequestDto,
            [FromServices] AuthenticationService authenticationService)
        {
            try
            {
                RoleAssignment roleAssignment = await authenticationService.LdapLogin(
                    loginRequestDto.Username,
                    loginRequestDto.Password);

                var accessToken = authenticationService.CreateAccessToken(roleAssignment);
                var refreshToken = await authenticationService.CreateRefreshToken(roleAssignment);

                return this.Ok(new LoginResponseDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
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
        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponseDto>> Refresh(
            [FromBody] RefreshRequestDto refreshRequestDto,
            [FromServices] AuthenticationService authenticationService)
        {
            try
            {
                RoleAssignment roleAssignment = await authenticationService.VerifyRefreshToken(refreshRequestDto.RefreshToken);

                var accessToken = authenticationService.CreateAccessToken(roleAssignment);
                var refreshToken = await authenticationService.CreateRefreshToken(roleAssignment);

                return this.Ok(new LoginResponseDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
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
        [HttpGet("verify-token")]
        public ActionResult VerifyToken()
        {
            return this.NoContent();
        }
    }
}