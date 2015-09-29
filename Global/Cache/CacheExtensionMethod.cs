using System;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;

namespace Global
{
    /// <summary>
    /// 扩充 System.Web.Caching 命名空间的 Extension Methods
    /// </summary>
    internal static class CacheExtensionMethod
    {
        /// <summary>
        /// 清除所有缓存信息
        /// </summary>
        /// <param name="x">缓存对象 </param>
        public static void Clear(this System.Web.Caching.Cache x)
        {
            List<string> cacheKeys = new List<string>();
            IDictionaryEnumerator cacheEnum = x.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cacheKeys.Add(cacheEnum.Key.ToString());
            }
            foreach (string cacheKey in cacheKeys)
            {
                x.Remove(cacheKey);
            }
        }

        /// <summary>
        /// 列显所有缓存键值信息
        /// </summary>
        /// <param name="x">缓存对象 </param>
        public static string[] List(this System.Web.Caching.Cache x)
        {
            List<string> cacheKeys = new List<string>();
            IDictionaryEnumerator cacheEnum = x.GetEnumerator();
            string[] cacheContent = new string[x.Count];
            int i = 0;
            while (cacheEnum.MoveNext())
            {
                cacheKeys.Add(cacheEnum.Key.ToString());
            }
            foreach (string cacheKey in cacheKeys)
            {
                cacheContent[i] = cacheKey;
                i++;
            }
            return cacheContent;
        }
    }
}