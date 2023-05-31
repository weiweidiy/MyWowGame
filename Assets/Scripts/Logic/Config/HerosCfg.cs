/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class HerosCfg
    {
        public Dictionary<string, HerosData> AllData;
        public static HerosData GetData(int pID)
        {
            return ConfigManager.Ins.m_HerosCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class HerosData
    {
        //HeroID
        public int ID;

        //Hero品质
        public int Quality;

        //Hero名字
        public string Name;

        //Hero类型
        public int HeroType;

        //Hero显示
        public bool HeroShow;

        //Hero获取来源
        public int GetHero;

        //Hero获取来源描述
        public string GetHeroDes;

        //Hero技能
        public int SkillID;

        //技能解锁等级
        public int SkillStartLvl;

        //拥有属性1
        public int AdditionAtt1;

        //属性1起始等级
        public int Att1StartLvl;

        //Hero拥有属性1成长
        public float AdditionAtt1Grow;

        //Hero拥有属性2
        public int AdditionAtt2;

        //属性2起始等级
        public int Att2StartLvl;

        //拥有属性2成长
        public float AdditionAtt2Grow;

        //拥有属性3
        public int AdditionAtt3;

        //属性3起始等级
        public int Att3StartLvl;

        //拥有属性3成长
        public float AdditionAtt3Grow;

        //Hero拥有属性4
        public int AdditionAtt4;

        //属性4起始等级
        public int Att4StartLvl;

        //拥有属性4成长
        public float AdditionAtt4Grow;

        //Hero突破属性
        public int BreakAtt;

        //Hero突破等级
        public int BreakStartLvl;

        //突破属性
        public float BreakAttGrow;

        //ResID
        public int ResID;

        //HeroShowResID
        public int HeroShowResID;

    }
}
