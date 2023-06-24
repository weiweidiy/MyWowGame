using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.UI.UIShop;
using Networks;

namespace Logic.Manager
{
    /// <summary>
    /// 商城管理
    /// </summary>
    public class ShopManager : Singleton<ShopManager>
    {
        public GameShopData m_ShopData;
        public Dictionary<int, GameShopBuyData> ShopMap { get; private set; }

        private EventGroup m_EventGroup = new();

        //初始化同步服务器相关数据
        public void Init(S2C_Login pLoginData)
        {
            //抽卡
            m_ShopSkillData = pLoginData.ShopSkillData;
            m_ShopPartnerData = pLoginData.ShopPartnerData;
            m_ShopEquipData = pLoginData.ShopEquipData;
            //商店
            m_ShopData = pLoginData.ShopData;
            ShopMap = new Dictionary<int, GameShopBuyData>();

            if (m_ShopData == null) return;
            foreach (var gameShopData in m_ShopData.BuyDataList)
            {
                ShopMap.Add(gameShopData.ID, gameShopData);
            }

            //注册
            m_EventGroup.Register(LogicEvent.TimeDayChanged, (i, o) => OnTimeDayChanged());
            m_EventGroup.Register(LogicEvent.TimeWeekChanged, (i, o) => OnTimeWeekChanged());
        }

        //注销
        public override void OnSingletonRelease()
        {
            m_EventGroup.Release();
        }

        //更新存在本地的商品数据
        private void UpdateShopItem(GameShopBuyData pShopBuyData)
        {
            if ((ShopType)pShopBuyData.Type == ShopType.Diamond) return;
            var shopData = GetShopData(pShopBuyData.ID);
            if (shopData == null) return;
            var itemList = shopData.ItemList;
            var countList = shopData.ItemCountList;
            var length = itemList.Count;
            for (var i = 0; i < length; i++)
            {
                switch ((ItemType)GetItemData(itemList[i]).Type)
                {
                    case ItemType.Coin:
                        GameDataManager.Ins.Coin += countList[i];
                        break;
                    case ItemType.Trophy:
                        GameDataManager.Ins.Trophy += countList[i];
                        break;
                }
            }
        }

        #region 通用

        public GameShopBuyData GetGameShopBuyData(int pId)
        {
            return ShopMap.TryGetValue(pId, out var data) ? data : null;
        }

        public ShopData GetShopData(int pId)
        {
            return ShopCfg.GetData(pId);
        }

        public ItemData GetItemData(int pId)
        {
            return ItemCfg.GetData(pId);
        }

        #endregion

        #region 商店

        public void DoShopBuy(int pId, ShopType shopType)
        {
            var pType = (int)shopType;
            NetworkManager.Ins.SendMsg(new C2S_ShopBuy()
            {
                ID = pId,
                Type = pType,
            });
        }

        public void OnShopBuyOrder(S2C_ShopBuyOrder pMsg)
        {
            //TODO:点击购买后进入其他流程，完成后进入购买流程
            EventManager.Call(LogicEvent.OnShopBuyOrder);
        }

        public async void OnShopBuy(S2C_ShopBuy pMsg)
        {
            //更新购买数据
            var gameShopBuyData = pMsg.Data;
            if (GetGameShopBuyData(gameShopBuyData.ID) == null)
            {
                ShopMap.Add(gameShopBuyData.ID, gameShopBuyData);
            }
            else
            {
                ShopMap[gameShopBuyData.ID] = gameShopBuyData;
            }

            //更新存在本地的商品数据
            UpdateShopItem(pMsg.Data);
            //通知收到商品购买消息事件
            EventManager.Call(LogicEvent.OnShopBuy, pMsg);
            //弹出获取礼包商品面板
            await UIManager.Ins.OpenUI<UIShopObtain>(pMsg.Data);
        }

        private void OnTimeDayChanged()
        {
            //清空每日商店购买次数
            foreach (var mapData in from mapData in ShopMap
                     let shopData = GetShopData(mapData.Value.ID)
                     where shopData != null
                     where (ShopGiftType)shopData.Type == ShopGiftType.Day
                     select mapData)
            {
                mapData.Value.Count = 0;
            }
        }

