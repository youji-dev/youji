using System.ComponentModel.DataAnnotations;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the configuration file
    /// </summary>
    public class ConfigFileDto
    {
        /// <summary>
        /// The json web token key.
        /// </summary>
        public required string JWTKey { get; set; }

        /// <summary>
        /// The database connection
        /// </summary>
        public required string DefaultConnection { get; set; }

        /// <summary>
        /// The uri of LDAP instance as a <see langword="string"/>.
        /// </summary>
        public required string LDAPUri { get; set; }

        /// <summary>
        /// The BaseDN for all groups
        /// </summary>
        public required string LDAPBaseDN { get; set; }

        /// <summary>
        /// Lifetime of maximum lifetime of a user session in minutes
        /// </summary>
        public int? SessionLifeTime { get; set; }
    }
}
