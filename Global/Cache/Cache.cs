using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Caching;

namespace Global
{
    /// <summary>缓存类</summary>
    /// <remarks>
    /// 	<para>此缓存类可以在.NET环境中存储任意对象</para>
    /// 	<para>此缓存是多线程安全的，在多线程操作中不影响性能</para>
    /// </remarks>
    /// <example>
    ///     本实例给出了如何应用此功能
    /// 	<code lang="CS" title="Cache objects">
    /// 	public class SampleCache
    /// 	{ 	
    ///   	public SampleCache()
    ///   	{
    ///     		Cache cache = Cache.GetCacheService();
    ///     		
    ///             添加缓存，建立文件依赖，文件名为_realPath
    ///     		cache.AddObject(VirtualPath, data, _realPath);
    ///     		添加缓存，不建立文件依赖
    ///             cache.AddObject(VirtualPath, data);
    ///             
    ///             获取缓存
    ///             byte[] data = (byte[])cache.RetrieveObject(FileName);
    ///             string stringData = cache.RetrieveStringObject(FileName);
    ///             string[] strData = cache.RetrieveArrayObject(FileName);
    ///             string[,] strArrData = cache.RetrieveArraysObject(FileName);
    ///             ...
    ///   	}     	
    ///   	...
    /// 	}
    ///     </code>
    /// </example>
    public class Cache
    {
        private static ICacheStrategy cs;
        private static volatile Cache instance = null;
        private static object lockHelper = new object();

        private Cache()
        {
            cs = new DefaultCacheStrategy();
        }


