using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TagAC.Apis.AccessControl.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;
        public CacheRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetValue(string key, CancellationToken cancelationToken)
        {
            return await _cache.GetStringAsync(key, cancelationToken);
        }

        public async Task SetValue(string key, string value, CancellationToken cancellationToken)
        {
            await _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromDays(7),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
            }, cancellationToken);
        }
    }
}
