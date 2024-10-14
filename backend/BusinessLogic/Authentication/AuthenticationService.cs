using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Enums;
using LdapForNet;
using LdapForNet.Native;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace DomainLayer.BusinessLogic.Authentication
{
    /// <summary>
    /// Service used for TokenHandling and LDAP Communication
    /// </summary>
    public class AuthenticationService(
        RoleAssignmentRepository roleAssignmentRepository,
        RefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration)
    {
        /// <summary>
        /// Generate JWT AccessToken
        /// </summary>
        /// <param name="roleAssignment">User and Role that will be stored in the token</param>
        /// <returns>A short living JWT AccessToken</returns>
        public string CreateAccessToken(RoleAssignment roleAssignment)
        {
            var jwtKey = configuration["JWTKey"] ?? throw new InvalidOperationException("JWT Key cant be empty");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim("username", roleAssignment.UserId),
                    new Claim(ClaimTypes.Role, roleAssignment.Type.ToString()),
                },
                expires: DateTime.UtcNow.AddMinutes(15),
                issuer: "youji",
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generate RefreshToken and saving it to the Database
        /// </summary>
        /// <param name="roleAssignment">User and role that the RefreshToken will be generated for</param>
        /// <returns>A RefreshToken</returns>
        public async Task<string> CreateRefreshToken(RoleAssignment roleAssignment)
        {
            var refreshToken = Guid.NewGuid().ToString();
            await refreshTokenRepository.AddAsync(new RefreshToken
            {
                Id = default,
                UserId = roleAssignment.UserId,
                Token = refreshToken,
                CreationDateTime = DateTime.UtcNow,
            });
            return refreshToken;
        }

        /// <summary>
        /// Verify if a RefreshToken is valid
        /// </summary>
        /// <param name="token">Token to be verified</param>
        /// <returns><see cref="RoleAssignment"/> if valid</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if provided refresh token is invalid</exception>
        public async Task<RoleAssignment> VerifyRefreshToken(string token)
        {
            var sessionLifetime = int.Parse(configuration["SessionLifeTime"] ??
                                            throw new InvalidOperationException("SessionLifeTime cant be empty"));

            var refreshTokenEntry = await refreshTokenRepository.Find(x =>
                    x.Token == token && x.CreationDateTime.ToUniversalTime() >= DateTime.UtcNow.AddMinutes(0 - sessionLifetime))
                .FirstOrDefaultAsync();

            if (refreshTokenEntry is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            await refreshTokenRepository.DeleteAsync(refreshTokenEntry);

            return await this.GetOrCreateRoleAssignment(refreshTokenEntry.UserId);
        }

        /// <summary>
        /// Method used for login in with LDAP
        /// </summary>
        /// <param name="username">LDAP-DN pointing to a single Entity</param>
        /// <param name="password">Password for the entity</param>
        /// <returns>A <see cref="RoleAssignment"/> that is either queries from the database or
        /// create if the user logs in for the first time</returns>
        public async Task<RoleAssignment> LdapLogin(string username, string password)
        {
            bool dev = bool.Parse(configuration["DevAuth"] ??
                          throw new InvalidOperationException("Dev auth was empty"));
            if (dev)
            {
                return await this.GetOrCreateRoleAssignment(username.ToLowerInvariant());
            }

            var host = configuration["LDAPHost"] ?? throw new InvalidOperationException("LDAPHost");
            var port = int.Parse(configuration["LDAPPort"] ?? throw new InvalidOperationException("LDAPPort"));
            var baseDn = configuration["LDAPBaseDN"] ?? throw new InvalidOperationException("LDAPBaseDN");
            var connection = new LdapConnection();
            try
            {
                connection.Connect(host, port);
                await connection.BindAsync(
                    Native.LdapAuthMechanism.SIMPLE,
                    $"CN={username},{baseDn}",
                    password);
                return await this.GetOrCreateRoleAssignment(username.ToLowerInvariant());
            }
            catch (LdapException ex)
            {
                throw new UnauthorizedAccessException("Invalid credentials", ex);
            }
            finally
            {
                connection.Dispose();
            }
        }

        private async Task<RoleAssignment> GetOrCreateRoleAssignment(string username)
        {
            var assignment = await roleAssignmentRepository
                .Find(x => x.UserId.Equals(username))
                .FirstOrDefaultAsync();

            if (assignment is not null)
                return assignment;

            var newAssignment = new RoleAssignment
            {
                UserId = username.ToLowerInvariant(),
                Type = (int)Roles.Teacher,
            };

            await roleAssignmentRepository.AddAsync(newAssignment);
            return newAssignment;
        }
    }
}