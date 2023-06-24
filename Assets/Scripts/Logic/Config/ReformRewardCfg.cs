/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class ReformRewardCfg
    {
        public Dictionary<string, ReformRewardData> AllData;
        public static ReformRewardData GetData(int pID)
        {
            return ConfigManager.Ins.m_ReformRewardCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class ReformRewardData
    {
        //改造副本奖励ID
        public int ID;

        //战斗波次
        public int AttributeID;

        /*
        Administrator:
改造副本每波完成后,奖励科技点总值百分比
结果向下取整
        */
        //奖励比例
        public float Reward;

    }
}
