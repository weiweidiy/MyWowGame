/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class ItemCfg
    {
        public Dictionary<string, ItemData> AllData;
        public static ItemData GetData(int pID)
        {
            return ConfigManager.Ins.m_ItemCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class ItemData
    {
        //ID
        public int ID;

        //item名称
        public string Name;

        //item类型
        public int Type;

        //Item描述
        public string Des;

        //Item资源
        public string Res;

        //ItemShow资源
        public string ResShow;

    }
}
