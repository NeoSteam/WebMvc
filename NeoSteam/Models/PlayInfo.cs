using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeoSteam.Models
{
    public class PlayInfo
    {
        public int Pid { get; set; }

        [StringLength(5, ErrorMessage = "*长度必须<5")]

        public string NickName { get; set; }

        public int Psex { get; set; }
    }
}