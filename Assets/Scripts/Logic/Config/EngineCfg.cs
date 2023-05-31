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
        //引擎ID
        public int ID;

        //类型
        public int Quality;

        /*
        Administrator:
显示为百分比

        */
        //引擎名称
        public string Name;

        //拥有加成攻击力比例
        public float HasAdditionATK;

        //拥有加成体力比例
        public float HasAdditionHP;

        //消耗齿轮
        public int CostGear;

        //改造次数
        public int ReformTime;

        //引擎属性随机组ID
        public List<int> EngineAttrGroup;

        //引擎属性随机组权重
        public List<int> Wight;

        //攻击成长
        public int AttGrow;

        //体力成长
        public int HPGrow;

        //ResID
        public int ResID;

    }
}
