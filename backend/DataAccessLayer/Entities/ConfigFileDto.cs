namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the configuration file to connect to LDAP.
    /// </summary>
    public class ConfigFileDto
    {
        /// <summary>
        /// The json web token key.
        /// </summary>
        public required string JWTKey { get; set; }

        /// <summary>
        /// The default connection
        /// </summary>
        public required string DefaultConnection { get; set; }

        /// <summary>
        /// The uri of LDAP as a <see langword="string"/>.
        /// </summary>
        public required string LDAPUri { get; set; }

        /// <summary>
        /// The distinguished name of the user.
        /// </summary>
        public required string LDAPNBindDN { get; set; }

        /// <summary>
        /// The password for the LDAP connection.
        /// </summary>
        public required string LDAPPassword { get; set; }

        /// <summary>
        /// The orginazional units that will ignored.
        /// </summary>
        public string? LDAPIgnoredOU { get; set; }
    }
}
