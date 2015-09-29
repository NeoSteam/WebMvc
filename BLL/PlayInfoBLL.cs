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
    public partial class PlayInfoBLL
    {
        private PlayInfoSQL sql_instance = new PlayInfoSQL();

        public static void ExitSessionInfo()
        {
            HttpContext.Current.Session.Abandon();  //清空session
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        public int AddPlayInfo(PlayInfo item)
        {
            return sql_instance.AddPlayInfo(item);
        }
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public List<PlayInfo> GetAllPlayInfo()
        {
            return sql_instance.GetAllPlayInfo();
        }
        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<PlayInfo> GetAllPlayInfoPage(int pageindex, int pagesize, out int total)
        {
            return sql_instance.GetAllPlayInfoPage(pageindex, pagesize, out  total);
        }
    }
}
