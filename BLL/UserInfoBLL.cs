using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NeoSteam.Model;
using NeoSteam.SQLServerDAL;

namespace NeoSteam.BLL
{
    public partial class UserInfoBLL
    {
        private UserInfoSQL sql_instance = new UserInfoSQL();

        public static void ExitSessionInfo()
        {
            HttpContext.Current.Session.Abandon();  //清空session
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserInfo GetOneUserInfo(string username, string password)
        {
            return sql_instance.GetOneUserInfo(username, password);
        }

        public int AddUserInfo(string username, string password)
        {
            return sql_instance.AddUserInfo(username,password);
        }
        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int Exist_UserInfo(string username)
        {
            return sql_instance.Exist_UserInfo(username);
        }
        /// <summary>
        /// 登录session处理
        /// </summary>
        public class CheckLogin
        {
            private static readonly string SessionName = ConfigurationManager.AppSettings["SessionName"].ToString();

            public static void ExitSessionInfo()
            {
                HttpContext.Current.Session.Abandon();   //清空session
            }

            public static UserInfo GetSessionInfo()
            {
                object s = HttpContext.Current.Session[SessionName];
                if (s != null)
                {
                    return s as UserInfo;
                }
                else
                {
                    return null;
                }
            }
            public static bool SetSessionInfo(UserInfo ui)
            {
                if (ui == null)
                {
                    return false;
                }
                else
                {
                    HttpContext.Current.Session[SessionName] = ui;
                    return true;
                }
            }
        }
    }
}
