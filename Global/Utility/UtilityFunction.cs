using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace Global
{
    /// <summary>
    /// 通用工具函数类
    /// </summary>
    public class UtilityFunction
    {
        public static void SaveFile(string path, string filename, string value)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string full_path = Path.Combine(path, filename);
            FileStream fs = new FileStream(full_path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(value);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static string ReadFile(string path, string filename)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string full_path = Path.Combine(path, filename);
            FileStream fs = new FileStream(full_path, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            string value = sr.ReadLine();
            sr.Close();
            fs.Close();

            return value;
        }

        public static string SerializeToString(object obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);

            ms.Position = 0;
            byte[] buffer = new byte[ms.Length];
            ms.Read(buffer, 0, buffer.Length);
            ms.Flush();
            ms.Close();

            return Convert.ToBase64String(buffer);
        }

        public static T DeserializeToObj<T>(string str)
        {
            if (string.IsNullOrEmpty(str)) return default(T);

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                byte[] buffer = Convert.FromBase64String(str);
                MemoryStream ms = new MemoryStream(buffer);
                T obj = (T)bf.Deserialize(ms);
                ms.Flush();
                ms.Close();

                return obj;
            }
            catch
            {
                return default(T);
            }
        }

        public static bool ArrayConvert(string[] from_arr, out int[] to_arr)
        {
            bool f = true;
            int i = 0;
            List<int> l = new List<int>();

            foreach (string s in from_arr)
            {
                if (int.TryParse(s, out i))
                {
                    l.Add(i);
                }
                else
                {
                    f = false;
                }
            }

            to_arr = l.ToArray();

            return f;
        }

        /// <summary>
        /// html关键字替换
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string HtmlReplace(string str)
        {

            string s;
            s = str;
            s = s.Replace("<", "&lt;");
            s = s.Replace(">", "&gt;");
            s = s.Replace("\"", "&quot;");
            //htmlreplace=s
            return s;
        }

        /// <summary>
        /// Format DateTime Type Object to "1992-07-30" Style
        /// </summary>
        /// <param name="strDate">string Date</param>
        /// <param name="strFormatType">DateType(0:2012-12-21;1:2012-12-21 12:12:12)</param>
        /// <returns>string Date</returns>
        public static string FormatDate(string strDate, string strFormatType)
        {
            DateTime dtDate = DateTime.Now;
            if (!DateTime.TryParse(strDate, out dtDate))
            {
                return strDate;
            }
            return FormatDate(dtDate, strFormatType);
        }

        public static string FormatDate(string strFormatType)
        {
            DateTime dtDate = DateTime.Now;
            return FormatDate(dtDate, strFormatType);
        }

        public static string FormatDate(DateTime dtDate, string strFormatType)
        {
            string strDate = "";
            switch (strFormatType)
            {
                case "0":
                    strDate = dtDate.ToString("yyyy-MM-dd");
                    break;
                case "1":
                    strDate = dtDate.ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "2":
                    strDate = dtDate.ToString("yy-MM-dd");
                    break;
            }
            return strDate;
        }

        /// <summary>
        /// 检查字符串是否是日期
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsDateTime(string strDate)
        {
            try
            {
                DateTime _date = DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否为时间类型，如果是，则将输入转为时间型并返回true,否则返回false,并将错误类型存在errorCode变量中.
        /// </summary>
        /// <param name="strValue">内容</param>
        /// <param name="strMin">允许最小值</param>
        /// <param name="strMax">允许最大值</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns></returns>
        public static bool GFN_IsDate(string strValue, string strMin, string strMax, ref int errorCode, ref DateTime dtValue)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                errorCode = 3;
                return false;
            }
            strValue = strValue.Trim();
            strMin = strMin.Trim();
            strMax = strMax.Trim();
            DateTime dtMin, dtMax;

            if (string.IsNullOrEmpty(strMin))
            {
                strMin = "1900-1-1";
                dtMin = Convert.ToDateTime(strMin);
            }
            else
            {
                if (!IsDateTime(strMin))
                {
                    errorCode = 999;
                    return false;
                }
                else
                    dtMin = Convert.ToDateTime(strMin);
            }

            if (string.IsNullOrEmpty(strMax))
            {
                strMax = "2199-12-31";
                dtMax = Convert.ToDateTime(strMax);
            }
            else
            {
                if (!IsDateTime(strMax))
                {
                    errorCode = 999;
                    return false;
                }
                else
                    dtMax = Convert.ToDateTime(strMax);
            }

            if (DateTime.Compare(dtMin, dtMax) > 0)
            {
                errorCode = 999;
                return false;
            }

            if (!IsDateTime(strValue))
            {
                errorCode = 1;
                return false;
            }
            else
                dtValue = Convert.ToDateTime(strValue);

            if (DateTime.Compare(dtValue, dtMin) < 0)
            {
                errorCode = 2;
                return false;
            }

            if (DateTime.Compare(dtValue, dtMax) > 0)
            {
                errorCode = 2;
                return false;
            }
            errorCode = 0;
            return true;
        }

        /// <summary>
        /// 判断是否为数据库Int类型，如果是，则将输入转为数值型并返回true，否则返回false，并将错误类型存在errorCode
        /// </summary>
        /// <param name="strValue">内容</param>
        /// <param name="strMin">允许的最小值</param>
        /// <param name="strMax">允许的最大值</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns></returns>
        public static bool GFN_IsDBInt(string strValue, string strMin, string strMax, ref int errorCode)
        {
            const long DBINTRANGE = 2147483647;
            if (string.IsNullOrEmpty(strValue))
            {
                errorCode = 3;
                return false;
            }

            strValue = strValue.Trim();
            strMin = strMin.Trim();
            strMax = strMax.Trim();

            long lngMin = 0, lngMax = 0;
            //参数校验
            if (string.IsNullOrEmpty(strMin))
                lngMin = 0 - DBINTRANGE;
            else
            {
                if (!IsNumber(strMin))
                {
                    errorCode = 999;
                    return false;
                }
                else
                    lngMin = Convert.ToInt64(strMin);
            }

            if (string.IsNullOrEmpty(strMax))
                lngMax = DBINTRANGE;
            else
            {
                if (!IsNumber(strMax))
                {
                    errorCode = 999;
                    return false;
                }
                else
                    lngMax = Convert.ToInt64(strMax);
            }

            if (lngMin > lngMax)
            {
                errorCode = 999;
                return false;
            }
            //类型校验
            if (!IsNumber(strValue))
            {
                errorCode = 1;
                return false;
            }

            if (strValue.IndexOf('.') > 0)
            {
                errorCode = 1;
                return false;
            }

            long lngValue = Convert.ToInt64(strValue);
            if (lngValue > DBINTRANGE || lngValue < (0 - DBINTRANGE))
            {
                errorCode = 1;
                return false;
            }

            //范围检查
            if (lngValue < lngMin)
            {
                errorCode = 2;
                return false;
            }

            if (lngValue > lngMax)
            {
                errorCode = 2;
                return false;
            }

            errorCode = 0;
            return true;
        }

        /// <summary>
        /// 是否是数字串，支持负号，但不支持正号（+），支持小数
        /// </summary>
        /// <param name="strNumber">待验证字符串</param>
        /// <returns>ture/false</returns>
        /// <remarks>
        /// Author:mason.zhou
        /// </remarks>
        public static bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
            !objTwoDotPattern.IsMatch(strNumber) &&
            !objTwoMinusPattern.IsMatch(strNumber) &&
            objNumberPattern.IsMatch(strNumber);
        }
        /// <summary>
        /// 是否是整数字符串,可带正负号,但只能出现一次。如：参数形如-111，正确，返回true，参数形如 --111，+-111参数错误，返回false
        /// </summary>
        /// <param name="strNumber">待验证字符串</param>
        /// <returns>ture/false</returns>
        /// <remarks>
        /// Author:mason.zhou
        /// </remarks>
        public static bool IsIntNumber(string strNumber)
        {
            Regex rexIntNumber = new Regex("^([-]|[+])?[0-9]+$");
            return rexIntNumber.IsMatch(strNumber);
        }
        /// <summary>
        /// Author:peterlu
        /// create time 2009/08/04
        /// risk:int不能超出数值的范围
        /// Scope : Global
        /// </summary>
        /// <param name="min">Range Min</param>
        /// <param name="max">Range Max</param>
        /// <returns>Random Int</returns>
        /// <remarks>
        /// 该函数提供的随机数包含上下限值，如GetRandomInt(0, 9)，返回的随机数可能是0或者9----modify by mason 2009-09-01
        /// </remarks>
        public static int GetRandomInt(int min, int max)
        {
            Random rd = new Random();
            //modify by mason 2009-09-01
            return rd.Next(min, max + 1);
            //End modify by mason 2009-09-01
        }
        /// <summary>
        /// String to int32 安全转换,错误返回0
        /// </summary>
        /// <param name="numberStr">string</param>
        /// <returns>int32</returns>
        /// <remark>
        /// 注意：
        /// 1,此处若数字字符串太大，超出int32的范围，会引发异常，这种可能行发生概率非常小，考虑效率这里不做try处理
        /// 2,该函数自动截取参数的第一个“.”（小数点）前面的字串做为有效字串，如StrToInt32(9.9)返回9
        /// 3,支持正负符号，只能是半角（-,+），正负号只能出现其一，并且只能出现一次，如：--1将被视为无效数字串，返回0
        /// Author:mason.zhou
        /// </remark>
        public static int StrToInt32(string numberStr)
        {
            if (string.IsNullOrEmpty(numberStr))
                return 0;
            //截取小数点前面的部分
            if (numberStr.IndexOf('.') > -1)
            {
                numberStr = numberStr.Substring(0, numberStr.IndexOf('.'));
            }
            if (IsIntNumber(numberStr))
            {
                return Convert.ToInt32(numberStr);
            }
            else return 0;
        }
        /// <summary>
        /// string to int64 安全转换,错误返回0
        /// </summary>
        /// <param name="numberStr">string</param>
        /// <returns>int64</returns>
        /// <remark>
        /// 注意：
        /// 1,此处若数字字符串太大，超出int64的范围，会引发异常，这种可能行发生概率非常小，考虑效率这里不做try处理
        /// 2,该函数自动截取参数的第一个“.”（小数点）前面的字串做为有效字串，如StrToInt32("9.9.9")返回9
        /// 3,支持正负符号，只能是半角（-,+），正负号只能出现其一，并且只能出现一次，如：--1将被视为无效数字串，返回0
        /// Author:mason.zhou
        /// </remark>
        public static long StrToInt64(string numberStr)
        {
            if (string.IsNullOrEmpty(numberStr))
                return 0;
            //截取小数点前面的部分
            if (numberStr.IndexOf('.') > -1)
            {
                numberStr = numberStr.Substring(0, numberStr.IndexOf('.'));
            }
            if (IsIntNumber(numberStr))
            {
                return Convert.ToInt64(numberStr);
            }
            else return 0;
        }

        public static int IntParse(string v)
        {
            int a = 0;

            int.TryParse(v, out a);

            return a;
        }

        /// <summary>
        /// 字符串做非NULL和TRIM处理
        /// </summary>
        /// <param name="oldString">原字符串</param>
        /// <returns>处理后的字符串，NULL返回空</returns>
        public static string StrOperate(string oldString)
        {
            if (oldString == null)
            {
                return "";
            }
            else
            {
                return oldString.Trim();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStr_Value"></param>
        /// <param name="pInt_Length"></param>
        /// <returns></returns>
        public static string SubstringRealLength(string pStr_Value, int pInt_Length)
        {
            string Str_ValueReturn = string.Empty;
            if (GetStringRealLength(pStr_Value) > pInt_Length)
            {
                for (int Int_Count_I = 1; Int_Count_I <= pInt_Length; Int_Count_I++)
                {

                    Str_ValueReturn = pStr_Value.Substring(0, Int_Count_I);
                    if (GetStringRealLength(Str_ValueReturn.ToString()) >= pInt_Length)
                        return Str_ValueReturn;
                }
                return Str_ValueReturn.ToString();
            }
            else
            {
                return pStr_Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStr_Value"></param>
        /// <returns></returns>
        public static int GetStringRealLength(string pStr_Value)
        {
            return pStr_Value.Length + System.Text.RegularExpressions.Regex.Matches(pStr_Value, "[\u0080-\uffff]").Count;
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <returns></returns>
        public static bool IsExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <returns></returns>
        public static bool IsFolderExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <returns></returns>
        public static bool CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
            return true;
        }

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="strPath">文件相对路径</param>
        /// <returns>文件绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath == null)
            {
                return string.Empty;
            }
            else
            {
                if (HttpContext.Current != null)
                {
                    if (strPath.StartsWith("/"))
                    {
                        strPath = strPath.TrimStart('/');
                    }
                    string newPath = strPath.Replace("/", "\\");

                    if (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, newPath) != strPath)
                    {
                        return HttpContext.Current.Server.MapPath("\\" + strPath);
                    }
                    else
                    {
                        return strPath;
                    }
                }
                //非web程序引用
                else
                {
                    strPath = strPath.Replace("/", "\\");
                    if (strPath.StartsWith("\\"))
                    {
                        strPath = strPath.TrimStart('\\');
                    }
                    return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
                }
            }
        }

        /// <summary>
        /// 获得字符串左边一定长度的字符串(同vb中Left方法)
        /// </summary>
        /// <param name="pStr_Value">字符串</param>
        /// <param name="pInt_Length">长度</param>
        /// <returns>左边相应长度字符串</returns>
        public static string Left(string pStr_Value, int pInt_Length)
        {
            return pStr_Value.Substring(0, pInt_Length > pStr_Value.Length ? pStr_Value.Length : pInt_Length);
        }

        /// <summary>
        /// 获得字符串左边一定长度的字符串(同vb中Left方法)
        /// </summary>
        /// <param name="pStr_Value">字符串</param>
        /// <param name="pInt_Length">长度</param>
        /// <returns>右边相应长度字符串</returns>
        public static string Right(string pStr_Value, int pInt_Length)
        {
            return pStr_Value.Substring(pInt_Length > pStr_Value.Length ? 0 : pStr_Value.Length - pInt_Length);
        }

        /// <summary>
        /// 获取访问者的IP地址 无视代理
        /// </summary>
        /// <returns></returns>
        public static string GetVisitorIPAddress()
        {
            string userHostAddress = string.Empty;

            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                string _x_forward = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(_x_forward))
                {
                    userHostAddress = _x_forward.Split(',')[0];
                }
            }
            else
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;

                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress))
            {
                bool f = System.Text.RegularExpressions.Regex.IsMatch(userHostAddress, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

                if (f == false)
                {
                    userHostAddress = "";
                }
            }            

            return userHostAddress;
        }

        /// <summary>
        /// Unicode编码转汉字
        /// </summary>
        /// <param name="unicodeString">Unicode编码字符串</param>
        /// <returns></returns>
        public static string ConvertUnicodeToGB(string unicodeString)
        {
            string[] strArray = unicodeString.Split(new string[] { @"\u" }, StringSplitOptions.None);
            string result = string.Empty;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Trim() == "" || strArray[i].Length < 2 || strArray.Length <= 1)
                {
                    result += i == 0 ? strArray[i] : @"\u" + strArray[i]; continue;
                }
                for (int j = strArray[i].Length > 4 ? 4 : strArray[i].Length; j >= 2; j--)
                {
                    try
                    {
                        result += char.ConvertFromUtf32(Convert.ToInt32(strArray[i].Substring(0, j), 16)) + strArray[i].Substring(j);
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 汉字转Unicode编码
        /// </summary>
        /// <param name="strGB">汉字字符串</param>
        /// <returns></returns>
        public static string ConvertGBToUnicode(string strGB)
        {
            char[] chs = strGB.ToCharArray();
            string result = string.Empty;
            foreach (char c in chs)
            {
                result += @"\u" + char.ConvertToUtf32(c.ToString(), 0).ToString("x");
            }
            return result;
        }

        /// <summary>
        /// 汉字转Unicode编码(英文或数字均补0到四位)
        /// </summary>
        /// <param name="strGB">汉字字符串</param>
        /// <returns></returns>
        public static string ConvertGBToUnicode4(string strGB)
        {
            char[] chs = strGB.ToCharArray();
            string result = string.Empty;
            string result1 = string.Empty;
            foreach (char c in chs)
            {
                result1 = char.ConvertToUtf32(c.ToString(), 0).ToString("x");
                if (result1.Length < 4)
                {
                    result1 = result1.PadLeft(4, '0');
                }
                result += @"\u" + result1;
            }
            return result;
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string UrlEncode(string strUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(strUrl))
                    return "";
                return HttpUtility.UrlEncode(strUrl);
            }
            catch
            {
                return strUrl;
            }
        }

        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string UrlDecode(string strUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(strUrl))
                    return "";
                return HttpUtility.UrlDecode(strUrl);
            }
            catch
            {
                return strUrl;
            }
        }

        /// <summary>
        /// HTML编码
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string HtmlEncode(string strValue)
        {
            try
            {
                if (string.IsNullOrEmpty(strValue))
                    return "";
                return HttpUtility.HtmlEncode(strValue);
            }
            catch
            {
                return strValue;
            }
        }

        /// <summary>
        /// HTML解码
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string HtmlDecode(string strValue)
        {
            try
            {
                if (string.IsNullOrEmpty(strValue))
                    return "";
                return HttpUtility.HtmlDecode(strValue);
            }
            catch
            {
                return strValue;
            }
        }

        public static bool IsValidEmailAddress(string email)
        {
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regex.IsMatch(email);
        }

        public static bool IsValidURL(string url)
        {
            Regex regex = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return regex.IsMatch(url);
        }

        public static bool OnlyCharacters(string character)
        {
            Regex regex = new Regex(@"^.[A-Za-z]+$");
            return regex.IsMatch(character);
        }

        public static bool OnlyNumber(string number)
        {
            Regex regex = new Regex(@"^.[0-9]*$");
            return regex.IsMatch(number);
        }

        public static bool IsValidDate(string date)
        {   
            //验证日期类型为yyyy-MM-dd
            Regex regex = new Regex(@"^((((19|20)(([02468][048])|([13579][26]))-02-29))|((20[0-9][0-9])|(19[0-9][0-9]))-((((0[1-9])|(1[0-2]))-((0[1-9])|(1\d)|(2[0-8])))|((((0[13578])|(1[02]))-31)|(((0[1,3-9])|(1[0-2]))-(29|30)))))$");
            return regex.IsMatch(date);
        }

        public static bool IsValidDateTime(string dateTime)
        {   
            //验证日期类型为yyyy-MM-dd hh:mm:ss
            Regex regex = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
            return regex.IsMatch(dateTime);
        }

        public static bool IsValidUSPhone(string phone)
        {
            Regex regex = new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
            return regex.IsMatch(phone);
        }

        public static bool IsValidUSZipCode(string zipcode)
        {
            Regex regex = new Regex(@"\d{5}(-\d{4})?");
            return regex.IsMatch(zipcode);
        }

        public static bool IsValidKorean(string korean)
        {
            Regex regex = new Regex(@"^.[\uac00-\ud7af\u1100-\u11FF\u3130-\u318f]+$");
            return regex.IsMatch(korean);
        }

        public static bool IsValidCNMobile(string mobile)
        {
            Regex regex = new Regex(@"^((\(\d{3}\))|(\d{3}\-))?13[0-9]\d{8}|15[0-9]\d{8}|18[0-9]\d{8}");
            return regex.IsMatch(mobile);
        }

        public static bool IsValidCNPhone(string phone)
        {
            Regex regex = new Regex(@"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}");
            return regex.IsMatch(phone);
        }

        public static bool IsValidCNZipCode(string zipcode)
        {
            Regex regex = new Regex(@"d{6}");
            return regex.IsMatch(zipcode);
        }

        public static bool IsValidCNID(string ID)
        {  
            //验证身份证是否为15位或18位
            Regex regex = new Regex(@"d{18}|d{15}");
            return regex.IsMatch(ID);
        }

        public static string ReplaceLowOrderASCIICharacters(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss < 32)))
                    info.AppendFormat("&#x{0:X};", ss);
                else info.Append(cc);
            }
            return info.ToString();
        }

        public static string StrToHtml(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            if (value.IndexOf('<') >= 0 && value.IndexOf('>') >= 0)
            {
                return value;
            }
            value = value.Replace("\r\n", "<br />");
            return value;
        }

        public static bool ConvertLong(string value, out long returnvalue)
        {
            returnvalue = -1;
            Regex rexIntNumber = new Regex("^([-]|[+])?[0-9]+$");
            if (rexIntNumber.IsMatch(value))
            {
                returnvalue = long.Parse(value);
            }
            if (returnvalue == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得MD5加密结果
        /// </summary>
        /// <param name="content">待加密的字符串</param>
        /// <returns>32位的16进制加密结果</returns>
        public static string GetMd5Hash(string content)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(content));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string FileUpload(HttpPostedFile hpf, string id,string ver,out string msg)
        {
            string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
            int FileSize = int.Parse(ConfigurationManager.AppSettings["FileSize"].ToString());
            string FileType = ConfigurationManager.AppSettings["FileType"].ToString();
            int ThumbnailWidth = int.Parse(ConfigurationManager.AppSettings["ThumbnailWidth"].ToString());

            int maxSize = 1024 * FileSize;
            string[] fileTypeList = FileType.Split(',');

            string fileExt = Path.GetExtension(hpf.FileName).ToLower();
            msg = string.Empty;
            bool f = true;

            if (hpf.InputStream == null || hpf.InputStream.Length > maxSize)
            {
                msg = "上传文件大小超过限制，最大只能" + FileSize.ToString() + "k。";
                f = false;
            }

            if (String.IsNullOrEmpty(fileExt) || fileTypeList.All(x => "." + x != fileExt))
            {
                msg = "只允许上传" + FileType + "类型的图片。";
                f = false;
            }

            string newFileName = string.Empty;
            if (f == true)
            {
                newFileName = id+"_"+ver + fileExt;
                string filePath = FilePath + newFileName;
                hpf.SaveAs(filePath);

                ImageFormat format = new ImageFormat(new Guid());

                if (fileExt == ".jpg")
                {
                    format = ImageFormat.Jpeg;
                }else if (fileExt == ".gif")
                {
                    format = ImageFormat.Gif;
                }
                else if (fileExt == ".png")
                {
                    format = ImageFormat.Png;
                }

                ReducedImage(ThumbnailWidth, filePath, format);
            }

            return newFileName;
        }

        public static bool DeleteFile(string FileName)
        {
            bool isdel = false;

            string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();

            string FullPath = FilePath+FileName;

            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);

                isdel = true;
            }

            return isdel;
        }


        public static bool ImgCallback()
        {
            return false;
        }
        /// <summary>
        /// 按大小缩放图片
        /// </summary>
        /// <param name="Width">缩放到的宽</param>
        /// <param name="Height">缩放到的高</param>
        /// <param name="targetFilePath">图片的名字</param>
        /// <returns>bool</returns>
        public static bool ReducedImage(int Width,string targetFilePath,ImageFormat format)
        {
            try
            {
                Image ResourceImage = Image.FromFile(targetFilePath);
                double Percent = (double)Width / ResourceImage.Width;
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ImgCallback);
                int ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
                int ImageHeight = (ResourceImage.Height) * ImageWidth / ResourceImage.Width;//等比例缩放
                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);
                string filename = Path.GetFileNameWithoutExtension(targetFilePath);
                targetFilePath = targetFilePath.Replace(filename, filename + "_s");
                ReducedImage.Save(@targetFilePath, format);
                ReducedImage.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 按百分比缩放
        /// </summary>
        /// <param name="Percent">小数：0.4表示百分之40</param>
        /// <param name="targetFilePath">图片的名称</param>
        /// <returns>bool</returns>
        public static bool ReducedImage(double Percent, string targetFilePath, ImageFormat format)
        {
            try
            {
                Image ResourceImage = Image.FromFile(targetFilePath);
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ImgCallback);
                int ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
                int ImageHeight = (ResourceImage.Height) * ImageWidth / ResourceImage.Width;//等比例缩放
                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);
                string filename = Path.GetFileNameWithoutExtension(targetFilePath);
                targetFilePath = targetFilePath.Replace(filename, filename + "_s");
                ReducedImage.Save(@targetFilePath,format);
                ReducedImage.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
