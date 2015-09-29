using System;
using System.Collections.Generic;
using System.Text;

namespace NeoSteam.Model
{
    public partial class UserInfo
    {
        public int UserId { get; set; } //用户ID
        public string UserName { get; set; }//用户帐号
        public string PassWord { get; set; }//密码
        public string RealName { get; set; }//真实姓名
        public string Email { get; set; }//邮箱
        public DateTime CreateTime { get; set; }//创建时间
        public string Enabled { get; set; }//账户状态 0开通 1关闭
    }
}
 