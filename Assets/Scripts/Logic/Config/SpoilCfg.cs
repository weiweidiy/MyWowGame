/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SpoilCfg
    {
        public Dictionary<string, SpoilData> AllData;
        public static SpoilData GetData(int pID)
        {
            return ConfigManager.Ins.m_SpoilCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SpoilData
    {
        //SpoilID
        public int ID;

        //SpoilGroup
        public int GroupID;

        //SpoilGroupName
        public string GroupName;

        //Spoil名称
        public string SpoilName;

        //Spoil属性
        public int AttributeID;

        //Spoil升级成长
        public float AttributeGrow;

        //Spoil属性最大等级
        public int MaxAttribute;

        /*
        Administrator:
百分数
7.5 ： 提升 7.5%
62278： 提升62.3A%
        */
        //初始拥有攻击加成
        public float HasAtkAdditionBase;

        //拥有攻击加成成长
        public float HasATKAdditionGrow;

        //初始拥有HP加成
        public float HasHPAdditionBase;

        //拥有攻击HP成长
        public float HasHPAdditionGrow;

        //ResID
        public int ResID;

        //GroupResID
        public int GroupResID;

    }
}
