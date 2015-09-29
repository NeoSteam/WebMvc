using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NeoSteam.BLL
{
    public partial class HomeBLL
    {
        private string Post_Url = "";//提交地址
        string Key = "lInKs.ReWaRds_!#%@$^.ORDER";// 测试：5c7ac092477ab5ba01478bccc4a60016
        public string BtbAddOrder()
        {
            Post_Url = "http://114.215.143.35:1111/Order/AddOrder";//下单接口地址  测试
            string jsondata = "{";
            jsondata = "\"BOrderId\": \"B2014010800001\",";

            //string result = GetResult_Http(jsondata, 150000);
            return jsondata;
        }

        #region Http请求
        /// <summary>
        /// 提交申请并处理返回结果
        /// </summary>
        /// <param name="Parameter">连接参数字符串</param>
        /// <param name="timeout">等待响应时间单位:毫秒</param>
        public string GetResult_Http(string Parameter, int timeout)
        {
            string retStr = string.Empty;
            try
            {
                #region 设置POST参数
                /*充值申请接口*/
                string valpairs = Parameter;
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] b = encoding.GetBytes(valpairs);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Post_Url);
                request.Timeout = timeout;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = b.Length;
                System.IO.Stream sw = request.GetRequestStream();
                sw.Write(b, 0, b.Length);
                sw.Close();
                #endregion

                #region 获取Response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream resStream = response.GetResponseStream();
                        System.IO.StreamReader streamReader = new StreamReader(resStream, System.Text.Encoding.GetEncoding("UTF-8"));
                        retStr = streamReader.ReadToEnd();
                        streamReader.Close();
                        resStream.Close();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retStr;
        }
        #endregion
    }
}
