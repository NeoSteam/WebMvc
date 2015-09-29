using System;
using System.Text;

namespace Global
{
    /// <summary>
    /// 建一个静态类，用来包含要添加的扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 要添加的扩展方法必须为一个静态方法
        /// 此方法参数列表必须以this开始 第二个即为要扩展的数据类型，在这里就是要扩展string类型
        /// 此方法返回一个int型的值，即返回调用此方法字符串的长度，全角算2，半角算1。
        /// </summary>
        public static int RealLength(this string s)
        {
            return s.Length + System.Text.RegularExpressions.Regex.Matches(s, "[\u0080-\uffff]").Count;
        }

        /// <summary>
        /// 要添加的扩展方法必须为一个静态方法
        /// 此方法参数列表第一个参数表示要扩展哪一个类，第二个参数才表示此扩展方法的真正参数
        /// 依长度切字符串，全角字符为2长度，半角为1
        /// </summary>
        public static string CutString(this string s, int len)
        {

            string tempString = string.Empty;

            for (int i = 0, tempIndex = 0; i < s.Length; ++i, ++tempIndex)
            {
                if (System.Text.Encoding.UTF8.GetBytes(new char[] { s[i] }).Length > 1)
                {
                    ++tempIndex;
                }

                if (tempIndex >= len)
                {
                    break;
                }

                tempString += s[i];
            }

            return tempString;
        }

        /// <summary>
        /// 从右取字符串
        /// </summary>
        /// <param name="s">原串</param>
        /// <param name="len">长度</param>
        /// <returns>从右取出的字符串</returns>
        public static string Right(this string s, int len)
        {
            return s.Substring(len > s.Length ? 0 : s.Length - len);
        }

        /// <summary>
        /// 从左取字符串
        /// </summary>
        /// <param name="s">原串</param>
        /// <param name="len">长度</param>
        /// <returns>从左取出的字符串</returns>
        public static string Left(this string s, int len)
        {
            if (!string.IsNullOrEmpty(s))
                return s.Substring(0, len > s.Length ? s.Length : len);
            else
                return string.Empty;
        }
    }
}

