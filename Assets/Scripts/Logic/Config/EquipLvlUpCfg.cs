/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class EquipLvlUpCfg
    {
        public Dictionary<string, EquipLvlUpData> AllData;
        public static EquipLvlUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_EquipLvlUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class EquipLvlUpData
    {
        //ID
        public int ID;

        //升级消耗
        public int Cost;

    }
}