        private void OnTimeWeekChanged()
        {
            //清空每周商店购买次数
            foreach (var mapData in from mapData in ShopMap
                     let shopData = GetShopData(mapData.Value.ID)
                     where shopData != null
                     where (ShopGiftType)shopData.Type == ShopGiftType.Week
                     select mapData)
            {
                mapData.Value.Count = 0;
            }
        }

        #endregion

        #region 抽卡

        public GameShopCardData m_ShopSkillData;
        public GameShopCardData m_ShopPartnerData;
        public GameShopCardData m_ShopEquipData;

        public void OnDrawResult(S2C_DrawCard pMsg)
        {
            (int, List<int>) _Data = (pMsg.DrawCardType, pMsg.List);

            switch ((DrawCardType)pMsg.DrawCardType)
            {
                case DrawCardType.Skill:
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_8003, pMsg.List.Count);
                    break;
                case DrawCardType.Partner:
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_8004, pMsg.List.Count);
                    break;
                case DrawCardType.Equip:
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_8002, pMsg.List.Count);
                    break;
            }

            TaskManager.Ins.DoTaskUpdate(TaskType.TT_8001, pMsg.List.Count);

            EventManager.Call(LogicEvent.ShowDrawCardResult, _Data);
        }

        public void SendMsgC2SDrawCard(int type, DrawCardCostType costType)
        {
            var pMsg = new C2S_DrawCard()
            {
                DrawCardType = type,
                DrawCardCostType = (int)costType
            };
            NetworkManager.Ins.SendMsg(pMsg);
        }

        public void SendMsgC2SUpdateShopData(int type)
        {
            var pMsg = new C2S_UpdateDrawCardData()
            {
                DrawCardType = type
            };
            NetworkManager.Ins.SendMsg(pMsg);
        }

        public void On_S2C_UpdateShopData(S2C_UpdateDrawCardData pMsg)
        {
            (int, int, int, int, int) data = (pMsg.DrawCardType, pMsg.DrawCardId, pMsg.DrawCardLevel,
                pMsg.DrawCardExp, pMsg.DrawCardTotalExp);
            switch ((DrawCardType)pMsg.DrawCardType)
            {
                case DrawCardType.Skill:
                {
                    m_ShopSkillData.ID = pMsg.DrawCardId;
                    m_ShopSkillData.Level = pMsg.DrawCardLevel;
                    m_ShopSkillData.Exp = pMsg.DrawCardExp;
                    m_ShopSkillData.TotalExp = pMsg.DrawCardTotalExp;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_4002, m_ShopSkillData.TotalExp);
                }
                    break;
                case DrawCardType.Partner:
                {
                    m_ShopPartnerData.ID = pMsg.DrawCardId;
                    m_ShopPartnerData.Level = pMsg.DrawCardLevel;
                    m_ShopPartnerData.Exp = pMsg.DrawCardExp;
                    m_ShopPartnerData.TotalExp = pMsg.DrawCardTotalExp;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_4003, m_ShopPartnerData.TotalExp);
                }
                    break;
                case DrawCardType.Equip:
                {
                    m_ShopEquipData.ID = pMsg.DrawCardId;
                    m_ShopEquipData.Level = pMsg.DrawCardLevel;
                    m_ShopEquipData.Exp = pMsg.DrawCardExp;
                    m_ShopEquipData.TotalExp = pMsg.DrawCardTotalExp;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_4001, m_ShopEquipData.TotalExp);
                }
                    break;
            }

            EventManager.Call(LogicEvent.UpdateDrawCardData, data);
        }

        public List<int> GetSummonWeightList(int id)
        {
            var weight = new List<int>();
            var summonData = SummonCfg.GetData(id);
            weight.Add(summonData.NormalWight);
            weight.Add(summonData.AdvancedWight);
            weight.Add(summonData.RareWight);
            weight.Add(summonData.EpiclWight);
            weight.Add(summonData.LegendaryWight);
            weight.Add(summonData.MythicWight);
            weight.Add(summonData.TransWight);
            return weight;
        }

        #endregion
    }
}