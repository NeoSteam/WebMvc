using System;
using System.Text;
using System.Web.Caching;

namespace Global
{
    /// <summary>
    /// 公共缓存策略接口
    /// </summary>
    public interface ICacheStrategy
    {
        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        void AddObject(string objId, object o);

        /// <summary>
        /// 添加指定ID的对象(关联指定文件组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="files"></param>
        void AddObjectWithFileChange(string objId, object o, string[] files);

        /// <summary>
        /// 添加指定ID的对象(关联指定文件)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="fileName"></param>
        void AddObjectWithFileChange(string objId, object o, string fileName);

        /// <summary>
        /// 添加指定ID的对象(关联指定文件)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="fileName"></param>
        /// <param name="time"></param>
        void AddObjectWithFileChange(string objId, object o, string fileName, double time);

        /// <summary>
        /// 添加指定ID的对象(关联指定文件)
        /// </summary>
        /// <param name="objId">缓存键值</param>
        /// <param name="o">被缓存的对象</param>
        /// <param name="fileName">监视的路径文件</param>
        /// <param name="callBack">回调的委托</param>'
        /// <remarks>
        /// add by mason 2009-09-11
        /// </remarks>
        void AddObjectWithFileChange(string objId, object o, string fileName, CacheItemRemovedCallback callBack);


        /// <summary>
        /// 添加指定ID的对象(关联指定键值组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependKey"></param>
        void AddObjectWithDepend<T>(string objId, T o, string[] dependKey);

        /// <summary>
        /// 添加指定ID的对象(关联指定键值组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependKey"></param>
        void AddObjectWithDepend<T>(string objId, T[] o, string[] dependKey);

        /// <summary>
        /// 添加指定ID的对象(关联指定键值组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependKey"></param>
        void AddObjectWithDepend<T>(string objId, T[,] o, string[] dependKey);

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        void RemoveObject(string objId);

        /// <summary>
        /// 清除缓存
        /// </summary>        
        void Clear();

        /// <summary>
        /// 列显缓存键值信息
        /// </summary> 
        string[] List();

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        object RetrieveObject(string objId);

        /// <summary>
        /// 添加指定泛型对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>        
        void AddObject<T>(string objId, T o);

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool RetrieveObject<T>(string objId, out T value);

        /// <summary>
        /// 到期时间
        /// </summary>
        int TimeOut { set; get; }
    }
}
