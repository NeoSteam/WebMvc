using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace NeoSteam.Models
{
    public class PlayInfoModels
    {
        public int Pid { get; set; }

        
        [DisplayName("昵称")]
        public string NickName { get; set; }

        public int Psex { get; set; }
    }
}