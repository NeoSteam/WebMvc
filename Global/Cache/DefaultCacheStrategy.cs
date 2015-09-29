using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Web.Caching;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Threading;

namespace Global
{
    /// <summary>
    /// 默认缓存管理类
    /// </summary>   
    public class DefaultCacheStrategy : ICacheStrategy
    {
        private static readonly DefaultCacheStrategy instance = new DefaultCacheStrategy();

        /// <summary>
        /// 缓存对象 
        /// </summary>
        protected static volatile System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;

        /// <summary>
        /// 缓存时间 
        /// </summary>
        protected int defaultTimeOut = 1440; // 默认缓存存活期为1440分钟(24小时)

        private static readonly string files = AppDomain.CurrentDomain.BaseDirectory + @"\config\ClearList.xml";

        /// <summary>
        /// 构造函数
        /// </summary>
        static DefaultCacheStrategy()
        {
            if (files != null && files.Length != 0 && File.Exists(files))
            {
                CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(cacheRemove);

                CacheDependency dep = new CacheDependency(files, DateTime.Now);

                webCache.Insert(files, cacheClearList(files), dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 设置到期相对时间[单位：／分钟] 
        /// </summary>
        public int TimeOut
        {
            set { defaultTimeOut = value > 0 ? value : 1440; }
            get { return defaultTimeOut > 0 ? defaultTimeOut : 1440; }
        }
        /// <summary>
        /// 获得Cache对象
        /// </summary> 
        public static System.Web.Caching.Cache GetWebCacheObj
        {
            get { return webCache; }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public void AddObject(string objId, object o)
        {
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, null, DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>        
        public void AddObject<T>(string objId, T o)
        {
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, null, DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public void AddObjectByHour(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            webCache.Insert(objId, o, null, System.DateTime.Now.AddHours(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
        }


        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="files">监视的路径文件集</param>
        public void AddObjectWithFileChange(string objId, object o, string[] files)
        {
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            CacheDependency dep = new CacheDependency(files, DateTime.Now);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }


        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="fileName">监视的路径文件</param>
        public void AddObjectWithFileChange(string objId, object o, string fileName)
        {
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            CacheDependency dep = new CacheDependency(fileName, DateTime.Now);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="fileName">监视的路径文件</param>
        /// <param name="time">缓存过期时间</param>
        public void AddObjectWithFileChange(string objId, object o, string fileName, double time)
        {
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            CacheDependency dep = new CacheDependency(fileName, DateTime.Now);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddSeconds(time), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="fileName">监视的路径文件</param>
        /// <param name="callBack">回调委托</param>
        public void AddObjectWithFileChange(string objId, object o, string fileName, CacheItemRemovedCallback callBack)
        {

            CacheDependency dep = new CacheDependency(fileName, DateTime.Now);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }
        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="files">监视的路径文件集</param>
        public void AddObjectWithFileChange<T>(string objId, T o, string[] files)
        {
            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            CacheDependency dep = new CacheDependency(files, DateTime.Now);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }


        /// <summary>
        /// 加入当前对象到缓存中,并使用依赖键
        /// </summary>
        /// <typeparam name="T">参数类型为泛型</typeparam>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">对象为二维数组</param>
        /// <param name="dependKey">依赖关联的键值</param>
        public void AddObjectWithDepend<T>(string objId, T o, string[] dependKey)
        {
            if (string.IsNullOrEmpty(objId) || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            CacheDependency dep = new CacheDependency(null, dependKey, DateTime.Now);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中,并使用依赖键
        /// </summary>
        /// <typeparam name="T">参数类型为泛型</typeparam>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">对象为二维数组</param>
        /// <param name="dependKey">依赖关联的键值</param>
        public void AddObjectWithDepend<T>(string objId, T[] o, string[] dependKey)
        {
            if (string.IsNullOrEmpty(objId) || o == null || o.Length == 0)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            CacheDependency dep = new CacheDependency(null, dependKey, DateTime.Now);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中,并使用依赖键
        /// </summary>
        /// <typeparam name="T">参数类型为泛型</typeparam>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">对象为二维数组</param>
        /// <param name="dependKey">依赖关联的键值</param>
        public void AddObjectWithDepend<T>(string objId, T[,] o, string[] dependKey)
        {
            if (string.IsNullOrEmpty(objId) || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

            CacheDependency dep = new CacheDependency(null, dependKey, DateTime.Now);

            if (TimeOut == 1440)
            {
                webCache.Insert(objId, o, dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }


        /// <summary>
        /// 建立回调委托的一个实例
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="reason"></param>
        private void onRemove(string key, object val, CacheItemRemovedReason reason)
        {
        }


        /// <summary>
        /// 建立回调委托的一个实例
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="reason"></param>
        private static void cacheRemove(string key, object val, CacheItemRemovedReason reason)
        {
            switch (reason)
            {
                case CacheItemRemovedReason.DependencyChanged:
                    {
                        CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(cacheRemove);
                        Thread.Sleep(50);
                        CacheDependency dep = new CacheDependency(files, DateTime.Now);
                        webCache.Insert(files, cacheClearList(files), dep, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
                        break;
                    }
                case CacheItemRemovedReason.Expired:
                    {
                        break;
                    }
                case CacheItemRemovedReason.Removed:
                    {
                        break;
                    }
                case CacheItemRemovedReason.Underused:
                    {
                        break;
                    }
                default: break;
            }
        }

        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        public void RemoveObject(string objId)
        {
            webCache.Remove(objId);
        }


        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public object RetrieveObject(string objId)
        {
            return webCache.Get(objId);
        }


        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <typeparam name="T">缓存对象类类型</typeparam>
        /// <param name="objId">对象的关键字</param>
        /// <param name="value">指定类型的对象,默认Default(T)</param>
        /// <returns>是否返回了指定的类型对象</returns>
        public bool RetrieveObject<T>(string objId, out T value)
        {
            try
            {
                if (!Exists(objId))
                {
                    value = default(T);
                    return false;
                }

                value = (T)webCache.Get(objId);
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }


        /// <summary>
        /// 检查对象是否在缓存中
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns></returns>
        public bool Exists(string objId)
        {
            return webCache.Get(objId) != null;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>    
        public void Clear()
        {
            webCache.Clear();
        }

        /// <summary>
        /// 列显缓存键值信息
        /// </summary> 
        public string[] List()
        {
            return webCache.List();
        }

        /// <summary>
        /// 获取配置文件中缓存数组
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private static string[] cacheClearList(string fileName)
        {
            List<string> cacheList = new List<string>();

            XmlTextReader xmlReader = new XmlTextReader(fileName);

            try
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Text)
                    {
                        cacheList.Add(xmlReader.Value);
                    }
                }
                for (int i = 0; i < cacheList.Count; i++)
                {
                    webCache.Remove(cacheList[i]);
                }
                return cacheList.ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return new string[0];
            }

            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }
    }

}
