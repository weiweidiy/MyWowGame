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

        /*
        钻石	101	货币
金币	102	货币
零件	103	货币
统领材料	104	货币
古代金属	105	货币
钢铁	106	货币
音符	107	货币
鸡腿	108	货币
蘑菇	109	货币
氪金矿石	110	货币
武器	1	武器
防具	2	防具
技能	3	技能
伙伴	4	伙伴
火花塞	5	火花塞
气缸	6	气缸
战利品	7	战利品
礼包	8	礼包
英雄技能	10	英雄技能
宝箱	1000	宝箱
道具	2000	道具
钻石钥匙	2101	钥匙
金币钥匙	2102	钥匙
功勋钥匙	2103	钥匙
音乐钥匙	2104	钥匙
改造钥匙	2105	钥匙
        */
        //item类型
        public int Type;

        //Item描述
        public string Des;

        //Item资源
        public string Res;

        //ItemShow资源
        public string ResShow;

        //品质
        public int Quality;

    }
}
