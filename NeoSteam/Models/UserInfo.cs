using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeoSteam.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }

        [StringLength(5, ErrorMessage = "*长度必须<5")]

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string RealName { get; set; }

        public string Email { get; set; }

        public DateTime CreateTime { get; set; }

        public int Enabled { get; set; }

    }
}