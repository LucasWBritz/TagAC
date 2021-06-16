using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using TagAC.Apis.Identity.Configuration;
using TagAC.Apis.Identity.Context;
using TagAC.Apis.Identity.Services;
using TagAC.Logging;

namespace TagAC.Apis.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfiguration(Configuration);

            services.AddScoped<IAuthenticationService, AuthenticationService>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TagAC.Apis.Identity", Version = "v1" });
            });            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<Startup> logger)
        {
            RunDatabaseMigrations(app, logger);
            ApplicationDbInitializer.SeedAdminUser(userManager, roleManager).GetAwaiter().GetResult();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TagAC.Apis.Identity v1"));
            }
            
            app.ConfigureGlobalExceptionHandler(logger);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RunDatabaseMigrations(IApplicationBuilder app, ILogger<Startup> logger)
        {
            try
            {
                logger.LogInformation("Starting Migrations");
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<TagacIdentityDbContext>();
                    context.Database.Migrate();
                }

                logger.LogInformation("Migrations finished.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error running initial migrations. {ex.Message}");
            }
        }
    }
}
