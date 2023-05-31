using System;
using System.Collections.Generic;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Networks;

namespace Logic.Manager
{
    /// <summary>
    /// 商城管理
    /// </summary>
    public class ShopManager : Singleton<ShopManager>
    {
        public GameShopSkillData m_ShopSkillData;
        public GameShopPartnerData m_ShopPartnerData;
        public GameShopEquipData m_ShopEquipData;

        public void Init(S2C_Login pLoginData)
        {
            m_ShopSkillData = pLoginData.ShopSkillData;
            m_ShopPartnerData = pLoginData.ShopPartnerData;
            m_ShopEquipData = pLoginData.ShopEquipData;
        }

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
    }
}