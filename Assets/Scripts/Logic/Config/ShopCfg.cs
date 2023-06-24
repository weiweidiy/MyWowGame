/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class ShopCfg
    {
        public Dictionary<string, ShopData> AllData;
        public static ShopData GetData(int pID)
        {
            return ConfigManager.Ins.m_ShopCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class ShopData
    {
        /*
        Administrator:
（礼包ID要根据类型分类型预留数位）
        */
        //商品ID
        public int ID;

        //商品名字
        public string Name;

        /*
        Administrator:
标识礼包刷新逻辑逻辑
1 一次性礼包- 购买到限购次数后售罄
2 每日礼包- 购买后售罄,自然日刷新上架
3 每周礼包- 购买后售罄,自然周刷新上架

        */
        //商品刷新类型
        public int Type;

        /*
        Administrator:

0 无效
1 固定时间间隔 读：起始时间 终止时间 :2023-06-01 05:00:00 2023-06-11 05:00:00
2 无时间限制 读 起始时间：2023-06-01 05:00:00
3 解锁条件，持续时间后终止 读 持续时间 : 864000
4 解锁条件，无时间限制

        */
        //时间类型
        public int TimeType;

        //起始时间
        public string BeginTime;

        /*
        Administrator:

        */
        //终止时间
        public string EndTime;

        /*
        Administrator:
以秒为单位
礼包持续时间
        */
        //持续时间
        public int ContinueTime;

        /*
        Administrator:
1  免费
2 金币
3 钻石
4 货币
代表这个礼包可以用什么类型货币购买，包含免费，需要问程序这个类型会不会有作弊可能）
        */
        //货币类型
        public int PayType;

        /*
        Administrator:
（根据前面填的类型对该类型填具体的数量）
        */
        //价格
        public int PayNum;

        /*
        Administrator:
0 不限购
其他限购次数
        */
        //限购次数
        public int LimitCount;

        /*
        Administrator:
item ID
        */
        //货物list
        public List<int> ItemList;

        //货物数量list
        public List<int> ItemCountList;

        /*
        Administrator:
0 无效
1 有效

        */
        //有效性
        public bool Validity;

        //礼包排序
        public int Order;

        //商品主展示位
        public string MainGood;

        //礼包折扣标签
        public int DiscountTag;

        //价值倍数
        public int Value;

        /*
        Administrator:
0不显示
1新增、
2火热、
3特惠、
4限时
        */
        //礼包状态标签
        public int StatusTag;

        /*
        Administrator:
一次性礼包:
0 黄 
1 绿
每日礼包:
0 黄 <=4
1 紫 <=4
2 黄 >=5 
每周礼包:
0 黄 <=4
1 紫 <=4
2 黄 >=5 
        */
        //礼包底框
        public int Background;

    }
}
