using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TagAC.Apis.AccessControl.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheRepository> _logger;
        public CacheRepository(IDistributedCache cache, ILogger<CacheRepository> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> GetValue(string key, CancellationToken cancelationToken)
        {
            return await _cache.GetStringAsync(key, cancelationToken);
        }

        public async Task SetValue(string key, string value, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(key) || key == ":")
            {
                _logger.LogError($"Invalid cache key. Value={key}");
                return;
            }

            await _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromDays(7),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
            }, cancellationToken);
        }
    }
}
