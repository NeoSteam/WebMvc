using System;
using System.Collections.Generic;
using System.Text;

namespace NeoSteam.Model
{
    public partial class UserOfficer
    {
        public string UO_Id { get; set; } //角色ID
        public string UserId { get; set; } //用户ID
        public string Off_Level { get; set; } //角色等级
        public string Offid { get; set; } //武将ID
        public string Offname { get; set; } //武将姓名
        public string Star { get; set; } //星级
        public string Attribute { get; set; } //属性
        public string Profession { get; set; } //职业
        public string Attack { get; set; } //攻击力
        public string Blood { get; set; } //血量
        public string Speed { get; set; } //速度
        public string Miss { get; set; } //闪避
        public string Crit { get; set; } //暴击
        public string Skill { get; set; } //技能
        public string Introduction { get; set; } //人物介绍
        public string Status { get; set; } //状态

        public List<UserOfficer> UserOfficerList { get; set; } //武将队列
        public int combat_power { get; set; }//战斗力
    }
}
