using AcmeCaseAPI.Application;
using AcmeCaseAPI.Infrastructure.Extensions;
using AcmeCaseAPI.Persistence;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using NSwag.AspNetCore;

namespace AcmeCaseAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true; // If you need to see the full information present in exceptions, set to "true" (should consider switching to false in prod)

            services.AddRateLimiting(Configuration); // Rate limiting (throttle) for the API

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddSecurity(Configuration); // Authentication and Authorization options

            services.AddScoped<IDbConnect>(c => new DbConnect(Configuration.GetConnectionString("DbConnectionString"))); // DB connect connection string

            services.AddApplicationLayerDependencies();

            services.AddVersioning(); // API versioning options

            services.AddSwaggerDocumentation(Configuration); // Swagger documentation options

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIpRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.OAuth2Client = new OAuth2ClientSettings
                {

                    ClientId = "4b3929f6-6fc2-45c5-b397-3e2b6e0ad9a3",
                    ClientSecret = null,
                    AppName = "Swagger UI",
                    UsePkceWithAuthorizationCodeGrant = true,

                };

                settings.Path = "/docs";
                settings.CustomStylesheetPath = "/css/swagger-ui/custom.css";
                settings.CustomJavaScriptPath = "/js/swagger-ui/custom.js";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
