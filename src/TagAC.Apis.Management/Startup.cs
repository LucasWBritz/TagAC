using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using TagAC.Management.Data.EFCore.Context;
using TagAC.Management.Data.EFCore.Repositories.Entities;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Apis.Management
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
            services.AddScoped<ManagementDBContext>();

            services.AddDbContext<ManagementDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TagAC.Apis.Management", Version = "v1" });
            });

            services.AddScoped<IAccessCredentialsRepository, AccessCredentialsRepository>();
            services.AddScoped<ISmartLockRepository, SmartLockRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            RunDatabaseMigrations(app, logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TagAC.Apis.Management v1"));
            }

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
                    var context = serviceScope.ServiceProvider.GetRequiredService<ManagementDBContext>();
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
