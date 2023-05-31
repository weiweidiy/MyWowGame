using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIShop
{
    public class PartDrawCard : MonoBehaviour
    {
        // 抽卡
        public GameObject m_DrawResultNode;
        public GameObject m_DrawRoot;
        public GameObject m_CommonDrawItem;
        public Button m_BtnLeftDrawCardAgain;
        public TextMeshProUGUI m_LeftDrawCardText;
        public TextMeshProUGUI m_LeftDrawCardCost;
        public Button m_BtnRightDrawCardAgain;
        public TextMeshProUGUI m_RightDrawCardText;
        public TextMeshProUGUI m_RightDrawCardCost;
        public Sprite m_LeftNormalImage;
        public Sprite m_RightNormalImage;
        public Sprite m_CantImage;
        public Button m_BtnCloseResult;
        private List<GameObject> m_DrawItemList = new List<GameObject>(64);

        // 卡池概率
        public GameObject m_DrawProbability;
        public TextMeshProUGUI m_ProbabilityLevel;
        public Button m_BtnLeftProbability;
        public Button m_BtnRightProbability;
        public Button m_BtnProbabilityClose;
        public List<CommonItemProbability> m_ItemProbabilityList = new List<CommonItemProbability>();

        // 卡池升级
        public GameObject m_DrawLevelUp;
        public GameObject m_DrawLevelUpBg;
        public TextMeshProUGUI m_LastLevel;
        public TextMeshProUGUI m_NextLevel;
        public TextMeshProUGUI m_Quality;
        private CanvasGroup m_LevelUpCanvasGroup;

        // 记录该次抽卡的类型便于再次抽卡
        private int m_DrawCardType;
        private int m_DrawCardId;
        private int m_DrawCardLevel;
        private bool m_IsDrawCard;

        private EventGroup m_EventGroup = new();

        private void Awake()
        {
            // 抽卡
            m_EventGroup.Register(LogicEvent.ShowDrawCardResult, OnShowDrawCardResult);
            m_BtnCloseResult.onClick.AddListener(OnBtnCloseResultClick);
            m_BtnLeftDrawCardAgain.onClick.AddListener(() => OnBtnDrawCardClickAgain(DrawCardCostType.Draw11CardCost));
            m_BtnRightDrawCardAgain.onClick.AddListener(() => OnBtnDrawCardClickAgain(DrawCardCostType.Draw35CardCost));
            m_EventGroup.Register(LogicEvent.DiamondChanged, (i, o) => OnDiamondChanged());

            // 卡池概率
            m_EventGroup.Register(LogicEvent.ShowDrawProbability, OnShowDrawProbability);
            m_BtnProbabilityClose.onClick.AddListener(() => m_DrawProbability.Hide());
            m_BtnLeftProbability.onClick.AddListener(OnBtnLeftProbabilityClick);
            m_BtnRightProbability.onClick.AddListener(OnBtnRightProbabilityClick);

            // 卡池升级
            m_EventGroup.Register(LogicEvent.ShowOrHideLevelUp, OnShowOrHideLevelUp);
            m_LevelUpCanvasGroup = m_DrawLevelUp.GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// 抽卡动画播放中被剧情跳转异常关闭处理
        /// </summary>
        private void OnEnable()
        {
            m_IsDrawCard = false;
            if (m_DrawResultNode.activeSelf)
            {
                OnBtnCloseResultClick();
            }
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        #region 抽卡

        private IEnumerator ShowDrawCardResultCoroutine(object o)
        {
            m_IsDrawCard = true;
            var _Data = (ValueTuple<int, List<int>>)o;
            m_DrawCardType = _Data.Item1;
            m_DrawResultNode.SetActive(true);
            var _List = _Data.Item2;
            foreach (var id in _List)
            {
                var _Obj = Instantiate(m_CommonDrawItem, m_DrawRoot.transform);
                var item = _Obj.GetComponent<CommonDrawItem>();
                item.m_ClickCB += OnClickCB;
                switch (_Data.Item1)
                {
                    case (int)DrawCardType.Skill:
                        item.InitBySkill(id);
                        break;
                    case (int)DrawCardType.Partner:
                        item.InitByPartner(id);
                        break;
                    case (int)DrawCardType.Equip:
                        item.InitByEquip(id);
                        break;
                }

                _Obj.Show();
                m_DrawItemList.Add(_Obj);
                yield return new WaitForSeconds(0.02f);
            }

            m_IsDrawCard = false;
        }

        private async void OnClickCB(CommonDrawItem pItem, ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                    var equipData = (itemType, pItem.m_EquipData.ID);
                    await UIManager.Ins.OpenUI<UICommonCardInfo>(equipData);
                    break;
                case ItemType.Skill:
                    var skillData = (itemType, pItem.m_SkillData.ID);
                    await UIManager.Ins.OpenUI<UICommonCardInfo>(skillData);
                    break;
                case ItemType.Partner:
                    var partnerData = (itemType, pItem.m_PartnerData.ID);
                    await UIManager.Ins.OpenUI<UICommonCardInfo>(partnerData);
                    break;
            }
        }

        private void OnShowDrawCardResult(int eventId, object o)
        {
            StartCoroutine(ShowDrawCardResultCoroutine(o));
        }

        private void SendMsgC2SDrawCardAgain(DrawCardCostType costType)
        {
            // StopAllCoroutines();
            if (m_IsDrawCard) return;

            DestroyDrawItemList();
            ShopManager.Ins.SendMsgC2SDrawCard(m_DrawCardType, costType);
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

        private void OnBtnDrawCardClickAgain(DrawCardCostType type)
        {
            if (IsCanDrawCard(type))
            {
                SendMsgC2SDrawCardAgain(type);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "钻石不足");
            }
        }

        private void OnDiamondChanged()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (IsCanDrawCard(DrawCardCostType.Draw11CardCost))
            {
                m_BtnLeftDrawCardAgain.interactable = true;
                m_BtnLeftDrawCardAgain.image.sprite = m_LeftNormalImage;
                m_LeftDrawCardText.color = Color.white;
                m_LeftDrawCardCost.color = Color.white;
            }
            else
            {
                m_BtnLeftDrawCardAgain.interactable = true;
                m_BtnLeftDrawCardAgain.image.sprite = m_CantImage;
                m_LeftDrawCardText.color = Color.red;
                m_LeftDrawCardCost.color = Color.red;
            }

            if (IsCanDrawCard(DrawCardCostType.Draw35CardCost))
            {
                m_BtnRightDrawCardAgain.interactable = true;
                m_BtnRightDrawCardAgain.image.sprite = m_RightNormalImage;
                m_RightDrawCardText.color = Color.white;
                m_RightDrawCardCost.color = Color.white;
            }
            else
            {
                m_BtnRightDrawCardAgain.interactable = true;
                m_BtnRightDrawCardAgain.image.sprite = m_CantImage;
                m_RightDrawCardText.color = Color.red;
                m_RightDrawCardCost.color = Color.red;
            }
        }

        private void DestroyDrawItemList()
        {
            if (m_DrawItemList == null) return;
            // TODO: 用对象池优化
            foreach (var item in m_DrawItemList)
            {
                Destroy(item);
            }

            m_DrawItemList.Clear();
        }

        private void OnBtnCloseResultClick()
        {
            // StopAllCoroutines();
            if (m_IsDrawCard) return;

            m_DrawResultNode.Hide();
            DestroyDrawItemList();
            ShopManager.Ins.SendMsgC2SUpdateShopData(m_DrawCardType);
        }

        #endregion

        #region 卡池概率

        private void OnShowDrawProbability(int eventId, object o)
        {
            var (level, type, probability) = (ValueTuple<int, int, List<int>>)o;
            m_DrawCardLevel = level;
            m_DrawCardId = (DrawCardType)type switch
            {
                DrawCardType.Skill => level + 3000,
                DrawCardType.Partner => level + 4000,
                DrawCardType.Equip => level + 1000,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            UpdateProbabilityLevel(m_DrawCardLevel);
            UpdateCommonItemProbability(probability);
            m_DrawProbability.Show();
        }

        private void UpdateCommonItemProbability(IReadOnlyList<int> probability)
        {
            var j = probability.Count;
            for (var i = 0; i < j; i++)
            {
                m_ItemProbabilityList[i].UpdateProbabilityText(probability[i]);
            }
        }

        private void UpdateProbabilityLevel(int level)
        {
            if (level <= 1)
            {
                m_DrawCardLevel = 1;
                m_BtnLeftProbability.Hide();
            }
            else
            {
                m_BtnLeftProbability.Show();
            }

            if (level >= 10)
            {
                m_DrawCardLevel = 10;
                m_BtnRightProbability.Hide();
            }
            else
            {
                m_BtnRightProbability.Show();
            }

            m_ProbabilityLevel.text = $"LEVEL {m_DrawCardLevel}";
        }

        private void OnBtnLeftProbabilityClick()
        {
            m_DrawCardLevel--;
            m_DrawCardId--;
            UpdateProbabilityLevel(m_DrawCardLevel);
            UpdateCommonItemProbability(ShopManager.Ins.GetSummonWeightList(m_DrawCardId));
        }

        private void OnBtnRightProbabilityClick()
        {
            m_DrawCardLevel++;
            m_DrawCardId++;
            UpdateProbabilityLevel(m_DrawCardLevel);
            UpdateCommonItemProbability(ShopManager.Ins.GetSummonWeightList(m_DrawCardId));
        }

        #endregion

        #region 卡池升级

        private void OnShowOrHideLevelUp(int eventId, object o)
        {
            var (isShow, level, position) = (ValueTuple<bool, int, Vector3>)o;

            if (isShow)
            {
                m_DrawLevelUpBg.transform.position = position + new Vector3(0, -1, 0);
                m_DrawLevelUp.Show();
                m_LevelUpCanvasGroup.DOFade(1, 0.5f);
                m_LastLevel.text = (level - 1).ToString();
                m_NextLevel.text = level.ToString();
                m_Quality.text = UICommonHelper.GetQualityShowText(level - 1);
            }
            else
            {
                m_LevelUpCanvasGroup.DOFade(0, 0.5f);
                m_DrawLevelUp.Hide();
            }
        }

        #endregion
    }
}