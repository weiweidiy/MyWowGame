using Logic.Config;
using System;
using System.Collections.Generic;

namespace Configs
{
    public partial class SpoilBreakUpCfg
    {
        public static void GetDataList(Func<SpoilBreakUpData, bool> perdicate, List<SpoilBreakUpData> result)
        {
            var keys = ConfigManager.Ins.m_SpoilBreakUpCfg.AllData.Keys;
            foreach (var key in keys)
            {
                var breakData = ConfigManager.Ins.m_SpoilBreakUpCfg.AllData[key];
                if (perdicate(breakData))
                {
                    result.Add(breakData);
                }
            }
        }

        public static int GetAllDataCount()
        {
            return ConfigManager.Ins.m_SpoilBreakUpCfg.AllData.Keys.Count;
        }
    }
}