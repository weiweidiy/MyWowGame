/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class PartnerLvlUpCfg
    {
        public Dictionary<string, PartnerLvlUpData> AllData;
        public static PartnerLvlUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_PartnerLvlUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class PartnerLvlUpData
    {
        //ID
        public int ID;

        //升级消耗
        public int Cost;

    }
}
