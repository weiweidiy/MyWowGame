/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SpoilUnlockCfg
    {
        public Dictionary<string, SpoilUnlockData> AllData;
        public static SpoilUnlockData GetData(int pID)
        {
            return ConfigManager.Ins.m_SpoilUnlockCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SpoilUnlockData
    {
        //Spoil解锁ID
        public int ID;

        //Spoil解锁消耗
        public int SpoilUnlockCost;

        //SpoilID
        public int SpoilID;

    }
}
