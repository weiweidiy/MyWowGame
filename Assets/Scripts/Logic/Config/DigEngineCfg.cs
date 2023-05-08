/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class DigEngineCfg
    {
        public Dictionary<string, DigEngineData> AllData;
        public static DigEngineData GetData(int pID)
        {
            return ConfigManager.Ins.m_DigEngineCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class DigEngineData
    {
        //ID
        public int ID;

        //层数
        public int DigDeep;

        //引擎列表
        public List<int> Name;

        //引擎权重
        public List<int> Wight;

    }
}
