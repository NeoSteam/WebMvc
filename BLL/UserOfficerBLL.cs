using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoSteam.SQLServerDAL;
using NeoSteam.Model;

namespace NeoSteam.BLL
{
    public partial class UserOfficerBLL
    {
        private UserOfficerSQL sql_instance = new UserOfficerSQL();
        public List<UserOfficer> GetUserOfficerList(int userid,out int power)
        {
            power = 0;
            List<UserOfficer> list = new List<UserOfficer>();
            list = sql_instance.GetUserOfficerList(userid);
            foreach(UserOfficer item in list)
            {
                power += user_power(item.Attack, item.Blood, item.Speed,item.Miss,item.Crit);
                item.Attribute = Global.BasedataStandard.GetNameByID(Global.BaseDataType.Attribute, item.Attribute, Global.eLanguage.Chinese);
                item.Profession = Global.BasedataStandard.GetNameByID(Global.BaseDataType.Profession, item.Profession, Global.eLanguage.Chinese);
            }
            return list;
        }
        /// <summary>
        /// *武将战斗力计算*
        /// </summary>
        /// <param name="attack"></param>
        /// <param name="blood"></param>
        /// <param name="speed"></param>
        /// <param name="miss"></param>
        /// <param name="crit"></param>
        /// <returns></returns>
        public int user_power(string attack, string blood, string speed, string miss, string crit)
        {
            int officer_power = int.Parse(attack) + int.Parse(blood) 
                                + int.Parse(speed) + int.Parse(miss) 
                                + int.Parse(crit);
            return officer_power;
        }
    }
}
