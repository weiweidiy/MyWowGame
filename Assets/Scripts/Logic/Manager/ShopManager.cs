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
            m_ShopSkillData = pLoginData.m_ShopSkillData;
            m_ShopPartnerData = pLoginData.m_ShopPartnerData;
            m_ShopEquipData = pLoginData.m_ShopEquipData;
        }

        public void OnDrawResult(S2C_DrawCard pMsg)
        {
            (int, List<int>) _Data = (pMsg.m_DrawCardType, pMsg.m_List);

            switch ((DrawCardType)pMsg.m_DrawCardType)
            {
                case DrawCardType.Skill:
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_8003, pMsg.m_List.Count);
                    break;
                case DrawCardType.Partner:
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_8004, pMsg.m_List.Count);
                    break;
                case DrawCardType.Equip:
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_8002, pMsg.m_List.Count);
                    break;
            }
            
            TaskManager.Ins.DoTaskUpdate(TaskType.TT_8001, pMsg.m_List.Count);
            
            EventManager.Call(LogicEvent.ShowDrawCardResult, _Data);
        }

        public void SendMsgC2SDrawCard(int type, DrawCardCostType costType)
        {
            var pMsg = new C2S_DrawCard()
            {
                m_DrawCardType = type,
                m_DrawCardCostType = (int)costType
            };
            NetworkManager.Ins.SendMsg(pMsg);
        }

        public void SendMsgC2SUpdateShopData(int type)
        {
            var pMsg = new C2S_UpdateDrawCardData()
            {
                m_DrawCardType = type
            };
            NetworkManager.Ins.SendMsg(pMsg);
        }

        public void On_S2C_UpdateShopData(S2C_UpdateDrawCardData pMsg)
        {
            (int, int, int, int, int) data = (pMsg.m_DrawCardType, pMsg.m_DrawCardId, pMsg.m_DrawCardLevel,
                pMsg.m_DarwCardExp, pMsg.m_DrawCardTotalExp);
            switch ((DrawCardType)pMsg.m_DrawCardType)
            {
                case DrawCardType.Skill:
                {
                    m_ShopSkillData.m_ID = pMsg.m_DrawCardId;
                    m_ShopSkillData.m_Level = pMsg.m_DrawCardLevel;
                    m_ShopSkillData.m_Exp = pMsg.m_DarwCardExp;
                    m_ShopSkillData.m_TotalExp = pMsg.m_DrawCardTotalExp;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_4002, m_ShopSkillData.m_TotalExp);
                }
                    break;
                case DrawCardType.Partner:
                {
                    m_ShopPartnerData.m_ID = pMsg.m_DrawCardId;
                    m_ShopPartnerData.m_Level = pMsg.m_DrawCardLevel;
                    m_ShopPartnerData.m_Exp = pMsg.m_DarwCardExp;
                    m_ShopPartnerData.m_TotalExp = pMsg.m_DrawCardTotalExp;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_4003, m_ShopPartnerData.m_TotalExp);
                }
                    break;
                case DrawCardType.Equip:
                {
                    m_ShopEquipData.m_ID = pMsg.m_DrawCardId;
                    m_ShopEquipData.m_Level = pMsg.m_DrawCardLevel;
                    m_ShopEquipData.m_Exp = pMsg.m_DarwCardExp;
                    m_ShopEquipData.m_TotalExp = pMsg.m_DrawCardTotalExp;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_4001, m_ShopEquipData.m_TotalExp);
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