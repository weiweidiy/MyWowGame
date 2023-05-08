/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class DigMapCfg
    {
        public Dictionary<string, DigMapData> AllData;
        public static DigMapData GetData(int pID)
        {
            return ConfigManager.Ins.m_DigMapCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class DigMapData
    {
        //地图ID
        public int ID;

        //第1列
        public int FirstC;

        //第2列
        public int SecondC;

        //第3列
        public int ThridC;

        //第4列
        public int ForthC;

        //奖励item类型
        public List<int> RewardType;

        //奖励权重
        public List<int> RewardWeight;

        //开启
        public List<int> Open;

    }
}
