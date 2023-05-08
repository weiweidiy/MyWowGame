/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SkillLvlUpCfg
    {
        public Dictionary<string, SkillLvlUpData> AllData;
        public static SkillLvlUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_SkillLvlUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SkillLvlUpData
    {
        //ID
        public int ID;

        //升级消耗
        public int Cost;

    }
}
