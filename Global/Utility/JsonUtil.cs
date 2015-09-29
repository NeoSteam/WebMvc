using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml;
using Newtonsoft.Json;

namespace Global
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonUtility
    {
        /// <summary>
        /// Json序列化，将对象生成json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string BuildJsonString<T>(T o)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(o.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, o);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Json反序列化，将Json字符串反列化到对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="JsonString">Json数据串</param>
        /// <returns></returns>
        public static T ParserJsonString<T>(string JsonString)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(JsonString)))
                {
                    T o = (T)serializer.ReadObject(ms);
                    return o;
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 将json内容转成XML格式并保存为xml文件
        /// </summary>
        /// <param name="JsonString">Json内容</param>
        /// <param name="xmlPath">xml文件路径</param>
        /// <returns></returns>
        public static bool SaveXMLFileWithJson(string JsonString, string xmlPath)
        {
            try
            {
                XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(JsonString), XmlDictionaryReaderQuotas.Max);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                doc.Save(xmlPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tobject"></param>
        /// <returns></returns>
        public static string SerializdJson<T>(T tobject)
        {
            string strValue = "";
            try
            {
                strValue = JsonConvert.SerializeObject(tobject);
                return strValue;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
