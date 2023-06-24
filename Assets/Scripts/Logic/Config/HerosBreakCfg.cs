/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class HerosBreakCfg
    {
        public Dictionary<string, HerosBreakData> AllData;
        public static HerosBreakData GetData(int pID)
        {
            return ConfigManager.Ins.m_HerosBreakCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class HerosBreakData
    {
        //突破ID
        public int ID;

        //突破属性随机组ID
        public int BreakAttrGroup;

        //突破项目
        public string BreakProject;

        /*
        Administrator:
<10000 等级上限
=99999 无等级上限
        */
        //等级上限
        public int LvlMax;

        //成长数值
        public int ResearchGrow;

        //突破基础消耗数值
        public int BaseCost;

        //ResID
        public int ResID;

        //PreID
        public List<int> PreID;

        //NextID
        public List<int> NextID;

    }
}
