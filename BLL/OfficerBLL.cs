using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoSteam.Model;
using NeoSteam.SQLServerDAL;


namespace NeoSteam.BLL
{
    public partial class OfficerBLL
    {
        private OfficerSQL sql_instance = new OfficerSQL();
        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<Officer> GetAllOfficerPage(int pageindex, int pagesize, out int total)
        {
            List<Officer> list = new List<Officer>();
            list = sql_instance.GetAllOfficerPage(pageindex, pagesize, out  total);
            foreach (Officer item in list)
            {
                item.Attribute = Global.BasedataStandard.GetNameByID(Global.BaseDataType.Attribute, item.Attribute, Global.eLanguage.Chinese);
                item.Profession = Global.BasedataStandard.GetNameByID(Global.BaseDataType.Profession, item.Profession, Global.eLanguage.Chinese);
            }
            return list;
        }
    }
}
