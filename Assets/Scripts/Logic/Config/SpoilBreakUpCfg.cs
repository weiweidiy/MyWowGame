/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SpoilBreakUpCfg
    {
        public Dictionary<string, SpoilBreakUpData> AllData;
        public static SpoilBreakUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_SpoilBreakUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SpoilBreakUpData
    {
        //SpoilBreakID
        public int ID;

        //SpoilID
        public int SpoilID;

        //突破等级
        public int BreakLvl;

        //突破最大等级
        public int BreakMaxlvl;

        //等级需求
        public int Lvl;

        //突破消耗
        public int BreakCost;

        /*
        Administrator:
百分数
7.5 ： 提升 7.5%
62278： 提升62.3A%
        */
        //突破拥有攻击加成
        public float HasAtkAdditionBase;

        /*
        Administrator:
拥有属性加成=突破拥有攻击加成+突破等级*拥有攻击加成成长
        */
        //拥有攻击加成成长
        public float HasATKAdditionGrow;

        //突破拥有HP加成
        public float HasHPAdditionBase;

        /*
        Administrator:
拥有属性加成=突破拥有HP加成+突破等级*拥有HP加成成长
        */
        //拥有攻击HP成长
        public float HasHPAdditionGrow;

        //突破属性
        public int AttributeID;

        /*
        Administrator:
Administrator:
突破属性数值 += 突破属性成长
        */
        //突破属性成长
        public float AttributeGrow;

    }
}
