using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersistenceLayer.DataAccess.Repositories;
using AuthenticationService = DomainLayer.BusinessLogic.Authentication.AuthenticationService;

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
            services.AddScoped<AuthenticationService>();

            services.AddScoped<RefreshTokenRepository>();
            services.AddScoped<RoleAssignmentRepository>();
        }

        /// <summary>
        /// Add Authentication that can be used with the Authorize decorator
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <param name="configurationManager">Instance of <see cref="ConfigurationManager"/>.</param>
        public static void AddAuthenticationConfiguration(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            string? jwtSignKey = configurationManager["JWTKey"];
            ArgumentException.ThrowIfNullOrEmpty(jwtSignKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "youji",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSignKey)),
                    };
                });
        }
    }
}
