using System;
using System.Web.UI;
using System.Web;
using System.Collections.Specialized;

namespace Global
{
    
    /// <summary>    
    /// <para ><b>Cookie管理类: </b>CookieManager是一个Global的公用的功能集合，主要提供Cookie的读写的基本操作，此类不能被继承.</para>
    /// <para >包括老系统提供的一维和二维的Cookie的读写操作</para>
    /// <para><b>Coordinator: </b>Peter Lu</para>
   /// </summary>
    /// <remarks >
    /// <para >注意的是ASP.NET没有指定PATH的时候，ASP是/WebAppliaction，ASP.NET是/也就是根</para>
    /// <para >兼容ASP提供的方法都是以GFN开头的函数，strName是一个可能以'_'分割的二维Cookie.</para>
    /// <para >新增了一个DefaultExpires [Cookie的过期时间]的DateTime对象，也就是过期时间缺省值是 DateTime.MinValue , 也就是1900-1-1，在使用的过程中可以修改这个数值.</para>
    /// <para >如果给DateTime对象赋予null的时候，对应C#是DateTime.MinValue.</para>
    /// </remarks>
    /// <example >
    /// 
    /// <code  lang="CS" title="CookieManager">
    /// public partial class _Default : System.Web.UI.Page
    /// {
    ///     private CookieManager cookieManager;
    ///     protected void Page_Load(object sender, EventArgs e)
    ///     {
    ///         cookieManager = new Chrm.Cookie.CookieManager(this);
    ///         //如果没有对缺省过期时间赋值，过期时间是1900-01-01，也就是DateTime.MinValue  
    ///         cookieManager.DefaultExpires = DateTime.Now.AddDays(2);//缺省的过期时间:2天
    ///   
    ///         cookieManager.setCookie("peterlu", "cookievalue");
    ///         string cookievalue = cookieManager.getCookie("peterlu");
    ///         Response.write("cookie name=peterlu value=" + cookievalue);
    ///         // "start setCookie(string cookiename, string cookievalue, DateTime expires, string domain, string path)..";
    ///         cookieManager.setCookie("domaincookiename", "cookievalue", DateTime.MinValue, null, "/");//domain =null 
    ///         string temp = cookieManager.getCookieValue("domaincookiename");
    ///     }    
    /// }
    /// </code>
    /// </example>
    public sealed class CookieManager
    {
        private DateTime defaultExpires = DateTime.MinValue;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CookieManager()
        {
        }

        #region CookieManager Base members

