using Microsoft.Extensions.Configuration;

namespace Common.Extensions
{
    /// <summary>
    /// Provides helper methods for interacting with the app configuration
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Returns the requested key or throws if it wasn't found
        /// </summary>
        /// <param name="configuration">The configuration object to use</param>
        /// <param name="key">The key to search for</param>
        /// <param name="sections">The sections the value should be found under</param>
        /// <returns>The configuration value if it was found</returns>
        /// <exception cref="InvalidOperationException">Thrown when no value was found for the key under the given sections</exception>
        public static string GetValueOrThrow(this IConfiguration configuration, string key, string[]? sections = null)
        {
            sections ??= [];

            string? result;

            if (sections.Length == 0)
            {
                result = configuration[key];
            }
            else
            {
                IConfigurationSection section = configuration.GetSection(sections[0]);

                foreach (var requestedSection in sections.Skip(1))
                {
                    section = section.GetSection(requestedSection);
                }

                result = section[key];
            }

            return result ?? throw new InvalidOperationException($"No value configured for '{string.Join(":", sections)}:{key}'");
        }
    }
}
