/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class DigSparkCfg
    {
        public Dictionary<string, DigSparkData> AllData;
        public static DigSparkData GetData(int pID)
        {
            return ConfigManager.Ins.m_DigSparkCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class DigSparkData
    {
        /*
        ID= 挖矿层数

        */
        //挖矿ID
        public int ID;

        //火花塞组
        public int SparkGroup;

    }
}
