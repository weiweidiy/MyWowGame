/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SpoilLvlUpCfg
    {
        public Dictionary<string, SpoilLvlUpData> AllData;
        public static SpoilLvlUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_SpoilLvlUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SpoilLvlUpData
    {
        //ID
        public int ID;

        //Spoil升级消耗
        public int SpoilCost;

    }
}
