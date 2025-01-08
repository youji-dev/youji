using System.Reflection;

namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Assembly"/>
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Get a stream for an embedded resource of the given assembly
        /// </summary>
        /// <remarks>
        /// - resource name must include the file extension
        /// - to differentiate files with same name in different folders use namespaces (i.e. "Folder.Resource.txt")
        /// </remarks>
        /// <param name="assembly">The assembly the resource is embedded in</param>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns>A stream of the resource</returns>
        public static Stream? GetResource(this Assembly assembly, string resourceName)
        {
            string? logoResourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(resourceName));

            if (logoResourceName is null)
                return null;

            return assembly.GetManifestResourceStream(logoResourceName);
        }
    }
}