        /// <summary>
        /// 单体模式返回当前类的实例
        /// </summary>
        /// <returns></returns>
        public static Cache GetCacheService()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                    {
                        instance = new Cache();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 设置过期时间
        /// </summary>
        public int TimeOut
        {
            set
            {
                try
                {
                    cs.TimeOut = value > 0 ? value : 1440;
                }
                catch
                {
                    cs.TimeOut = 1440;
                }
            }
        }

        /// <summary>
        /// 添加字符串类型数据缓存
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="str">被缓存的字符串数据</param>
        public virtual void AddObject(string objectId, string str)
        {
            if (str != null && str.Length != 0)
            {
                AddObject(objectId, (object)str);
            }
        }


        /// <summary>
        /// 添加字符串类型数据缓存
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="str">被缓存的字符串数据</param>
        /// <param name="fileName">监视的路径文件</param>
        public virtual void AddObject(string objectId, string str, string fileName)
        {
            if (str != null && str.Length != 0)
            {
                AddObject(objectId, (object)str, fileName);
            }
        }

        /// <summary>
        /// 添加字符串类型数据缓存
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="str">被缓存的字符串数据</param>
        /// <param name="fileName">监视的路径文件</param>
        /// <param name="time">缓存有效时间</param>
        public virtual void AddObject(string objectId, string str, string fileName, double time)
        {
            if (str != null && str.Length != 0)
            {
                AddObject(objectId, (object)str, fileName, time);
            }
        }

        /// <summary>
        /// 添加字符串数组类型数据缓存
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="strArray">被缓存的字符串数组数据</param>
        public virtual void AddObject(string objectId, string[] strArray)
        {
            if (strArray != null && strArray.Length > 0)
            {
                AddObject(objectId, (object)strArray);
            }
        }

        /// <summary>
        /// 添加字符串数组类型数据缓存
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="strArray">被缓存的字符串数组数据</param>
        /// <param name="fileName">监视的路径文件</param>
        public virtual void AddObject(string objectId, string[] strArray, string fileName)
        {
            if (strArray != null && strArray.Length > 0)
            {
                AddObject(objectId, (object)strArray, fileName);
            }
        }

        /// <summary>
        /// 添加二维字符串数组类型数据缓存
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="strArrays">被缓存的二维字符串数组数据</param>
        public virtual void AddObject(string objectId, string[,] strArrays)
        {
            if (strArrays != null && strArrays.GetLength(0) > 0 && strArrays.GetLength(1) > 0)
            {
                AddObject(objectId, (object)strArrays);
            }
        }


        /// <summary>
        /// 添加二维字符串数组类型数据缓存
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="strArrays">被缓存的二维字符串数组数据</param>
        /// <param name="fileName">监视的路径文件</param>
        public virtual void AddObject(string objectId, string[,] strArrays, string fileName)
        {
            if (strArrays != null && strArrays.GetLength(0) > 0 && strArrays.GetLength(1) > 0)
            {
                AddObject(objectId, (object)strArrays, fileName);
            }
        }


        /// <summary>
        /// 添加数据缓存
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="o">被缓存的对象</param>
        public virtual void AddObject(string objectId, object o)
        {
            if (objectId != null && objectId.Length != 0 && o != null)
            {
                lock (lockHelper)
                {
                    //向缓存加入新的对象
                    cs.AddObject(objectId, o);
                }
            }
        }


        /// <summary>
        /// 加入当前对象信息
        /// </summary>
        /// <param name="objectId">缓存键值 </param>
        /// <param name="o">被缓存的对象</param>
        /// <param name="fileName">监视的路径文件</param>
        public virtual void AddObject(string objectId, object o, string fileName)
        {
            if (objectId != null && objectId.Length != 0 && o != null && fileName != null && fileName.Length != 0 && File.Exists(fileName))
            {
                lock (lockHelper)
                {
                    //向缓存加入新的对象
                    cs.AddObjectWithFileChange(objectId, o, fileName);
                }
            }
        }

        /// <summary>
        /// 加入当前对象信息
        /// </summary>
        /// <param name="objectId">缓存键值 </param>
        /// <param name="o">被缓存的对象</param>
        /// <param name="fileName">监视的路径文件</param>
        /// <param name="time">缓存的有效时间</param>
        public virtual void AddObject(string objectId, object o, string fileName, double time)
        {
            if (objectId != null && objectId.Length != 0 && o != null && time != null && time != 0)
            {
                lock (lockHelper)
                {
                    //向缓存加入新的对象
                    cs.AddObjectWithFileChange(objectId, o, fileName, time);
                }
            }
        }

        /// <summary>
        /// 加入当前对象信息
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <param name="o">被缓存的对象</param>
        /// <param name="fileName">监视的路径文件</param>
        /// <param name="callBack">回调的委托</param>'
        /// <remarks>
        /// add by mason 2009-09-11
        /// </remarks>
        public virtual void AddObject(string objectId, object o, string fileName, CacheItemRemovedCallback callBack)
        {
            if (objectId != null && objectId.Length != 0 && o != null && fileName != null && fileName.Length != 0 && File.Exists(fileName))
            {
                lock (lockHelper)
                {
                    //向缓存加入新的对象
                    cs.AddObjectWithFileChange(objectId, o, fileName, callBack);
                }
            }
        }

        public virtual void AddObject<T>(string objectId, T[] o, string[] dependencyKeys)
        {
            if ((!string.IsNullOrEmpty(objectId)) && (o != null) && (o.Length > 0) && (dependencyKeys != null) && (dependencyKeys.Length > 0))
            {
                lock (lockHelper)
                {
                    cs.AddObjectWithDepend<T>(objectId, o, dependencyKeys);
                }
            }
        }
        /// <summary>
        /// 返回缓存数据
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <returns>缓存对象</returns>
        public virtual object RetrieveObject(string objectId)
        {
            if (objectId != null && objectId.Length != 0)
            {
                return cs.RetrieveObject(objectId);
            }
            return null;
        }


        /// <summary>
        /// 返回缓存数据
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <returns>缓存字符串对象</returns>
        public virtual string RetrieveStringObject(string objectId)
        {
            //向缓存加入新的对象
            object cacheObject = RetrieveObject(objectId);
            if (cacheObject != null)
            {
                string otype = cacheObject.GetType().Name.ToString();
                //当缓存类型是字符串类型时
                if (otype == "String")
                {
                    if (cacheObject.ToString().Length == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return (string)cacheObject;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 返回缓存数据
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <returns>缓存字符串数组对象</returns>
        public virtual string[] RetrieveArrayObject(string objectId)
        {
            //向缓存加入新的对象
            object cacheObject = RetrieveObject(objectId);
            if (cacheObject != null)
            {
                string otype = cacheObject.GetType().Name.ToString();
                //当缓存类型是字符串类型时
                if (otype == "String[]") 
                {
                    if (((Array)cacheObject).Length == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return (string[])cacheObject;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 返回缓存数据
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <returns>缓存字符串数组对象</returns>
        public virtual T[] RetrieveArrayObject<T>(string objectId)
        {
            //向缓存加入新的对象
            object cacheObject = RetrieveObject(objectId);
            if (cacheObject != null)
            {
                return cacheObject as T[];
            }
            return null;
        }


        /// <summary>
        /// 返回缓存数据
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        /// <returns>缓存字符串数组对象</returns>
        public virtual string[,] RetrieveArraysObject(string objectId)
        {
            //向缓存加入新的对象
            object cacheObject = RetrieveObject(objectId);
            if (cacheObject != null)
            {
                string otype = cacheObject.GetType().Name.ToString();
                //当缓存类型是字符串类型时
                if (otype == "String[,]")
                {
                    if (((Array)cacheObject).Length == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return (string[,])cacheObject;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 通过缓存键值删除缓存中的对象
        /// </summary>
        /// <param name="objectId">缓存键值</param>
        public virtual void RemoveObject(string objectId)
        {
            if (objectId != null && objectId.Length != 0)
            {                
                lock (lockHelper)
                {
                    //移除相应的缓存项
                    cs.RemoveObject(objectId);
                }
            }
        }


        /// <summary>
        /// 移除所有缓存项
        /// </summary>
        public void ClearCache()
        {
            lock (lockHelper)
            {
                //移除所有缓存项
                cs.Clear();
            }
        }


        /// <summary>
        /// 列显缓存键值信息
        /// </summary> 
        public string[] List()
        {
            return cs.List();
        }


        /// <summary>
        /// 加载指定的缓存策略
        /// </summary>
        /// <param name="ics"></param>
        public void LoadCacheStrategy(ICacheStrategy ics)
        {
            lock (lockHelper)
            {
                cs = ics;
            }
        }


        /// <summary>
        /// 加载默认的缓存策略
        /// </summary>
        public void LoadDefaultCacheStrategy()
        {
            lock (lockHelper)
            {
                cs = new DefaultCacheStrategy();
            }
        }

    }
}
