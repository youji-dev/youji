using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Enums;
using Common.Extensions;
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
        UserRepository userRepository,
        RefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration)
    {
        /// <summary>
        /// Generate JWT AccessToken
        /// </summary>
        /// <param name="roleAssignment">User and Role that will be stored in the token</param>
        /// <returns>A short living JWT AccessToken</returns>
        public string CreateAccessToken(User roleAssignment)
        {
            var jwtKey = configuration.GetValueOrThrow("JWTKey");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims:
                [
                    new Claim("username", roleAssignment.UserId),
                    new Claim(ClaimTypes.Role, roleAssignment.Type.ToString()),
                ],
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
        public async Task<string> CreateRefreshToken(User roleAssignment)
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
        /// <returns><see cref="User"/> if valid</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if provided refresh token is invalid</exception>
        public async Task<User> VerifyRefreshToken(string token)
        {
            var sessionLifetime = int.Parse(configuration.GetValueOrThrow("SessionLifeTime"));

            var refreshTokenEntry = await refreshTokenRepository.Find(x =>
                    x.Token == token && x.CreationDateTime.ToUniversalTime() >= DateTime.UtcNow.AddMinutes(0 - sessionLifetime))
                .FirstOrDefaultAsync();

            if (refreshTokenEntry is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            await refreshTokenRepository.DeleteAsync(refreshTokenEntry);

            return await this.GetOrCreateUser(refreshTokenEntry.UserId);
        }

        /// <summary>
        /// Method used for login in with LDAP
        /// </summary>
        /// <param name="username">LDAP-DN pointing to a single Entity</param>
        /// <param name="password">Password for the entity</param>
        /// <returns>A <see cref="User"/> that is either queries from the database or
        /// create if the user logs in for the first time</returns>
        public async Task<User> LdapLogin(string username, string password)
        {
            bool dev = bool.Parse(configuration["DevAuth"] ?? "false");

            if (dev)
            {
                var user = await this.GetOrCreateUser(username.ToLowerInvariant());
                user.Type = Roles.Admin;
                return user;
            }

            var host = configuration.GetValueOrThrow("LDAPHost");
            var port = int.Parse(configuration.GetValueOrThrow("LDAPPort"));
            var baseDn = configuration.GetValueOrThrow("LDAPBaseDN");

            var connection = new LdapConnection();
            try
            {
                connection.Connect(host, port);
                await connection.BindAsync(
                    Native.LdapAuthMechanism.SIMPLE,
                    $"CN={username},{baseDn}",
                    password);

                SearchResponse emailSearchResponse = (SearchResponse)connection.SendRequest(
                    new SearchRequest(
                        baseDn,
                        $"(&(cn={username}))",
                        Native.LdapSearchScope.LDAP_SCOPE_SUBTREE,
                        ["userPrincipalName"]));

                string? email = null;
                try
                {
                    email = emailSearchResponse.Entries
                        .SingleOrDefault()?
                        .Attributes["userPrincipalName"]
                        .GetValues<string?>()
                        .SingleOrDefault();
                }
                catch (KeyNotFoundException)
                {
                }

                return await this.GetOrCreateUser(username.ToLowerInvariant(), email);
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

        private async Task<User> GetOrCreateUser(string username, string? email = null)
        {
            var user = await userRepository
                .Find(x => x.UserId.Equals(username))
                .FirstOrDefaultAsync();

            if (user is not null)
            {
                user.Email = email is not null ? email : user.Email;
                await userRepository.UpdateAsync(user);

                return user;
            }

            var newUser = new User
            {
                UserId = username.ToLowerInvariant(),
                Type = Roles.Teacher,
                Email = email,
            };

            await userRepository.AddAsync(newUser);
            return newUser;
        }
    }
}