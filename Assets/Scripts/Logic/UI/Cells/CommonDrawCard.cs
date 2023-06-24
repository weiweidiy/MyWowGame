using System;
using System.Collections.Generic;
using Configs;
using DG.Tweening;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonDrawCard : MonoBehaviour
    {
        [LabelText("卡池类型")] public DrawCardType m_DrawCardType;
        public TextMeshProUGUI m_LevelText;
        public Button m_BtnProbability;
        public Image m_CanProcess;
        public TextMeshProUGUI m_TextProcess;
        public Button m_BtnLeftDrawCard;
        public TextMeshProUGUI m_LeftDrawCardText;
        public TextMeshProUGUI m_LeftDrawCardCost;
        public Button m_BtnRightDrawCard;
        public TextMeshProUGUI m_RightDrawCardText;
        public TextMeshProUGUI m_RightDrawCardCost;
        public Sprite m_LeftNormalImage;
        public Sprite m_RightNormalImage;
        public Sprite m_CantImage;

        // 记录值
        private int m_ID;
        private int m_Level;
        private int m_Exp;
        private int m_TotalExp;

        public EventGroup m_EventGroup = new();

        private void Awake()
        {
            m_BtnLeftDrawCard.onClick.AddListener(() => OnBtnDrawCardClick(DrawCardCostType.Draw11CardCost));
            m_BtnRightDrawCard.onClick.AddListener(() => OnBtnDrawCardClick(DrawCardCostType.Draw35CardCost));
            m_BtnProbability.onClick.AddListener(OnBtnProbabilityClick);
            m_EventGroup.Register(LogicEvent.UpdateDrawCardData, OnUpdateDrawCardData);
            m_EventGroup.Register(LogicEvent.DiamondChanged, (i, o) => OnDiamondChanged());
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        private void Start()
        {
            InitDrawCardData();
            Refresh();
        }

        private bool IsCanDrawCard(DrawCardCostType type)
        {
            var cost = 0;
            switch (type)
            {
                case DrawCardCostType.Draw11CardCost:
                    cost = GameDefine.Draw11CardCost;
                    break;
                case DrawCardCostType.Draw35CardCost:
                    cost = GameDefine.Draw35CardCost;
                    break;
            }

            return GameDataManager.Ins.Diamond >= cost;
        }

        private void OnDiamondChanged()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (IsCanDrawCard(DrawCardCostType.Draw11CardCost))
            {
                m_BtnLeftDrawCard.interactable = true;
                m_BtnLeftDrawCard.image.sprite = m_LeftNormalImage;
                m_LeftDrawCardText.color = Color.white;
                m_LeftDrawCardCost.color = Color.white;
            }
            else
            {
                m_BtnLeftDrawCard.interactable = true;
                m_BtnLeftDrawCard.image.sprite = m_CantImage;
                m_LeftDrawCardText.color = Color.red;
                m_LeftDrawCardCost.color = Color.red;
            }

            if (IsCanDrawCard(DrawCardCostType.Draw35CardCost))
            {
                m_BtnRightDrawCard.interactable = true;
                m_BtnRightDrawCard.image.sprite = m_RightNormalImage;
                m_RightDrawCardText.color = Color.white;
                m_RightDrawCardCost.color = Color.white;
            }
            else
            {
                m_BtnRightDrawCard.interactable = true;
                m_BtnRightDrawCard.image.sprite = m_CantImage;
                m_RightDrawCardText.color = Color.red;
                m_RightDrawCardCost.color = Color.red;
            }
        }

        private void OnBtnDrawCardClick(DrawCardCostType type)
        {
            if (IsCanDrawCard(type))
            {
                ShopManager.Ins.SendMsgC2SDrawCard((int)m_DrawCardType, type);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "钻石不足");
            }
        }

        private void OnBtnProbabilityClick()
        {
            var probability = ShopManager.Ins.GetSummonWeightList(m_ID);
            (int, int, List<int>) data = (m_Level, (int)m_DrawCardType, probability);
            EventManager.Call(LogicEvent.ShowDrawProbability, data);
        }

        private void InitDrawCardData()
        {
            switch (m_DrawCardType)
            {
                case DrawCardType.Skill:
                    m_ID = ShopManager.Ins.m_ShopSkillData.ID;
                    m_Level = ShopManager.Ins.m_ShopSkillData.Level;
                    m_Exp = ShopManager.Ins.m_ShopSkillData.Exp;
                    m_TotalExp = ShopManager.Ins.m_ShopSkillData.TotalExp;
                    break;
                case DrawCardType.Partner:
                    m_ID = ShopManager.Ins.m_ShopPartnerData.ID;
                    m_Level = ShopManager.Ins.m_ShopPartnerData.Level;
                    m_Exp = ShopManager.Ins.m_ShopPartnerData.Exp;
                    m_TotalExp = ShopManager.Ins.m_ShopPartnerData.TotalExp;
                    break;
                case DrawCardType.Equip:
                    m_ID = ShopManager.Ins.m_ShopEquipData.ID;
                    m_Level = ShopManager.Ins.m_ShopEquipData.Level;
                    m_Exp = ShopManager.Ins.m_ShopEquipData.Exp;
                    m_TotalExp = ShopManager.Ins.m_ShopEquipData.TotalExp;
                    break;
            }

            m_LevelText.text = $"LV {m_Level}";
            m_TextProcess.text = $"{m_Exp}/{SummonCfg.GetData(m_ID).LevelExp}";
            if (m_Exp != 0)
            {
                m_CanProcess.Show();
                m_CanProcess.fillAmount = (float)m_Exp / SummonCfg.GetData(m_ID).LevelExp;
            }
            else
            {
                m_CanProcess.Hide();
            }
        }

        private void OnUpdateDrawCardData(int eventId, object shopData)
        {
            var (type, id, level, exp, totalExp) = (ValueTuple<int, int, int, int, int>)shopData;
            if (type != (int)m_DrawCardType) return;

            //抽卡后变化逻辑
            if (id > m_ID)
            {
                /* TODO:
                 * 升级
                 * 升多级
                 */
                var lastExp = SummonCfg.GetData(m_ID).LevelExp;
                DOTween.Sequence().Append(UpdateExpText(lastExp, lastExp))
                    .Append(UpdateLevelText(level, this.transform.position))
                    .AppendInterval(1.5f)
                    .AppendCallback(() =>
                    {
                        (bool, int, Vector3) data = (false, level, this.transform.position);
                        EventManager.Call(LogicEvent.ShowOrHideLevelUp, data);
                        m_Exp = 0;
                        m_ID = id;
                        var levelExp = SummonCfg.GetData(m_ID).LevelExp;
                        UpdateExpText(exp, levelExp);
                        m_Level = level;
                        m_LevelText.text = $"LV {m_Level}";
                    }).Play();
            }
            else
            {
                // 未升级
                m_ID = id;
                var levelExp = SummonCfg.GetData(m_ID).LevelExp;
                UpdateExpText(exp, levelExp);
                m_Level = level;
                m_LevelText.text = $"LV {m_Level}";
            }
        }

        private Tweener UpdateExpText(int exp, int levelExp)
        {
            return DOTween.To(() => m_Exp, x => m_Exp = x, exp, 0.5f)
                .OnUpdate(() =>
                {
                    m_TextProcess.text = $"{m_Exp}/{levelExp}";
                    if (m_Exp != 0)
                    {
                        m_CanProcess.Show();
                        m_CanProcess.fillAmount = (float)m_Exp / levelExp;
                    }
                    else
                    {
                        m_CanProcess.Hide();
                    }
                });
        }

        private Sequence UpdateLevelText(int level, Vector3 position)
        {
            return DOTween.Sequence()
                .AppendCallback(() =>
                {
                    (bool, int, Vector3) data = (true, level, position);
                    EventManager.Call(LogicEvent.ShowOrHideLevelUp, data);
                });
        }
    }
}