using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Global
{
    /// <summary>
    /// 文本基础数据处理类的基类
    /// </summary>
    /// <remarks>
    /// 设计思路：由于文本基础数据的处理分为几种情况，
    /// 1.标准三列数据, 数据格式为: ID|中文名称|英文名称
    /// 2.职位小类基础数据,数据格式为:大类ID|小类Code|小类ID|小类中文名称|小类英文名称
    /// 3.城市基础数据,数据格式为:城市ID|中文名称|英文名称|城市缩写
    /// 4.没有特定列数的基础数据
    /// 由于上述几种情况数据处理方法类似,故尔写了该类,提供了一些通用的处理方法
    /// 基础数据缓存有两种格式：
    /// 1、二维数组；cache命名格式为CACHE_BASEDATA_TEXT_ + CategoryName
    /// 2、Dictionary；key一般为ID，value为string list集合
    /// cache命名格式为CACHE_BASEDATA_TEXT_DIC_ + CategoryName
    /// </remarks>
    internal class TextBasedataManager
    {
        private static string BasedataFolderPath = ConfigurationManager.AppSettings["BaseDataPath"];

        private const string CNT_CACHE_BASEDATA_TEXT_PREFIX = "CACHE_BASEDATA_TEXT_";
        private const string CNT_CACHE_BASEDATA_TEXT_DIC_PREFIX = "CACHE_BASEDATA_TEXT_DIC_";

        //public TextBasedataManager() { }
        /// <summary>
        /// 通过ID获取中文名称
        /// 数据文件编码格式采用操作系统默认ANSI格式。数据项分隔符默认为"|"
        /// </summary>
        /// <param name="categoryName">基础数据类别名称</param>
        /// <param name="id">数据项ID</param>
        /// <returns>数据项ID所对应的中文名称</returns>
        internal static string GetNameByID(string categoryName, string id)
        {
            return GetNameByID(categoryName, id, eLanguage.Chinese, Encoding.Default, BasedataSeparator.VerticalLine);
        }

        /// <summary>
        /// 通过ID获取中文名称
        /// 数据文件编码格式采用操作系统默认ANSI格式。
        /// </summary>
        /// <param name="categoryName">基础数据类别名称</param>
        /// <param name="id">数据项ID</param>
        /// <param name="separator">数据项分隔符号，如，；|</param>
        /// <returns>数据项ID所对应的中文名称</returns>
        internal static string GetNameByID(string categoryName, string id, string separator)
        {
            return GetNameByID(categoryName, id, eLanguage.Chinese, Encoding.Default, separator);
        }

        /// <summary>
        /// 通过ID获取中文名称
        /// </summary>
        /// <param name="categoryName">基础数据分类名称,
        /// 可以先到以下枚举对象中查看分类名称是否存在?
        /// 1. RDBaseDataCategory;
        /// 2. JSBaseDataCategory
        /// 如果不存在，则提供分类名称字符串,即基础数据文件名称去掉文件扩展名的内容.
        /// </param>
        /// <param name="id">基础数据项ID</param>
        /// <param name="encode">基础数据文件编码格式</param>
        /// <param name="separator">数据项分隔符号，如，；|</param>
        /// <returns>返回指定基础数据对应ID的中文名称</returns>
        internal static string GetNameByID(string categoryName, string id, Encoding encode, string separator) 
        {
            return GetNameByID(categoryName, id, eLanguage.Chinese, encode, separator);
        }      

        /// <summary>
        /// 通过ID获取language指定的名称(返回中文或英文名称)
        /// 注意：该方法只适用于基础数据项中第1列为ID，第2列为中文名称，第3列为英文名称
        /// </summary>
        /// <param name="categoryName">基础数据分类名称,
        /// 可以先到以下枚举对象中查看分类名称是否存在?
        /// 1. RDBaseDataCategory;
        /// 2. JSBaseDataCategory
        /// 如果不存在，则提供分类名称字符串,即基础数据文件名称去掉文件扩展名的内容.
        /// </param>
        /// <param name="id">基础数据项ID</param>
        /// <param name="language">语言</param>
        /// <param name="encode">基础数据文件编码格式</param>
        /// <param name="separator">数据项分隔符号，如，；|</param>
        /// <returns>返回指定基础数据对应ID的Language名称</returns>
        internal static string GetNameByID(string categoryName, string id, eLanguage language, Encoding encode, string separator)
        {
            if (string.IsNullOrEmpty(id))
            { 
                //参数不正确
                return string.Empty;
            }
            if (language == eLanguage.Chinese)
                return GetColumnValueByID(categoryName, id, encode, separator, 1);
            else if (language == eLanguage.English)
                return GetColumnValueByID(categoryName, id, encode, separator, 2);
            else
                return string.Empty;
        }

        /// <summary>
        /// 根据ID获取指定列的内容
        /// </summary>
        /// <param name="categoryName">基础数据分类名称</param>
        /// <param name="id">要查找的ID字符串</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <param name="columnNumber">取第几列数据，列号从0开始</param>
        /// <returns></returns>
        internal static string GetColumnValueByID(string categoryName, string id, Encoding encode, string separator, int columnNumber)
        {
            Dictionary<string, List<string>> dicData = GetDataDictionary(categoryName, encode, separator);
            if (dicData == null)
            {
                //异常处理
                return string.Empty;
            }
            try
            {
                List<string> dataline = dicData[id];
                if ((dataline == null) || (dataline.Count == 0))
                    return string.Empty;
                if (columnNumber < 0 || columnNumber >= dataline.Count)
                    return string.Empty;
                return dataline[columnNumber].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据多个ID返回指定列的内容
        /// 返回结果为Dictionary对象，string key, string value
        /// key = ID值
        /// </summary>
        /// <param name="categoryName">基础数据分类名称</param>
        /// <param name="arrIds">要查找的ID字符串数组</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <param name="columnNumber">需要返回基础文件中第几列数据内容,列数从0开始，
        /// 例如取基础数据文件中第2列数据，则该参数设置为1</param>
        /// <returns>查找结果放入dictionary, key为ID，value为ID所对应数据行的第几列数据</returns>
        internal static Dictionary<string, string> GetColumnValuesByIDs(string categoryName, string[] arrIds, Encoding encode, string separator, int columnNumber)
        {
            try
            {
                if (arrIds == null || arrIds.Length == 0)
                {
                    //没有指定要查找的数据,异常处理
                    return null;
                }
                Dictionary<string, List<string>> dicData = GetDataDictionary(categoryName, encode, separator);
                if (dicData == null)
                {
                    //异常处理
                    return null;
                }
                if (columnNumber < 0 || columnNumber >= (dicData[arrIds[0]].Count))
                {
                    //要查找的列号不存在，异常处理
                    return null;
                }

                Dictionary<string, string> _dicRet = new Dictionary<string, string>();
                for (int i = 0; i < arrIds.Length; i++)
                {
                    _dicRet.Add(arrIds[i], dicData[arrIds[i]][columnNumber]);
                }
                return _dicRet;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据多个ID返回对应的数据行
        /// 返回结果为Dictionary对象，string key, string[] value;
        /// key = ID值
        /// </summary>
        /// <param name="categoryName">基础数据分类名称</param>
        /// <param name="arrIds">要查找的ID字符串数组</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <returns>查找结果放入Dictionary,key为ID，value为一行数据string[]</returns>
        internal static Dictionary<string, string[]> GetMultiDataRowByIDs(string categoryName, string[] arrIds, Encoding encode, string separator)
        {
            try
            {
                Dictionary<string, List<string>> dicData = GetDataDictionary(categoryName, encode, separator);
                if (dicData == null)
                {
                    //异常处理
                    return null;
                }
                Dictionary<string, string[]> _ret = new Dictionary<string, string[]>();
                foreach (string id in arrIds)
                {
                    _ret.Add(id, dicData[id].ToArray<string>());
                }
                return _ret;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据多个ID获取多个名称
        /// 结果存入dictionary对象，string key, string value
        /// key = ID值
        /// 注意：该方法只适用于基础数据文件中数据项，明确第1列为中文名称，第2列为英文名称
        /// </summary>
        /// <param name="categoryName">基础数据分类名称</param>
        /// <param name="arrID">要查找的ID字符串数组</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="language">语言</param>        
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <returns></returns>
        internal static Dictionary<string, string> GetMultiNamesByIDs(string categoryName, string[] arrIds, Encoding encode, eLanguage language, string separator)
        {
            try
            {
                Dictionary<string, string> _dicRet = new Dictionary<string, string>();
                if (arrIds == null || arrIds.Length == 0)
                {
                    //没有要处理的id
                    return null;
                }
                int cellNumber = 0;
                if (language == eLanguage.English)
                    cellNumber = 2;
                else
                    cellNumber = 1;
                _dicRet = GetColumnValuesByIDs(categoryName, arrIds, encode, separator, cellNumber);
                return _dicRet;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 通过名称获取ID字符串
        /// </summary>
        /// <param name="categoryName">基础数据分类名称,
        /// 可以先到以下枚举对象中查看分类名称是否存在?
        /// 1. RDBaseDataCategory;
        /// 2. JSBaseDataCategory
        /// 如果不存在，则提供分类名称字符串,即基础数据文件名称去掉文件扩展名的内容.
        /// </param>
        /// <param name="id">基础数据项ID</param>
        /// <param name="language">语言</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <returns>返回指定基础数据项名称所对应的ID</returns>
        internal static string GetIDByName(string categoryName, string Name, eLanguage language, Encoding encode, string separator)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    //要查找的数据为空，异常处理
                    return string.Empty;
                }
                int searchColumnNumber = 0;
                if (language == eLanguage.English)
                    searchColumnNumber = 2;
                else
                    searchColumnNumber = 1;
                string[] searchValue = new string[1];
                searchValue[0] = Name;
                List<string[]> data = GetRowDataByArrays(categoryName, encode, separator, searchValue, searchColumnNumber);
                if (data == null || data.Count == 0)
                    return string.Empty;
                else
                    return data[0][searchColumnNumber];
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据某列的多个值返回数据行
        /// 结果为List集合
        /// 在基础数据文件columnNumber指定列中查找string[]多项内容，如果找到则返回string[]集合；否则返回null.
        /// </summary>
        /// <param name="categoryName">基础数据分类名称</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <param name="arrSearchContents">待查找列值</param>
        /// <param name="searchColumn">待查找的列号，列号从0开始</param>
        /// <returns></returns>
        internal static List<string[]> GetRowDataByArrays(string categoryName, Encoding encode, string separator, string[] arrSearchContents, int searchColumn)
        {
            try
            {
                Cache cache = Cache.GetCacheService();
                string[,] data = GetDataArray(categoryName, encode, separator);
                if (data == null)
                {
                    //没有基础数据，异常处理 
                    return null;
                }
                int cellCount = data.GetLength(1); //获取列数
                if (searchColumn < 0 || searchColumn >= cellCount)
                {
                    //所要查找的列数不存在,异常处理
                    return null;
                }

                List<string[]> ret = new List<string[]>();
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    if (arrSearchContents.Length > 0)
                    {
                        int tmpCount = 0;
                        for (int m = 0; m < arrSearchContents.Length; m++)
                        {
                            if (data[i, searchColumn].ToUpper() == arrSearchContents[m].ToUpper())
                            {
                                string[] rowdata = new string[cellCount];
                                for (int j = 0; j < data.GetLength(1); j++)
                                {
                                    rowdata[j] = data[i, j];
                                }
                                ret.Add(rowdata);
                                //arrSearchContents.RemoveAt(tmpCount);
                            }
                            tmpCount++;
                        }
                    }
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据ID获取整行数据
        /// </summary>
        /// <param name="categoryName">基础数据分类名称,
        /// 可以先到以下枚举对象中查看分类名称是否存在?
        /// 1. RDBaseDataCategory;
        /// 2. JSBaseDataCategory
        /// 如果不存在，则提供分类名称字符串,即基础数据文件名称去掉文件扩展名的内容.
        /// </param>
        /// <param name="id">基础数据项ID</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <returns>返回指定ID所在的整个数据行string[]</returns>
        internal static string[] GetRowDataByID(string categoryName, string id, Encoding encode, string separator)
        {
            try
            {
                Dictionary<string, List<string>> dicData = GetDataDictionary(categoryName, encode, separator);
                if (dicData == null)
                {
                    //异常处理
                    return null;
                }
                string[] data = dicData[id].ToArray<string>();
                if (data.Length == 0)
                {
                    //没有找到数据行,异常处理
                    return null;
                }
                return data;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据多个ID获取多个数据行
        /// </summary>
        /// <param name="categoryName">基础数据分类名称</param>
        /// <param name="arrIds">基础数据项ID数组</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <returns></returns>
        internal static Dictionary<string, string[]> GetRowDataByID(string categoryName, string[] arrIds, Encoding encode, string separator)
        {
            if (arrIds == null || arrIds.Length == 0)
            { 
                //要查找的数据未设置,异常处理
                return null;
            }
            try
            {
                Dictionary<string, List<string>> dicData = GetDataDictionary(categoryName, encode, separator);
                if (dicData == null)
                {
                    //异常处理
                    return null;
                }
                Dictionary<string, string[]> ret = new Dictionary<string, string[]>();
                foreach (string id in arrIds)
                {
                    string _id = id.Trim();
                    if (!ret.ContainsKey(_id))
                    {
                        if (dicData.ContainsKey(_id))
                            ret.Add(_id, dicData[_id].ToArray<string>());
                    }
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 生成基础数据文件绝对路径
        /// </summary>
        /// <param name="categoryName">基础数据类别名称</param>
        /// <returns>基础数据文件绝对路径</returns>
        private static string GetBasedataFilePath(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return string.Empty;
            return string.Format("{0}{1}.txt", BasedataFolderPath.EndsWith("\\") ? BasedataFolderPath : BasedataFolderPath + "\\", categoryName);
        }



        /// <summary>
        /// 生成基础数据字典，放入Cache
        /// 默认情况用第一列数据作为Dictionary的key值
        /// </summary>
        /// <param name="categoryName">基础数据类别名称</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <returns>基础数据dictionary,包含所有数据行数据</returns>
        internal static Dictionary<string, List<string>> GetDataDictionary(string categoryName, Encoding encode, string separator)
        {
            int columnNumber = new BasedataDicCacheKeyCfg()[categoryName];
            return GetDataDictionary(categoryName, encode, separator, columnNumber);
        }

        /// <summary>
        /// 生成基础数据字典，放入Cache
        /// </summary>
        /// <param name="categoryName">基础数据分类名称</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <param name="keyColumnNumber">用第几列数据作为Dictionary的key </param>
        /// <returns>基础数据dictionary,包含所有数据行数据</returns>
        internal static Dictionary<string, List<string>> GetDataDictionary(string categoryName, Encoding encode, string separator, int keyColumnNumber)
        {
            try
            {
                Cache cache = Cache.GetCacheService();
                Dictionary<string, List<string>> dicData = (Dictionary<string, List<string>>)cache.RetrieveObject(CNT_CACHE_BASEDATA_TEXT_DIC_PREFIX + categoryName);
                string _baseFilePath = GetBasedataFilePath(categoryName);

                if (dicData == null)
                {
                    //用二维数据生成Dictionary对象
                    string[,] arrBasedata = GetDataArray(categoryName, encode, separator);

                    if (arrBasedata == null)
                    {
                        //异常处理
                        return null;
                    }

                    dicData = new Dictionary<string, List<string>>();
                    for (int i = 0; i < arrBasedata.GetLength(0); i++)
                    {
                        List<string> line = new List<string>();
                        for (int j = 0; j < arrBasedata.GetLength(1); j++)
                        {
                            line.Add(arrBasedata[i, j]);
                        }
                        dicData.Add(arrBasedata[i, keyColumnNumber], line);
                    }
                    cache.AddObject(CNT_CACHE_BASEDATA_TEXT_DIC_PREFIX + categoryName, dicData, _baseFilePath);
                }
                return dicData;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 生成基础数据二维数组
        /// 如果cache中没有找到，则从基础数据文件生成并写入cache
        /// </summary>
        /// <param name="categoryName">基础数据分类名称</param>
        /// <param name="encode">数据文件编码格式</param>
        /// <param name="separator">基础数据数据项分隔符</param>
        /// <returns>二维string数组,包含所有数据行数据</returns>
        internal static string[,] GetDataArray(string categoryName, Encoding encode, string separator)
        {
            if (string.IsNullOrEmpty(categoryName))
                return null;
            try
            {
                Cache cache = Cache.GetCacheService();
                string[,] cacheBasedataObject = cache.RetrieveArraysObject(CNT_CACHE_BASEDATA_TEXT_PREFIX + categoryName);
                if (cacheBasedataObject == null)
                {
                    string _baseFilePath = GetBasedataFilePath(categoryName);
                    if (!File.Exists(_baseFilePath))
                    {
                        //文件不存在，异常处理
                        //throw new Exception(string.Format("{0}文件不存在。", _baseFilePath));
                        return null;
                    }
                    string data;
                    using (StreamReader sr = new StreamReader(_baseFilePath, encode))
                    {
                        data = sr.ReadToEnd().Trim();
                        sr.Close();
                    }
                    string[] arrData = data.Replace("\r", "").Split('\n');
                    int _rowCount = 0;
                    int _cellCount = 0;
                    foreach (string line in arrData)
                    {
                        //"#"开头和空都是无效数据行
                        if (!IsValidDataLine(line))
                            continue;
                        _rowCount++;
                        if (_rowCount == 1)
                            _cellCount = line.Split(separator.ToString().ToArray()).Length;
                    }
                    string[,] basedata = new string[_rowCount, _cellCount];
                    int i = 0;
                    string[] arrCells;
                    foreach (string line in arrData)
                    {
                        //"#"开头和空都是无效数据行
                        if (!IsValidDataLine(line))
                            continue;
                        arrCells = line.Split(separator.ToString().ToArray());
                        for (int j = 0; j < arrCells.Length; j++)
                        {
                            basedata[i, j] = arrCells[j].Trim();
                        }
                        i++; //记录行号
                    }
                    cache.AddObject(string.Format("{0}{1}", CNT_CACHE_BASEDATA_TEXT_PREFIX, categoryName), basedata, _baseFilePath);
                    return basedata;
                }
                else
                {
                    return cacheBasedataObject;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 判断数据行是否是有效数据
        /// </summary>
        /// <param name="data">基础数据文件中的数据行</param>
        /// <returns>true-有效数据； false-无效数据；</returns>
        private static bool IsValidDataLine(string data)
        {
            //如果该行数据为空，则忽略
            if (string.IsNullOrEmpty(data.Trim()))
                return false;
            //判断是否是#开头，如果是，则忽略该行数据
            if (data.Trim().StartsWith("#"))
                return false;          
            return true;
        }

    }
}
