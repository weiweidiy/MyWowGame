/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class DJCfg
    {
        public Dictionary<string, DJData> AllData;
        public static DJData GetData(int pID)
        {
            return ConfigManager.Ins.m_DJCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class DJData
    {
        //DJID
        public int ID;

        //DJ消耗
        public int Cost;

        //DJ乐曲类型
        public List<int> MelodyType;

        //DJ乐曲权重
        public List<int> MelodyTypeWight;

        /*
        Administrator:
DJ属性，读Attribute表

        */
        //DJ属性
        public List<int> AttributeID;

        //DJ属性权重
        public List<int> AttributeWight;

    }
}
