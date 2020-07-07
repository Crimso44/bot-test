using System;
using Microsoft.Extensions.Caching.Memory;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.CommonServices.Const;

namespace ChatBot.Admin.CommonServices.Services
{
    internal class TempCache : ITempCache
    {
        private readonly IMemoryCache _cache;

        public TempCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public TItem Get<TKey, TItem>(TKey key)
        {
            return (TItem)_cache.Get(key);
        }

        public void Set<TKey, TItem>(TKey key, TItem item)
        {
            _cache.Set(key, item, TimeSpan.FromMinutes(TempCacheConst.AbsoluteExpirationMinutes));
        }

        public void Remove<TKey>(TKey key)
        {
            _cache.Remove(key);
        }
    }
}
