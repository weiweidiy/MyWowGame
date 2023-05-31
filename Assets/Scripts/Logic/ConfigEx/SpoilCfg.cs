using Logic.Config;
using System;
using System.Collections.Generic;

namespace Configs
{
    public partial class SpoilCfg
    {
        public static void GetDataList(Func<SpoilData, bool> perdicate , List<SpoilData> result)
        {
            var keys = ConfigManager.Ins.m_SpoilCfg.AllData.Keys;
            foreach (var key in keys)
            {
                var spoilData = ConfigManager.Ins.m_SpoilCfg.AllData[key];
                if (perdicate(spoilData))
                {
                    result.Add(spoilData);
                }
            }
        }

        public static int GetAllDataCount()
        {
            return ConfigManager.Ins.m_SpoilCfg.AllData.Keys.Count;
        }
    }
}