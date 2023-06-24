/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class FormationGroupCfg
    {
        public Dictionary<string, FormationGroupData> AllData;
        public static FormationGroupData GetData(int pID)
        {
            return ConfigManager.Ins.m_FormationGroupCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class FormationGroupData
    {
        //阵型组ID
        public int ID;

        /*
        阵型组内阵型等概率出现
        */
        //阵型ID
        public List<int> FormationID;

    }
}
