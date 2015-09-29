using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace NeoSteam.Models
{
    public class UserInfoModels
    {
        public int Userid { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public int Enabled { get; set; }
    }
}