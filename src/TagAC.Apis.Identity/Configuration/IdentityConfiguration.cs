using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TagAC.Apis.Identity.Context;

namespace TagAC.Apis.Identity.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TagacIdentityDbContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>((options) =>
            {
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<TagacIdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddJwtConfiguration(configuration);

            return services;
        }
    }
}
