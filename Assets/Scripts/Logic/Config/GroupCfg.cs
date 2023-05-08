/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class GroupCfg
    {
        public Dictionary<string, GroupData> AllData;
        public static GroupData GetData(int pID)
        {
            return ConfigManager.Ins.m_GroupCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class GroupData
    {
        //随机组ID
        public int ID;

        //随机组类型
        public int RandomGroupType;

        //道具ID
        public List<int> ItemID;

        //道具权重
        public List<int> ItemWight;

        //道具最小数量
        public List<int> ItemMinCount;

        //道具最大数量
        public List<int> ItemMaxCount;

    }
}
