using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace MVCPagedList.App.Data.Helpers
{
    public static class CacheHelper
    {
        public static void UpdateCache(string key, object value)
        {
            HttpRuntime.Cache.Remove(key);
            PutCache(key, value);
        }
        public static void PutCache(string key, object value)
        {
            HttpRuntime.Cache.Add(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        public static T GetCachedData<T>(string key) where T : class
        {
            return HttpRuntime.Cache[key] as T;
        }
    }
}