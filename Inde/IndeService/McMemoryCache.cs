using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndeService;
public class McMemoryCache<TItem> : IMcMemoryCache<TItem>
{
    private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();


    public async Task<TItem> Get(object key)
    {
        TItem cacheEntry;

        if (!_cache.TryGetValue(key, out cacheEntry))// Look for cache key.
        {
            return default(TItem);
            //SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            //await mylock.WaitAsync();
            //try
            //{
            //    if (!_cache.TryGetValue(key, out cacheEntry))
            //    {
            //        // Key not in cache, so get data.
            //        cacheEntry = await createItem();
            //        _cache.Set(key, cacheEntry);
            //    }
            //}
            //finally
            //{
            //    mylock.Release();
            //}
        }
        return cacheEntry;
    }

    public async Task<TItem> Set(object key, TItem item)
    {
        TItem cacheEntry;

        if (!_cache.TryGetValue(key, out cacheEntry))// Look for cache key.
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {

                    // Key not in cache, so get data.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSize(1)                                        //Size amount
                        .SetPriority(CacheItemPriority.High)               //Priority on removing when reaching size limit (memory pressure)
                        .SetSlidingExpiration(TimeSpan.FromSeconds(120))   // Keep in cache for this time, reset time if accessed.
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(600)); // Remove from cache after this time, regardless of sliding expiration

                    // Save data in cache.
                    _cache.Set(key, item, cacheEntryOptions);
                }
            }
            finally
            {
                mylock.Release();
            }
        }
        return cacheEntry;
    }
}
