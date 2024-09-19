namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the configuration file
    /// </summary>
    public class ConfigFileDto
    {
        /// <summary>
        /// The database connection
        /// </summary>
        public required string DefaultConnection { get; set; }

        /// <summary>
        /// The json web token key.
        /// </summary>
        public required string JWTKey { get; set; }

        /// <summary>
        /// Lifetime of maximum lifetime of a user session in minutes
        /// </summary>
        public int? SessionLifeTime { get; set; }

        /// <summary>
        /// The uri of LDAP instance.
        /// </summary>
        public required string LDAPHost { get; set; }

        /// <summary>
        /// The port of the LDAP instance
        /// </summary>
        public required int LDAPPort { get; set; }

        /// <summary>
        /// The BaseDN for the user entities that will be able to authenticate
        /// </summary>
        public required string LDAPBaseDN { get; set; }
    }
}
