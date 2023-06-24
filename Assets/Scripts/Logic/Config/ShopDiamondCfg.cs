/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class ShopDiamondCfg
    {
        public Dictionary<string, ShopDiamondData> AllData;
        public static ShopDiamondData GetData(int pID)
        {
            return ConfigManager.Ins.m_ShopDiamondCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class ShopDiamondData
    {
        /*
        Administrator:
钻石商城，
刷新周期：自然周 
刷新时间：周日05：00刷新
        */
        //商品ID
        public int ID;

        //商品名字
        public string Name;

        //购买金额
        public int Price;

        //商品
        public int Item;

        //奖励
        public int Reward;

        //额外奖励
        public int ExReward;

        //商品主展示位
        public string MainGood;

    }
}
