using DBUtility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoSteam.Model;

namespace NeoSteam.SQLServerDAL
{
    public partial class User_ExperienceSQL
    {
        private readonly string CONN_STRING_NON_DTC = SQLConnString.GetConnSting();
        private User_Experience SetOneRow(SqlDataReader rd)
        {
            User_Experience row = new User_Experience();

            row.ex_lv = rd["ex_lv"].ToString();
            row.exp_value = rd["exp_value"].ToString();

            return row;
        }
        public List<User_Experience> Get_User_Experience_List()
        {
            string sql = @"select ex_lv,exp_value from User_Experience";

            List<User_Experience> list = new List<User_Experience>();
            using (SqlDataReader rd = SqlHelper.ExecuteReader(CONN_STRING_NON_DTC, CommandType.Text, sql))
            {
                while (rd.Read())
                {
                    User_Experience row = SetOneRow(rd);
                    list.Add(row);
                }
            }
            return list;
        }
    }
}
