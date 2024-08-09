using Microsoft.OpenApi.Models;

namespace Application.WebApi
{
    /// <summary>
    /// Provides costum add functions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Adds swagger configuration.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        []
                    },
                });
            });
        }

        /// <summary>
        /// Adds logic services.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <param name="configurationManager">Instance of <see cref="ConfigurationManager"/>.</param>
        public static void AddLogicServices(this IServiceCollection services, ConfigurationManager configurationManager)
        {
        }
    }
}
