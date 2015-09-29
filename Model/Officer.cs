using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoSteam.Model
{
    public partial class Officer
    {
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
        public string States { get; set; } //状态
    }
}
