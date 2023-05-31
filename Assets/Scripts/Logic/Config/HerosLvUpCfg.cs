/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class HerosLvUpCfg
    {
        public Dictionary<string, HerosLvUpData> AllData;
        public static HerosLvUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_HerosLvUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class HerosLvUpData
    {
        //Hero升级ID
        public int ID;

        //Hero升级消耗基础值
        public int CostBase;

        //Hero升级消耗成长
        public int CostGrow;

        //Hero升级消耗体验
        public int CostExp;

    }
}
