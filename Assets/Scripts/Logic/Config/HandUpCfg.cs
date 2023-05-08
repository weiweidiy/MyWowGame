/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class HandUpCfg
    {
        public Dictionary<string, HandUpData> AllData;
        public static HandUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_HandUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class HandUpData
    {
        //放置奖励ID
        public int ID;

        //关卡系数
        public int Level;

        //掉落组ID
        public List<int> GroupID;

        //权重
        public List<int> Wight;

    }
}
