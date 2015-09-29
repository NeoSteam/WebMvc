using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Global
{
    /// <summary>
    /// 提供POST GET Request to Http Service , 得到返回的服务请求的结果。
    /// </summary>
    public class WebRequestClient
    {
        // System.Net.HttpWebRequest
        private string _method = "GET";
        /// <summary>
        /// POST OR GET
        /// </summary>
        public string Method
        {
            set
            {
                if (value.ToUpper() == "POST" || value.ToUpper() == "GET")
                {
                    this._method = value.ToUpper();
                }
                else
                {
                    this._method = "GET";
                }
            }
            get
            {
                return this._method;
            }
        }
        /// <summary>
        /// 网络的NetworkCredential对象，在fire wall 需要的认证令牌
        /// </summary>
        public System.Net.NetworkCredential Credential
        {
            set;
            get;

        }
        /// <summary>
        /// Get Result Stream Buffer MAX Size,单位是字节
        /// </summary>
        public int MaxBufferSize
        {
            set;
            get;
        }
        /// <summary>
        /// Get Result Stream Buffer MIN Size,单位是字节
        /// </summary>
        public int MinBufferSize
        {
            set;
            get;
        }
        /// <summary>
        /// REQUEST URL
        /// </summary>
        public string RequestUri
        {
            set;
            get;
        }
        /// <summary>
        /// 链接的超时时间
        /// </summary>
        public int TimeOut
        {
            set;
            get;
        }
        /// <summary>
        /// 数据流的解析的编码名称
        /// </summary>
        public string CharSet//default UTF8
        {
            set;
            get;
        }
        /// <summary>
        /// 读写Stream的超时时间，单位是毫秒
        /// </summary>
        public int ReadWriteTimeout
        {
            set;
            get;

        }

        /// <summary>
        /// cookie 容器
        /// </summary>
        public CookieContainer Cookie
        {
            get;
            set;
        }

        private string _postdata = "";
        /// <summary>
        /// 提交的数据。
        /// </summary>
        /// <remarks>
        /// 格式："paramA=aaa&paramB=bbb"
        /// </remarks>
        public string PostData
        {
            get
            {
                return this._postdata;
            }
            set
            {
                this._postdata = "t=" + DateTime.Now.ToString() + "&" + value;
            }
        }
         
        /// <summary>
        /// Trace 的错误信息
        /// </summary>
        private string ErrMessage;
        /// <summary>
        /// 构造函数，设置缺省值
        /// </summary>
        public WebRequestClient()
        {
            MaxBufferSize = 65536;//64k
            MinBufferSize = 8192;//8k
            TimeOut = 5000;//5s
            ReadWriteTimeout = 5000;//5s
            CharSet = "UTF-8";
        }
        /// <summary>
        /// 建立 Http Server Client Object//
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns>HttpWebRequest</returns>

        private HttpWebRequest OpenHttpServer(string requestUri)
        {
            try
            {
                if (this.Method == "GET")
                {
                    requestUri += this.PostData == string.Empty ? "" : "?" + this.PostData;

                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);

                if (this.Cookie == null || this.Cookie.Count == 0)
                {
                    request.CookieContainer = new CookieContainer();
                    this.Cookie = request.CookieContainer;
                }
                else
                {
                    request.CookieContainer = this.Cookie;
                }

                request.Method = this.Method;
                request.Timeout = this.TimeOut;
                request.ReadWriteTimeout = ReadWriteTimeout;
                
                //request.UseDefaultCredentials = true;
                if (Credential == null)
                    request.UseDefaultCredentials = true;
                else
                    request.Credentials = this.Credential;

                if (this.Method == "POST")
                {
                    if(!string.IsNullOrEmpty(this.PostData))
                    {
                        request.ContentType = "application/x-www-form-urlencoded";

                        Stream myRequestStream = request.GetRequestStream();

                        using (StreamWriter sw = new StreamWriter(myRequestStream, Encoding.GetEncoding(this.CharSet)))
                        {
                            sw.Write(this.PostData);
                        }

                    }
                }

                return request;
            }
            catch (Exception ex)
            {
                //ErrMessage = string.Format("Source={0},Message={1}", ex.Source, ex.Message);
            }

            return null;

        }

        /// <summary>
        /// 发出http request（用于将处理数据返回给外部调用接口）
        /// </summary>
        /// <returns></returns>
        public bool CallHttpWebRequest()
        {
            try
            {
                if (this.Method == "GET")
                {
                    RequestUri += this.PostData == string.Empty ? "" : "?" + this.PostData;

                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RequestUri);

                if (this.Cookie == null || this.Cookie.Count == 0)
                {
                    request.CookieContainer = new CookieContainer();
                    this.Cookie = request.CookieContainer;
                }
                else
                {
                    request.CookieContainer = this.Cookie;
                }

                request.Method = this.Method;
                request.Timeout = this.TimeOut;
                request.ReadWriteTimeout = ReadWriteTimeout;

                //request.UseDefaultCredentials = true;
                if (Credential == null)
                    request.UseDefaultCredentials = true;
                else
                    request.Credentials = this.Credential;

                if (this.Method == "POST")
                {
                    if (!string.IsNullOrEmpty(this.PostData))
                    {
                        request.ContentType = "application/x-www-form-urlencoded";

                        Stream myRequestStream = request.GetRequestStream();

                        using (StreamWriter sw = new StreamWriter(myRequestStream, Encoding.GetEncoding(this.CharSet)))
                        {
                            sw.Write(this.PostData);
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                
            }

            return false;

        }


        /// <summary>
        /// 发送GET OR POST Request to Http Service , And Get Response Result
        /// </summary>
        /// <returns>string</returns>
        public string GetHttpRequestResult()
        {
            //WebTrace wtt = new WebTrace();

            StringBuilder sb;    // A WebException is thrown if HTTP request fails

            HttpWebRequest request = OpenHttpServer(this.RequestUri);
            if (request == null)
            {
                sb = new StringBuilder(ErrMessage);
                return sb.ToString();
            }

            try
            {
                //wtt.StartTrace("Execute the request and obtain the response stream");
                // Execute the request and obtain the response stream        
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //wtt.EndTrace();

                Stream responseStream = response.GetResponseStream();

                // Content-Length header is not trustable, but makes a good hint.        
                // Responses longer than int size will throw an exception here!   
                int length = (int)response.ContentLength;
                // Use Content-Length if between bufSizeMax and bufSizeMin        
                int bufSize = MinBufferSize;

                if (length > bufSize)
                    bufSize = length > MaxBufferSize ? MaxBufferSize : length;
                // Allocate buffer and StringBuilder for reading response        
                byte[] buf = new byte[bufSize];

                byte[] content = new byte[length+1];
                
                // Read response stream until end        
                int index=0;

                //wtt.StartTrace("Read response stream until end");

                while ((length = responseStream.Read(buf, 0, buf.Length)) != 0)
                {

                    Array.Copy(buf, 0, content, index, length);
                    index += length;

                }
                //wtt.EndTrace();

                responseStream.Close();

                if (CharSet.Length == 0)
                {
                    return System.Text.Encoding.Default.GetString(content);
                }
                else
                {
                    return System.Text.Encoding.GetEncoding(CharSet).GetString(content);
                }

            }
            catch (Exception ex)
            {
                //System.Web.HttpContext.Current.Response.Write(ex.Message + "<br>" +ex.StackTrace);
                //System.Web.HttpContext.Current.Response.End();
                //ErrMessage = string.Format("Source={0},Message={1}", ex.Source, ex.Message);
                return ErrMessage;

            }

        }

        /// <summary>
        /// 如果出错，则返回字符串error；否则，返回数据。
        /// </summary>
        /// <returns></returns>
        public string GetHttpRequestResult_1()
        {
            WebTrace wtt = new WebTrace();

            StringBuilder sb;    // A WebException is thrown if HTTP request fails

            HttpWebRequest request = OpenHttpServer(this.RequestUri);
            if (request == null)
            {
                return "error";
            }

            try
            {
                wtt.StartTrace("Execute the request and obtain the response stream");
                // Execute the request and obtain the response stream        
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                wtt.EndTrace();

                Stream responseStream = response.GetResponseStream();

                // Content-Length header is not trustable, but makes a good hint.        
                // Responses longer than int size will throw an exception here!   
                int length = (int)response.ContentLength;
                // Use Content-Length if between bufSizeMax and bufSizeMin        
                int bufSize = MinBufferSize;

                if (length > bufSize)
                    bufSize = length > MaxBufferSize ? MaxBufferSize : length;
                // Allocate buffer and StringBuilder for reading response        
                byte[] buf = new byte[bufSize];

                byte[] content = new byte[length + 1];

                // Read response stream until end        
                int index = 0;

                wtt.StartTrace("Read response stream until end");

                while ((length = responseStream.Read(buf, 0, buf.Length)) != 0)
                {

                    Array.Copy(buf, 0, content, index, length);
                    index += length;

                }
                wtt.EndTrace();

                responseStream.Close();

                if (CharSet.Length == 0)
                {
                    return System.Text.Encoding.Default.GetString(content);
                }
                else
                {
                    return System.Text.Encoding.GetEncoding(CharSet).GetString(content);
                }

            }
            catch (Exception ex)
            {
                return "error";
            }

        }
    }
}
