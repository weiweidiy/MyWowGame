/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class HerosBreakUpCfg
    {
        public Dictionary<string, HerosBreakUpData> AllData;
        public static HerosBreakUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_HerosBreakUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class HerosBreakUpData
    {
        //HeroBreakID
        public int ID;

        //Hero突破消耗
        public int CostBase;

        //Hero突破消耗成长
        public int CostGrow;

        //Hero突破消耗体验
        public int CostExp;

    }
}
