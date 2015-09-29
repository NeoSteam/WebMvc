using System;
using System.Collections.Generic;
using System.Text;

namespace NeoSteam.Model
{
    public partial class PlayInfo
    {
        public int Pid { get; set; } //角色ID
        public int Uid { get; set; } //用户ID 外联
        public string NickName { get; set; } //昵称
        public int Psex { get; set; } //性别 0、男 1、女
        public int Country { get; set; } //国家 0、红国 1、蓝国
        //职业 0、战士 1、魔法师 2、流浪者 3、技术者
        //红国 A、武器大师 B、拥护者 C、追踪者 D、黑暗行者 E、吟唱者 F、元素使者 G、武器匠 H、鼓舞者
        //蓝国 I、武士 J、圣骑士 K、猎人 L、暗杀者 M、巫师 N、牧师 O、工匠 P、工程师
        public int Job { get; set; } 
        public int Plevel { get; set; } //级别
        public int Race { get; set; } //种族 0、人类 1、兽巨人 2、精灵 3、猿矮人
        
        //属性
        public int STR { get; set; } //=力量
        public int CON { get; set; } //=体力
        public int DEX { get; set; } //=敏捷
        public int INT { get; set; } //=智力
        public int ATK { get; set; } //=攻击力
        public int DEF { get; set; } //=防御力
        public int ATS { get; set; } //=魔法攻击力
        public int RES { get; set; } //=魔法防御力
        public int ASR { get; set; } //=攻击成功率
        public int DSR { get; set; } //=防御成功率
        public int HP { get; set; } //=HP
        public int MP { get; set; } //=MP
        public int STA { get; set; } //=STA
        public int NS { get; set; } //=NS
        public int EXP { get; set; } //=EXP
    }
}
