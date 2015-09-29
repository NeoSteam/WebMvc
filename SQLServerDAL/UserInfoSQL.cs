using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DBUtility;
using NeoSteam.Model;

namespace NeoSteam.SQLServerDAL
{
    public partial class UserInfoSQL
    {
        private readonly string CONN_STRING_NON_DTC = SQLConnString.GetConnSting();
        private UserInfo SetOneRow(SqlDataReader rd)
        {
            UserInfo row = new UserInfo();

            row.UserId = int.Parse(rd["UserId"].ToString());
            row.UserName = rd["UserName"].ToString();
            row.PassWord = rd["PassWord"].ToString();
            row.RealName = rd["RealName"].ToString();
            row.Email = rd["Email"].ToString();
            row.CreateTime = DateTime.Parse(rd["CreateTime"].ToString());
            row.Enabled = rd["Enabled"].ToString();
            return row;
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserInfo GetOneUserInfo(string username, string password)
        {
            //string sql = "SELECT * FROM [UserInfo] with(nolock) WHERE username = @username and password=@password";
            string stored_name = "PKG_GetOneUser";
            SqlParameter[] newParms = new SqlParameter[] { 
            new SqlParameter("@username", SqlDbType.VarChar),
            new SqlParameter("@password", SqlDbType.VarChar),
            };
            newParms[0].Value = username;
            newParms[1].Value = password;
            UserInfo row = new UserInfo();
            using (SqlDataReader rd = SqlHelper.ExecuteReader(CONN_STRING_NON_DTC, CommandType.StoredProcedure, stored_name, newParms))
            {
                if (rd.Read())
                {
                    row = SetOneRow(rd);
                }
                return row;
            }
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int AddUserInfo(string username, string password)
        {
            string sql_new = @"insert into userinfo 
                            (username,password)
                            values
                            (@username,@password)";
            SqlParameter[] newParms = new SqlParameter[]{
            new SqlParameter("@username",SqlDbType.NVarChar,150),
            new SqlParameter("@password",SqlDbType.NVarChar,150),
            };
            newParms[0].Value = username;
            newParms[1].Value = password;
            using (SqlConnection conn = new SqlConnection(CONN_STRING_NON_DTC))
            {
                int val = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql_new, newParms);
                return val;
            }
        }
        public int Exist_UserInfo(string username)
        {
            string sql = @"select count(1) from userinfo where username = @username";
            SqlParameter[] newParms = new SqlParameter[]{
            new SqlParameter("@username",SqlDbType.NVarChar,150),
            };
            newParms[0].Value = username;
            using (SqlConnection conn = new SqlConnection(CONN_STRING_NON_DTC))
            {
                object val = SqlHelper.ExecuteScalar(conn, CommandType.Text, sql, newParms);
                return val == null ? 0 : int.Parse(val.ToString());
            }
        }
    }
}
