using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
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
            string jwtKey = configuration["JWTKey"] ?? throw new InvalidOperationException();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim("username", roleAssignment.UserId),
                    new Claim("role", roleAssignment.Type.ToString()),
                },
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generate RefreshToken and saving it to the Database
        /// </summary>
        /// <param name="roleAssignment">User and role that the RefreshToken will be generated for</param>
        /// <returns>A RefreshToken</returns>
        public async string CreateRefreshToken(RoleAssignment roleAssignment)
        {
            var refreshToken = Guid.NewGuid().ToString();
            await refreshTokenRepository.AddAsync(new RefreshToken
            {
                Id = default(Guid),
                UserId = roleAssignment.UserId,
                Type = roleAssignment.Type,
                CreatedAt = DateTime.Now,
                Token = refreshToken,
                CreationDateTime = DateTime.Now
            });
            return refreshToken;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verify if a RefreshToken is valid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string VerifyRefreshToken(string token)
        {
            throw new NotImplementedException();
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
            string host = configuration["LDAPHost"] ?? throw new InvalidOperationException();
            int port = int.Parse(configuration["LDAPPort"] ?? throw new InvalidOperationException());
            string baseDn = configuration["LDAPBaseDN"] ?? throw new InvalidOperationException();

            var connection = new LdapConnection();

            try
            {
                connection.Connect(host, port);
                await connection.BindAsync(
                    Native.LdapAuthMechanism.SIMPLE,
                    $"CN={username},{baseDn}",
                    password);

                var assignmentList = await roleAssignmentRepository.Find(x => x.UserId == username.ToLowerInvariant()).ToListAsync();
                if (assignmentList.Count != 0)
                    return assignmentList[0];

                var roleAssignment = new RoleAssignment
                {
                    UserId = username.ToLowerInvariant(),
                    Type = 0,
                };
                await roleAssignmentRepository.AddAsync(roleAssignment);
                return roleAssignment;
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
    }
}