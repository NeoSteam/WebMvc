using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoSteam.Model;
using NeoSteam.SQLServerDAL;

namespace NeoSteam.BLL
{
    //for (int i = 0; i < 1;i++ )
    //        {
    //            var a = User_ExperienceBLL.class1.instance();
    //            List<User_Experience> list = a.userlist;
    //        }
    public partial class User_ExperienceBLL
    {
        private User_ExperienceSQL sql_instance = new User_ExperienceSQL();
        public List<User_Experience> Get_User_Experience_List()
        {
            List<User_Experience> list = new List<User_Experience>();
            list = sql_instance.Get_User_Experience_List();
            return list;
        }
        public class class1
        {
            private static class1 a = new class1();
            public List<User_Experience> userlist;
            private class1()
            {
                User_ExperienceBLL user_expbll = new User_ExperienceBLL();
                userlist = user_expbll.Get_User_Experience_List();
            }
            public static class1 instance()
            {
                return a;
            }
        }
    }
}
