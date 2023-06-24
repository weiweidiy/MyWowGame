/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class ReformHeroSlotCfg
    {
        public Dictionary<string, ReformHeroSlotData> AllData;
        public static ReformHeroSlotData GetData(int pID)
        {
            return ConfigManager.Ins.m_ReformHeroSlotCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class ReformHeroSlotData
    {
        //ID改造计划
        public int ID;

        //英雄数量
        public int HeroCount;

        //加成效果
        public string AddonEffect;

        //属性
        public int AttributeID;

    }
}
