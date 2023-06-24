using Logic.Config;
using System;
using System.Collections.Generic;

namespace Configs
{
    public partial class CylinderCfg
    {
        public static void GetDataList(Func<CylinderData, bool> perdicate, List<CylinderData> result)
        {
            var keys = ConfigManager.Ins.m_CylinderCfg.AllData.Keys;
            foreach (var key in keys)
            {
                var data = ConfigManager.Ins.m_CylinderCfg.AllData[key];
                if (perdicate(data))
                {
                    result.Add(data);
                }
            }
        }
    }

}