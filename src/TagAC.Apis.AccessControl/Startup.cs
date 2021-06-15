using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TagAC.Apis.AccessControl.BackgroundServices;
using TagAC.Apis.AccessControl.Configurations;
using TagAC.Apis.AccessControl.Middlewares;
using TagAC.Apis.AccessControl.Repositories;
using TagAC.Apis.AccessControl.Services;
using TagAC.Apis.AccessControl.Sessions;
using TagAC.MessageBus;

namespace TagAC.Apis.AccessControl
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TagAC.Apis.AccessControl", Version = "v1" });
            });            

            services.ConfigureCaching(Configuration);

            services.AddScoped<IHeaderParametersSession, HeaderParametersSession>();
            services.AddScoped<IAccessControlService, AccessControlService>();
            services.AddScoped<ICacheRepository, CacheRepository>();


            services.AddMessageBus(Configuration.GetSection("MessageQueueConnection")["MessageBus"]);
            services.AddHostedService<AccessControlBackgroundService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {        
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TagAC.Apis.AccessControl v1"));
            }            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<HeaderParametersMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
