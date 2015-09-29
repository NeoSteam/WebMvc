using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Global
{
    public enum BaseDataType
    {
        SexList,
        JobsList,
        RaceList,
        CountryList,
        Attribute,
        Profession
    }

    public enum eLanguage : int
    {
        Chinese = 1,
        English = 2,
        All = 3
    }

    /// <summary>
    /// 基础数据分隔符
    /// </summary>
    public class BasedataSeparator
    {
        /// <summary>
        /// 半角逗号
        /// </summary>
        public static string Comma = ",";

        /// <summary>
        /// 半角封号
        /// </summary>
        public static string Semicolon = ";";

        /// <summary>
        /// 半角竖线
        /// </summary>
        public static string VerticalLine  = "|";

        /// <summary>
        /// Tab符号
        /// </summary>
        public static string Tab = "\b";
    }

    /// <summary>
    /// 基础数据写入Cache的Dictionary对象的主键值为基础数据哪列数据
    /// </summary>
    internal class BasedataDicCacheKeyCfg
    {
        public int this[string categoryName]
        {
            get
            {
                Dictionary<string, int> dic = new BasedataDicCacheKeyCfgInit().CacheKeyCfg;
                if (dic.ContainsKey(categoryName))
                    return dic[categoryName];
                else 
                    return 0;
            }
        }
    }
    /// <summary>
    /// 初始化基础数据写入Cache的Dictionary对象的主键值为基础数据哪列数据
    /// 列号从0开始
    /// </summary>
    internal class BasedataDicCacheKeyCfgInit
    {
        public Dictionary<string, int> CacheKeyCfg;

        public BasedataDicCacheKeyCfgInit()
        {
            //初始化放入cache的基础数据Dictionary的key是用哪一列数据，列号从0开始
            CacheKeyCfg = new Dictionary<string, int>();
        }
    }
}
