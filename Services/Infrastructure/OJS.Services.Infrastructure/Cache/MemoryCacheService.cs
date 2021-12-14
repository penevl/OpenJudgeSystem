﻿namespace OJS.Services.Infrastructure.Cache
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;
    using System;

    public class MemoryCacheService : ICacheService
    {
        // public T Get<T>(string cacheId, Func<T> getItemCallback, DateTime absoluteExpiration) where T : class
        // {
        //     if (HttpRuntime.Cache.Get(cacheId) is T item)
        //     {
        //         return item;
        //     }
        //
        //     item = getItemCallback();
        //
        //     SetCache(cacheId, item, absoluteExpiration);
        //
        //     return item;
        // }
        //
        // public T Get<T>(string cacheId, Func<T> getItemCallback, int? cacheSeconds)
        //     where T : class
        // {
        //     var absoluteExpiration = cacheSeconds.HasValue
        //         ? DateTime.Now.AddSeconds(cacheSeconds.Value)
        //         : Cache.NoAbsoluteExpiration;
        //
        //     return this.Get(cacheId, getItemCallback, absoluteExpiration);
        // }
        //
        // public void Remove(string cacheId) => HttpRuntime.Cache.Remove(cacheId);
        //
        // private static void SetCache<T>(string cacheId, T item, DateTime absoluteExpiration)
        //     => HttpContext.Current.Cache.Add(
        //         cacheId,
        //         item,
        //         null,
        //         absoluteExpiration,
        //         Cache.NoSlidingExpiration,
        //         CacheItemPriority.Default,
        //         null);
        public T Get<T>(string cacheId, Func<T> getItemCallback, DateTime absoluteExpiration) where T : class => throw new NotImplementedException();

        public T Get<T>(string cacheId, Func<T> getItemCallback, int? cacheSeconds) where T : class => throw new NotImplementedException();

        public void Remove(string cacheId) => throw new NotImplementedException();
    }
}