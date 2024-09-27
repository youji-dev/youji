using System.Security.Claims;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.WebApi.Decorators
{
    /// <summary>
    /// <para>Decorator that can be applied to controllers and controller methods</para>
    /// <para>Checks if the incoming request fulfills the authentication and has the required roles</para>
    /// <param name="roles">The roles that are required to gain access to the resource</param>
    /// </summary>
    public class AuthorizeRolesAttribute(Roles roles) : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <inheritdoc/>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userClaims = context.HttpContext.User.Claims;
            var rolesClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (rolesClaim == null || !Enum.TryParse(rolesClaim.Value, out Roles userRoles))
            {
                context.Result = new ForbidResult();
                return;
            }

            if ((roles & userRoles) != roles)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}