/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class EngineCfg
    {
        public Dictionary<string, EngineData> AllData;
        public static EngineData GetData(int pID)
        {
            return ConfigManager.Ins.m_EngineCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class EngineData
    {
        /*
        等级=ID
        */
        //引擎ID
        public int ID;

        //基础攻击力比例
        public float BaseAdditionATK;

        //基础体力比例
        public float BaseAdditionHP;

        //消耗科技点经验
        public int CostGear;

        //消耗科技点
        public int Costtech;

    }
}
