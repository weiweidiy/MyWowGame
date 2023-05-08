/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SummonCfg
    {
        public Dictionary<string, SummonData> AllData;
        public static SummonData GetData(int pID)
        {
            return ConfigManager.Ins.m_SummonCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SummonData
    {
        //卡池ID
        public int ID;

        //卡池类型
        public int Type;

        //卡池等级
        public int Level;

        //卡池升级经验
        public int LevelExp;

        //普通权重(白卡权重)
        public int NormalWight;

        //高级权重(绿卡权重)
        public int AdvancedWight;

        //稀有权重(蓝卡权重)
        public int RareWight;

        //史诗权重(紫卡权重)
        public int EpiclWight;

        //传说权重(橙卡权重)
        public int LegendaryWight;

        //神话权重(青卡权重)
        public int MythicWight;

        //超越权重(红卡权重)
        public int TransWight;

    }
}
