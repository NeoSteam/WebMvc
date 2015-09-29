using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Global
{
    /// <summary>
    /// 该类适用于基础数据文件数据项只有三项，格式如下： 
    /// ID|中文名称|英文名称
    /// </summary>
    /// <remarks>
    /// 基础数据文件编码格式为操作系统默认格式ANSI,基础数据项分隔符号为竖线|
    /// </remarks>
    public class BasedataStandard {
        /// <summary>
        /// 根据ID获取中文名称
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="id">要查找的ID</param>
        /// <returns></returns>
        public static string GetNameByID(BaseDataType CategoryName, string id) {
            return GetNameByID(CategoryName, id, eLanguage.Chinese);
        }

        /// <summary>
        /// 根据ID获取名称
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="id">要查找的ID</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static string GetNameByID(BaseDataType CategoryName, string id, eLanguage language) {
            return TextBasedataManager.GetNameByID(CategoryName.ToString(), id, language, Encoding.Default, BasedataSeparator.VerticalLine);
        }

        /// <summary>
        /// 根据多个ID返回其对应的中文名称字符串
        /// 返回结果为逗号作为分隔符的字符串。
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="arrIds">要查找的ID数组</param>
        /// <returns>逗号分隔的字符串</returns>
        public static string GetNameByIDs(BaseDataType CategoryName, string[] arrIds) {
            return GetNameByIDs(CategoryName, arrIds, eLanguage.Chinese, BasedataSeparator.Comma);
        }

        /// <summary>
        /// 根据多个ID返回其对应的中文名称字符串
        /// 返回结果为separator分隔符的字符串。
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="arrIds">多个ID数组</param>
        /// <param name="separator">返回值的分隔符</param>
        /// <returns></returns>
        public static string GetNameByIDs(BaseDataType CategoryName, string[] arrIds, string separator) {
            return GetNameByIDs(CategoryName, arrIds, eLanguage.Chinese, separator);
        }

        /// <summary>
        /// 根据多个ID返回其对应的中文名称或英文名称字符串，返回结果用逗号作为分隔符。
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="arrIds">要查找的ID数组</param>
        /// <param name="language">语言</param>
        /// <param name="separator">返回值的分隔符</param>
        /// <returns>逗号分隔的字符串</returns>
        public static string GetNameByIDs(BaseDataType CategoryName, string[] arrIds, eLanguage language, string separator) {
            string[] ret = GetNameArrayByIDs(CategoryName, arrIds, language);
            if (ret != null && ret.Length > 0)
                return string.Join(separator, ret);
            return string.Empty;
        }

        /// <summary>
        /// 根据多个ID返回其对应的中文名称或英文名称字符串
        /// 返回结果为字符串数组string[]。
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="arrIds">要查找的ID数组</param>
        /// <param name="language">语言</param>
        /// <returns>逗号分隔的字符串</returns>
        public static string[] GetNameArrayByIDs(BaseDataType CategoryName, string[] arrIds, eLanguage language) {
            if (arrIds == null || arrIds.Length == 0)
                return null;

            Dictionary<string, string[]> data = TextBasedataManager.GetRowDataByID(CategoryName.ToString(), arrIds, Encoding.Default, BasedataSeparator.VerticalLine);
            if (data == null || data.Count == 0)
                return null;

            string[] ret = new string[arrIds.Length];
            int i = 0;
            foreach (KeyValuePair<string, string[]> row in data) {
                if (language == eLanguage.English)
                    ret[i++] = row.Value[2];
                else
                    ret[i++] = row.Value[1];
            }
            return ret;
        }

        /// <summary>
        /// 根据多个ID获取名称,查找结果存放到dictionary中，
        /// key为查找的ID， value为该ID所对应数据行的名称。
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="arrIds">要查找的ID数组</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetNamesByIDs(BaseDataType CategoryName, string[] arrIds, eLanguage language) {
            return TextBasedataManager.GetMultiNamesByIDs(CategoryName.ToString(), arrIds, Encoding.Default, language, BasedataSeparator.VerticalLine);
        }

        /// <summary>
        /// 根据ID获取数据行对象
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="id">要查找的ID</param>
        /// <returns></returns>
        public static BasedataStandardInfo GetModelByID(BaseDataType CategoryName, string id) {
            BasedataStandardInfo model = new BasedataStandardInfo();
            try
            {
                //根据ID返回该数据行
                string[] _rowData = TextBasedataManager.GetRowDataByID(CategoryName.ToString(), id, Encoding.Default, BasedataSeparator.VerticalLine);
                if (_rowData != null && _rowData.Length == 3)
                {
                    model.ID = _rowData[0];
                    model.CNName = _rowData[1];
                    model.ENName = _rowData[2];
                }
                return model;
            }
            catch
            {
                return model;
            }
        }

        /// <summary>
        /// 根据多个ID获取多个数据行对象
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="arrIds">要查找的ID数组</param>
        /// <returns></returns>
        public static BasedataStandardInfo[] GetModelArrayByIDs(BaseDataType CategoryName, string[] arrIds) {
            if (arrIds == null || arrIds.Length == 0) {
                //没有要查找的数据ID
                return null;
            }
            BasedataStandardInfo[] ret = new BasedataStandardInfo[arrIds.Length];
            Dictionary<string, string[]> data = TextBasedataManager.GetRowDataByID(CategoryName.ToString(), arrIds, Encoding.Default, BasedataSeparator.VerticalLine);
            if (data != null) {
                int i = 0;
                foreach (KeyValuePair<string, string[]> row in data) {
                    BasedataStandardInfo model = new BasedataStandardInfo();
                    model.ID = row.Value[0];
                    model.CNName = row.Value[1];
                    model.ENName = row.Value[2];
                    ret[i] = model;
                    i++;
                }
                return ret;
            } else
                return null;

        }

        /// <summary>
        /// 根据中文名称获取对应的ID
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="strName">中文名称</param>
        /// <returns></returns>
        public static string GetIDByName(BaseDataType CategoryName, string strName) {
            return GetIDByName(CategoryName, strName, eLanguage.Chinese);
        }

        /// <summary>
        /// 根据language名称获取对应的ID
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="strName">名称</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static string GetIDByName(BaseDataType CategoryName, string strName, eLanguage language) {
            string[] data = GetIDByNames(CategoryName, new string[] { strName }, language);
            if (data != null && data.Length > 0)
                return data[0];
            else
                return string.Empty;
        }

        /// <summary>
        /// 根据多个名称返回多个ID字符串数组
        /// 返回结果为string[]
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="arrName">要查找的名称字符串数组</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static string[] GetIDByNames(BaseDataType CategoryName, string[] arrName, eLanguage language) {
            if (arrName == null || arrName.Length == 0)
                return null;

            int lang = (language == eLanguage.English) ? 2 : 1;
            List<string[]> data = TextBasedataManager.GetRowDataByArrays(CategoryName.ToString(), Encoding.Default, BasedataSeparator.VerticalLine, arrName, lang);
            if (data == null || data.Count == 0)
                return null;
            string[] ret = new string[data.Count];
            int i = 0;
            foreach (string[] row in data) {
                ret[i++] = row[0];
            }
            return ret;
        }

        /// <summary>
        /// 根据中文名称获取BasedataStandardInfo对象(包含整个数据行数据)
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="strName">中文名称</param>
        /// <returns>BasedataStandardInfo对象</returns>
        public static BasedataStandardInfo GetModelByName(BaseDataType CategoryName, string strName) {
            return GetModelByName(CategoryName, strName, eLanguage.Chinese);
        }

        /// <summary>
        /// 根据名称获取BasedataStandardInfo对象
        /// 返回符合条件的第一行数据
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="strName">名称</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static BasedataStandardInfo GetModelByName(BaseDataType CategoryName, string strName, eLanguage language) {
            try
            {
                BasedataStandardInfo[] data = GetModelsByName(CategoryName, strName, language);
                if (data != null && data.Length > 0)
                    return data[0];
                return new BasedataStandardInfo();
            }
            catch
            {
                return new BasedataStandardInfo();
            }
        }

        /// <summary>
        /// 根据名称获取BasedataStandardInfo对象集合
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="strName">名称</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static BasedataStandardInfo[] GetModelsByName(BaseDataType CategoryName, string strName, eLanguage language) {
            if (string.IsNullOrEmpty(strName)) {
                //没有要查找的数据,异常处理
                return null;
            }

            List<string[]> data = TextBasedataManager.GetRowDataByArrays(CategoryName.ToString(), Encoding.Default, BasedataSeparator.VerticalLine, new string[] { strName }, (language == eLanguage.English) ? 2 : 1);
            if (data == null) {
                //没有找到数据
                return null;
            }
            BasedataStandardInfo[] ret = new BasedataStandardInfo[data.Count];
            int i = 0;
            foreach (string[] row in data) {
                BasedataStandardInfo model = new BasedataStandardInfo();
                model.ID = row[0];
                model.CNName = row[1];
                model.ENName = row[2];
                ret[i++] = model;
            }
            return ret;
        }

        /// <summary>
        /// 根据多个名称返回多个ID字符串（以逗号分隔）
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <param name="arrName">要查找的名称字符串数组</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static string GetIDsStringByNames(BaseDataType CategoryName, string[] arrName, eLanguage language) {
            string[] ret = GetIDByNames(CategoryName, arrName, language);
            if (ret != null)
                return string.Join(",", ret);
            else
                return null;
        }

        /// <summary>
        /// 获取基础数据文件所有数据行
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <returns></returns>
        public static BasedataStandardInfo[] GetAllDataArray(BaseDataType CategoryName) {
            Dictionary<string, List<string>> data = TextBasedataManager.GetDataDictionary(CategoryName.ToString(), Encoding.Default, BasedataSeparator.VerticalLine);
            if (data != null) {
                BasedataStandardInfo[] _ret = new BasedataStandardInfo[data.Count];
                int i = 0;
                foreach (KeyValuePair<string, List<string>> item in data) {
                    BasedataStandardInfo model = new BasedataStandardInfo();
                    model.ID = item.Value[0];
                    model.CNName = item.Value[1];
                    //下面一行是wang.jun添加，2010.11.4
                    if (item.Value.Count >= 3)
                        model.ENName = item.Value[2];
                    _ret[i++] = model;
                }
                return _ret;
            }
            return null;
        }

        /// <summary>
        /// 获取基础数据文件所有数据行
        /// </summary>
        /// <param name="CategoryName">基础数据分类名称</param>
        /// <returns></returns>
        public static BasedataStandardInfo[] GetAllData(BaseDataType CategoryName)
        {
            string[,] data = TextBasedataManager.GetDataArray(CategoryName.ToString(), Encoding.Default, BasedataSeparator.VerticalLine);
            if (data != null)
            {
                BasedataStandardInfo[] _ret = new BasedataStandardInfo[data.GetLength(0)];
                for (int i=0; i < data.GetLength(0); i++)
                {
                    BasedataStandardInfo model = new BasedataStandardInfo();
                    model.ID = data[i, 0];
                    model.CNName = data[i,1];
                    model.ENName = data[i,2];
                    _ret[i] = model;
                }
                return _ret;
            }
            return null;
        }
    }

    /// <summary>
    /// 基础数据标准格式模型类，只有3列，格式如下：
    /// ID|中文名称|英文名称
    /// </summary>
    public class BasedataStandardInfo
    {
        public BasedataStandardInfo()
        {
            this.CNName = string.Empty;
            this.ENName = string.Empty;
            this.ID = string.Empty;
        }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        public string CNName { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string ENName { get; set; }
    }
}