        /// <summary>
        /// 得到一个Cookie的Value值
        /// </summary>
        /// <param name="cookiename">Cookie Name</param>
        /// <returns>Cookie的String 的值,如果没有返回string.Empty</returns>     
        public string getCookieValue(string cookiename)
        {
            if (HttpContext.Current.Request.Cookies[cookiename] != null)
                return System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookiename].Value);
            else
                return string.Empty;
            
        }
        /// <summary>
        /// 取得一个二维Cookie的值
        /// </summary>
        /// <param name="cookiename">Cookie的名称</param>
        /// <param name="subkey">二维的Key的名称</param>
        /// <returns>如果有则返回string的值，没有则返回string.Empty</returns>
        public string getSubCookieValue(string cookiename, string subkey)
        {
            if (HttpContext.Current.Request.Cookies[cookiename] != null)
            {
                string cookieValue = System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookiename].Value);
                string subKeyName = subkey + "=";
                if (cookieValue.IndexOf(subKeyName) == -1)
                    return string.Empty;
                else
                    return System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookiename][subkey]);
            }
            else
                return string.Empty;
            

        }
        /// <summary>
        /// property 过期时间的缺省值，缺省时间是7天
        /// </summary>
        public DateTime DefaultExpires
        {
            set
            {
                this.defaultExpires = value;
            }

            get
            {
                return this.defaultExpires;
            }
        }

        /// <summary>
        /// 得到Cookie的所有的二维Cookie的Values
        /// </summary>
        /// <param name="cookiename">Cookie 的名称</param>
        /// <returns>返回一个dictionary否则为null</returns>
        public NameValueCollection getCookieValues(string cookiename)
        {
            if (HttpContext.Current.Request.Cookies[cookiename] != null)
            {
                NameValueCollection values = HttpContext.Current.Request.Cookies[cookiename].Values;
                if (values != null)
                {
                    int vCount = values.Keys.Count;

                    for (int i = 0; i < vCount; i++)
                    {
                        values[values[i]] = System.Web.HttpUtility.UrlDecode(values[values[i]]);
                    }
                }
                    return values;
            }
            else
                return null;

        
        }
        /// <summary>
        /// 向客户端写入Cookie
        /// </summary>
        /// <param name="cookiename">Cookie的名称</param>
        /// <param name="cookievalue">Cookie的值</param>
        /// <param name="domain">cookie的Domian</param>
        /// <param name="path">Cookie 的Path</param>
        /// <param name="expires">Cookie的过期时间</param>
        
        public void setCookie(string cookiename, string cookievalue, DateTime expires, string domain, string path)
        {
            setCookie(cookiename, cookievalue, null, expires, domain ,path);
           
        }
    /// <summary>
    /// 向客户端写入Cookie
    /// </summary>
   /// <param name="cookiename">Cookie 的名称</param>
    /// <param name="cookievalue">Cookie的值</param>
    /// <param name="domain">Cookie的domain</param>
    /// <param name="path">Cookie的path</param>
        public void setCookie(string cookiename, string cookievalue, string domain,string path)
        {
            setCookie(cookiename, cookievalue, null, DateTime.MinValue, domain, path);
        }
        /// <summary>
        /// 写入Cookie
        /// </summary>
        /// <param name="cookiename">Cookie 的名称</param>
        /// <param name="cookievalue">Cookie的Value</param>
        public void setCookie(string cookiename, string cookievalue)
        {
            setCookie(cookiename, cookievalue, null, DateTime.MinValue, string.Empty, string.Empty);
        }
        /// <summary>
        /// 写入Cookie的公用函数
        /// 可以为cookie对象赋于多个key键值
        /// 设键/值如下:
        /// NameValueCollection mycol = new NameValueCollection();
        /// mycol.Add("key1", "value1");
        /// mycol.Add("key2", "value2");
        /// mycol.Add("key3", "value3");
        /// mycol.Add("key1", "value4");
        /// 结果为"key1:value1,value4;key2:value2;key3:value3"
        /// </summary>
        /// <param name="cookiename">Cookie的名称，不能是Empty</param>
        /// <param name="cookievalue">Cookie的值可以Empty</param>
        /// <param name="cookievalues">二维Cookie的Values,可以是null</param>
        /// <param name="expires">过期时间，可以是DateTime或DateTime.MinValue--null</param>
        /// <param name="domain">Cookie的Domain ,可以是Empty</param>
        /// <param name="path">Cookie的Path,可以是Empty</param>
        private void setCookie(string cookiename, string cookievalue, NameValueCollection cookievalues, DateTime expires, string domain, string path)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie == null)
                cookie = new HttpCookie(cookiename);
            //if (cookievalue.Length > 0)
            
            cookie.Value = System.Web.HttpUtility.UrlEncode (cookievalue);
            if (cookievalues != null)
            {
                cookie.Values.Clear();
                //foreach (string key in cookievalues.Keys)
                //{
                //    cookie.Values.Add(key, System.Web .HttpUtility .UrlEncode (cookievalues[key]));
                //}
                cookie.Values.Add(cookievalues);
            }
            if (domain.Length > 0)
                cookie.Domain = domain;
            if (path.Length > 0)
                cookie.Path = path;
            if (expires != DateTime.MinValue)
                cookie.Expires = expires;
            else
                cookie.Expires = defaultExpires;

            HttpContext.Current.Response.Cookies.Add(cookie);
        
        }

 
        /// <summary>
        /// 写入一个二维Cookie的一个键值
        /// </summary>
        ///  /// <param name="cookiename">Cookie的名称，不能是Empty</param>
        /// <param name="cookievalue">Cookie的值可以Empty</param>
        /// <param name="subkey">二维Cookie的Key</param>
        /// <param name="subvalue">二维Cookie的值</param>
        /// 
        /// <param name="expires">过期时间，可以是DateTime或DateTime.MinValue--null</param>
        /// <param name="domain">Cookie的Domain ,可以是Empty</param>
        /// <param name="path">Cookie的Path,可以是Empty</param>
        /// 
        private void setCookie(string cookiename, string cookievalue, string subkey,string subvalue, DateTime expires, string domain, string path)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie == null)
                cookie = new HttpCookie(cookiename);
            //if (cookievalue.Length > 0)
            cookie.Value = System.Web.HttpUtility .UrlEncode (cookievalue);

            if (subkey.Length > 0)
            {
                cookie.Values.Add(subkey, System.Web .HttpUtility .UrlEncode (subvalue));

            }

            if (domain.Length > 0)
                cookie.Domain = domain;
            if (path.Length > 0)
                cookie.Path = path;
            if (expires != DateTime.MinValue)
                cookie.Expires = expires;
            else
                cookie.Expires = defaultExpires;


            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Request.Cookies.Add(cookie);
        }

        /// <summary>
        /// 写入一个Cookie的二维键值的集合
        /// </summary>
        /// <param name="cookiename">Cookie的名称</param>
        /// <param name="cookievalue">Cookie的值</param>
        /// <param name="cookievlues">Cookie的二维键值的集合</param>
        /// <param name="expires">过期时间，可以是DateTime.MinValue</param>
        public void setCookie(string cookiename, string cookievalue, NameValueCollection cookievlues,DateTime expires,string domain)
        {
            setCookie(cookiename, cookievalue, cookievlues, expires, domain , string.Empty);            

        }
        /// <summary>
        /// 写入Cookie,用缺省的过期时间，缺省的Path ,ASP.NET写COOKIE的缺省的Path是/,也就是ROOT
        /// </summary>
        /// <param name="cookiename">Cookie的名称</param>
        /// <param name="cookievalue">Cookie的值</param>
        /// <param name="expires">Cookie的过期时间</param>
        public void setCookie(string cookiename, string cookievalue, DateTime expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];

            if (cookie == null)
            {
                cookie = new System.Web.HttpCookie(cookiename);
            }

            cookie.Value = System.Web .HttpUtility .UrlEncode (cookievalue);
            cookie.Expires = expires;
            HttpContext.Current.Response.Cookies.Add(cookie);
            
        }
        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookiename">Cookie的名称</param>
        public void deleteCookie(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {                
                cookie.Expires = DateTime.Now.AddDays(-1);// DateTime.MinValue;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookiename">Cookie的名称</param>
        public void deleteCookie(string cookiename, string domain)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(domain)) { cookie.Domain = domain; }
                cookie.Expires = DateTime.Now.AddDays(-1);// DateTime.MinValue;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        #endregion

    }
}
