using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Collections.Generic;

namespace AcmeCaseAPI.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adds authentication and authorization options to the services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {

            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration.GetSection("IdsSettings")["AuthorityServer"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            // Authorization policies that match the scopes of ApiResources of Identity Server in order to be used for authorization filtering e.g [Authorize("acmecase.scope")] for a controller
            services.AddAuthorization(options =>
            {
                options.AddPolicy("acmecase.scope",
                    policy => policy.RequireClaim("scope", "acmecase.scope")); // is set in Identity Server config
            });


            return services;
        }

        /// <summary>
        /// Adds API versioning options
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // Specify the default API Version as 1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                options.ReportApiVersions = true;

            })
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "VVV";
                options.SubstituteApiVersionInUrl = true;
            });



            return services;
        }

        /// <summary>
        /// Adds swagger documentation options
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1";
                document.ApiGroupNames = new[] { "1" };
                document.Title = "Acme Case API";
                document.Description = "Acme Case API Documentation";
                document.Version = "v1";


                document.AddSecurity("oauth2", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = configuration.GetSection("IdsSettings")["TokenUrl"],
                            Scopes = new Dictionary<string, string> { { "acmecase.scope", "swaggerUI access" } }
                        },

                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = configuration.GetSection("IdsSettings")["AuthorizationUrl"],
                            TokenUrl = configuration.GetSection("IdsSettings")["TokenUrl"],
                            Scopes = new Dictionary<string, string> { { "acmecase.scope", "swaggerUI access" } }

                        }
                    }
                });

                document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("oauth2"));

            });

            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v2";
                document.ApiGroupNames = new[] { "2" };
                document.Title = "Acme Case API";
                document.Description = "Acme Case API Documentation";
                document.Version = "v2";

                document.AddSecurity("oauth2", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = configuration.GetSection("IdsSettings")["AuthorizationUrl"],
                            TokenUrl = configuration.GetSection("IdsSettings")["TokenUrl"],
                            Scopes = new Dictionary<string, string> { { "acmecase.scope", "swaggerUI access" } }

                        }
                    }
                });

                document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("oauth2"));

            });

            return services;
        }


        /// <summary>
        /// Adds rate limiting (throttle) to the API
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            return services;
        }


    }
}
