using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TagAC.Apis.AccessControl.Configurations
{
    public static class CacheConfiguration
    {
        public static IServiceCollection ConfigureCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheSettings = configuration.GetSection("CacheSettings");

            if(cacheSettings != null)
            {
                if (bool.Parse(cacheSettings["UseRedis"]))
                {
                    // Using Redis as cache.
                    services.AddStackExchangeRedisCache(options => options.Configuration = cacheSettings["ConnectionString"]);
                    
                    return services;
                }
            }
            
            // Otherwise we will use in memory cache. 
            services.AddDistributedMemoryCache();

            return services;
        }
    }
}
