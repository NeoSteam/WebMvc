using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DBUtility;
using NeoSteam.Model;
using System.Text;

namespace NeoSteam.SQLServerDAL
{
    public partial class PlayInfoSQL
    {
        private readonly string CONN_STRING_NON_DTC = SQLConnString.GetConnSting();
        private PlayInfo SetOneRow(SqlDataReader rd)
        {
            PlayInfo row = new PlayInfo();

            row.Pid = int.Parse(rd["Pid"].ToString());
            row.Uid = int.Parse(rd["Uid"].ToString());
            row.NickName = rd["NickName"].ToString();
            row.Psex = int.Parse(rd["Psex"].ToString());
            row.Country = int.Parse(rd["Country"].ToString());
            row.Job = int.Parse(rd["Job"].ToString());
            row.Plevel = int.Parse(rd["Plevel"].ToString());
            row.Race = int.Parse(rd["Race"].ToString());
            return row;
        }
        #region 添加角色
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int AddPlayInfo(PlayInfo item)
        {
            string SQL_NEW = @"INSERT INTO [PlayInfo] 
                            (uid,nickname,psex,country,job,plevel,race) 
                            VALUES 
                            (@uid,@nickname,@psex,@country,@job,@plevel,@race)";

            SqlParameter[] newParms = new SqlParameter[] {
            new SqlParameter("@uid", SqlDbType.Int),
            new SqlParameter("@nickname", SqlDbType.NVarChar,150),
            new SqlParameter("@psex", SqlDbType.Int),
            new SqlParameter("@country", SqlDbType.Int),
            new SqlParameter("@job", SqlDbType.Int),
            new SqlParameter("@plevel", SqlDbType.Int),
            new SqlParameter("@race", SqlDbType.Int),
      
            };

            newParms[0].Value = item.Uid;
            newParms[1].Value = item.NickName;
            newParms[2].Value = item.Psex;
            newParms[3].Value = item.Country;
            newParms[4].Value = item.Job;
            newParms[5].Value = item.Plevel;
            newParms[6].Value = item.Race;

            using (SqlConnection conn = new SqlConnection(CONN_STRING_NON_DTC))
            {
                int val = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, SQL_NEW, newParms);
                //LogSQL.AddLog(SQL_NEW, newParms);
                return val;
            }

        }
        #endregion
        #region 角色列表
        public List<PlayInfo> GetAllPlayInfo()
        {
            string sql = "SELECT * FROM [PlayInfo] with(nolock) where 1=1";

            List<PlayInfo> list = new List<PlayInfo>();

            using (SqlDataReader rd = SqlHelper.ExecuteReader(CONN_STRING_NON_DTC, CommandType.Text, sql, null))
            {
                while (rd.Read())
                {
                    list.Add(SetOneRow(rd));
                }

                return list;
            }
        }
        /// <summary>
        /// 分页查询角色列表
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        public List<PlayInfo> GetAllPlayInfoPage(int pageindex, int pagesize, out int total)
        {
            total = 0;
            string sql = @"pkg_pagination";
            StringBuilder NEW_SQL = new StringBuilder();
            NEW_SQL.Append(" SELECT * FROM PlayInfo(NOLOCK) WHERE 1=1 ");
            SqlParameter[] newParms = new SqlParameter[] { 
                new SqlParameter("@QueryStr", SqlDbType.VarChar),    //sql语句
                new SqlParameter("@PageSize", SqlDbType.Int),   //每页显示数
                new SqlParameter("@PageCurrent", SqlDbType.Int),  //当前页面
                new SqlParameter("@FdShow", SqlDbType.NVarChar,500),//要显示的列名
                new SqlParameter("@FdOrder", SqlDbType.VarChar,500),//排序条件
                new SqlParameter("@FdOrderDirec", SqlDbType.Int),
                new SqlParameter("@Rows", SqlDbType.Int),
            };
            newParms[0].Value = NEW_SQL.ToString();
            newParms[1].Value = pagesize;
            newParms[2].Value = pageindex;
            newParms[3].Value = "*";
            newParms[4].Value = "Pid";
            newParms[5].Value = 1;
            newParms[6].Direction = ParameterDirection.Output;

            List<PlayInfo> list = new List<PlayInfo>();
            using (SqlDataReader rd = SqlHelper.ExecuteReader(CONN_STRING_NON_DTC, CommandType.StoredProcedure, sql, newParms))
            {
                while (rd.Read())
                {
                    PlayInfo row = SetOneRow(rd);
                    list.Add(row);
                }
            }
            total = Convert.ToInt32(newParms[6].Value.ToString());
            return list;
        }
        #endregion
    }
}
