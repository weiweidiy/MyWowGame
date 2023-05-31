/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class DigResearchCfg
    {
        public Dictionary<string, DigResearchData> AllData;
        public static DigResearchData GetData(int pID)
        {
            return ConfigManager.Ins.m_DigResearchCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class DigResearchData
    {
        //研究ID
        public int ID;

        //研究属性随机组ID
        public int ResearchAttrGroup;

        //研究项目
        public string ResearchProject;

        //等级上限
        public int LvlMax;

        //研究成长数值
        public int ResearchGrow;

        //研究基础消耗数值
        public int BaseCost;

        //研究成长消耗数值
        public int GrowCost;

        //研究基础消耗时间
        public int BaseCostTime;

        //研究成长消耗时间
        public int GrowCostTime;

        //ResID
        public int ResID;

    }
}
