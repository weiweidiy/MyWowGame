/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class ReformCylinderGroupCfg
    {
        public Dictionary<string, ReformCylinderGroupData> AllData;
        public static ReformCylinderGroupData GetData(int pID)
        {
            return ConfigManager.Ins.m_ReformCylinderGroupCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class ReformCylinderGroupData
    {
        //气缸随机组ID
        public int ID;

        //卡池等级
        public int Level;

        //普通权重(白卡权重)0
        public int NormalWight;

        //高级权重(绿卡权重)1
        public int AdvancedWight;

        //稀有权重(蓝卡权重)2
        public int RareWight;

        //史诗权重(紫卡权重)3
        public int EpiclWight;

        //传说权重(橙卡权重)4
        public int LegendaryWight;

        //神话权重(青卡权重)5
        public int MythicWight;

        //超越权重(红卡权重)6
        public int TransWight;

    }
}
