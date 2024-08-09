using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersistenceLayer.DataAccess;

namespace PersistenceLayer.DataAccess
{
    /// <summary>
    /// Provides costum add functions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Adds infrastructure with db context.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <param name="connectionString">Connection string of the database.</param>
        public static void AddInfrastructure(this IServiceCollection services, string? connectionString)
        {
            ArgumentNullException.ThrowIfNull(connectionString);

            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }

        /// <summary>
        /// Adds repositories to DI.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        public static void AddRepositories(this IServiceCollection services)
        {
        }
    }
}
