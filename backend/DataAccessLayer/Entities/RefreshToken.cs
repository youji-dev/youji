using System.ComponentModel.DataAnnotations;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents RefreshTokens
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Id of the Token
        /// </summary>
        [Key]
        public required Guid Id { get; set; }

        /// <summary>
        /// Token itself
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// UserId that the token belongs to
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        /// CreationDateTime where the Token was initially created
        /// </summary>
        public required DateTime CreationDateTime { get; set; } = DateTime.Now;
    }
}