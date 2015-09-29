using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoSteam.Model;
using System.Data.SqlClient;
using DBUtility;
using System.Data;

namespace NeoSteam.SQLServerDAL
{
    public partial class UserOfficerSQL
    {
        private readonly string CONN_STRING_NON_DTC = SQLConnString.GetConnSting();
        private UserOfficer SetOneRow(SqlDataReader rd)
        {
            UserOfficer row = new UserOfficer();
            row.UO_Id = rd["UO_Id"].ToString();
            row.UserId = rd["UserId"].ToString();
            row.Off_Level = rd["Off_Level"].ToString();
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
            row.Status = rd["Status"].ToString();
            return row;
        }
        public List<UserOfficer> GetUserOfficerList(int userid)
        {
            string sql = @"select * from UserOfficer where UserId = @UserId";
            SqlParameter[] newParms = new SqlParameter[]{
            new SqlParameter("@UserId", SqlDbType.Int),
            };
            newParms[0].Value = userid.ToString();
            List<UserOfficer> list = new List<UserOfficer>();
            using (SqlDataReader rd = SqlHelper.ExecuteReader(CONN_STRING_NON_DTC, CommandType.Text, sql, newParms))
            {
                while (rd.Read())
                {
                    UserOfficer row = SetOneRow(rd);
                    list.Add(row);
                }
            }
            return list;
        }
    }
}
