using System.Text.Json.Serialization;

namespace Common
{
    /// <summary>
    /// Represents the CORS settings of the config file
    /// </summary>
    public class CorsConfigDto
    {
        /// <summary>
        /// The allowed origins for the CORS policy
        /// </summary>
        [JsonPropertyName("AllowedOrigins")]
        public required string[] AllowedOrigins { get; set; }

        /// <summary>
        /// The allowed headers for the CORS policy
        /// </summary>
        [JsonPropertyName("AllowedHeaders")]
        public required string[] AllowedHeaders { get; set; }

        /// <summary>
        /// The allowed methods for the CORS policy
        /// </summary>
        [JsonPropertyName("AllowedMethods")]
        public required string[] AllowedMethods { get; set; }
    }
}