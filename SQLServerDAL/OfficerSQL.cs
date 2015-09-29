using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBUtility;
using NeoSteam.Model;
using System.Data.SqlClient;
using System.Data;

namespace NeoSteam.SQLServerDAL
{
    public partial class OfficerSQL
    {
        private readonly string CONN_STRING_NON_DTC = SQLConnString.GetConnSting();
        private Officer SetOneRow(SqlDataReader rd)
        {
            Officer row = new Officer();

            row.Offid = rd["Offid"].ToString();
            row.Offname = rd["Offname"].ToString();
            row.Star = rd["Star"].ToString();
            row.Attribute = rd["Attribute"].ToString();
            row.Profession = rd["Profession"].ToString();
            row.Attack = rd["Attack"].ToString();
            row.Blood = rd["Blood"].ToString();
            row.Speed = rd["Speed"].ToString();
            row.Miss = rd["Miss"].ToString();
            row.Crit = rd["Crit"].ToString();
            row.Skill = rd["Skill"].ToString();
            row.Introduction = rd["Introduction"].ToString();
            row.States = rd["States"].ToString();
            return row;
        }
        /// <summary>
        /// 分页查询武将列表
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        public List<Officer> GetAllOfficerPage(int pageindex, int pagesize, out int total)
        {
            total = 0;
            string sql = @"pkg_pagination";
            StringBuilder NEW_SQL = new StringBuilder();
            NEW_SQL.Append(" SELECT * FROM Officer(NOLOCK) WHERE 1=1 and states = 1");
            SqlParameter[] newParms = new SqlParameter[] { 
                new SqlParameter("@QueryStr", SqlDbType.VarChar),    //sql语句
                new SqlParameter("@PageSize", SqlDbType.Int),   //每页显示数
                new SqlParameter("@PageCurrent", SqlDbType.Int),  //当前页面
                new SqlParameter("@FdShow", SqlDbType.NVarChar,500),//要显示的列名
                new SqlParameter("@FdOrder", SqlDbType.VarChar,500),//排序条件
                new SqlParameter("@FdOrderDirec", SqlDbType.Int),//升序降序
                new SqlParameter("@Rows", SqlDbType.Int),
            };
            newParms[0].Value = NEW_SQL.ToString();
            newParms[1].Value = pagesize;
            newParms[2].Value = pageindex;
            newParms[3].Value = "*";
            newParms[4].Value = "offid";
            newParms[5].Value = 0;
            newParms[6].Direction = ParameterDirection.Output;

            List<Officer> list = new List<Officer>();
            using (SqlDataReader rd = SqlHelper.ExecuteReader(CONN_STRING_NON_DTC, CommandType.StoredProcedure, sql, newParms))
            {
                while (rd.Read())
                {
                    Officer row = SetOneRow(rd);
                    list.Add(row);
                }
            }
            total = Convert.ToInt32(newParms[6].Value.ToString());
            return list;
        }
    }
}
