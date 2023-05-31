using System;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
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

namespace Logic.UI.UIUser
{
    public class PartQuenching : MonoBehaviour
    {
        public List<CommonQuenchingAttr> m_QuenchingAttrs = new List<CommonQuenchingAttr>();
        public TextMeshProUGUI m_DoLevel, m_ReLevel, m_MiLevel, m_FaLevel, m_SolLevel;
        public TextMeshProUGUI m_DjCount;
        public GameObject m_BtnQuenchingCan, m_BtnQuenchingCant;
        public TextMeshProUGUI m_DjCost;

        private CommonQuenchingAttr m_CurrentClickItem;

        private int m_LockCount;
        private int m_QuenchingCost;
        private bool m_IsFirst;
        private EventGroup m_EventGroup = new EventGroup();


        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.OnQuenching, OnQuenching);
            m_EventGroup.Register(LogicEvent.OilChanged, (i, o) => OnOilChanged());
        }

        private void Start()
        {
            InitQuenching();
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        // 初始化
        private void InitQuenching()
        {
            var quenchingMap = QuenchingManager.Ins.QuenchingMap;
            foreach (var (qId, quenchingData) in quenchingMap)
            {
                m_QuenchingAttrs[qId].Init(qId, quenchingData.UnlockType);
                m_QuenchingAttrs[qId].UpdateQuenchingAttr(quenchingData.AttributeId, quenchingData.MelodyId);
                m_QuenchingAttrs[qId].m_ClickItem += OnQuenchingItemClick;
            }

            // 初始没有淬炼属性时特殊处理初始状态
            if (quenchingMap.Count == 0)
            {
                var count = m_QuenchingAttrs.Count;
                for (var i = 0; i < count; i++)
                {
                    m_QuenchingAttrs[i].Init(i);
                    m_QuenchingAttrs[i].m_ClickItem += OnQuenchingItemClick;
                    m_QuenchingAttrs[i].m_ImageClick.raycastTarget = false;
                    m_QuenchingAttrs[i].m_AttrQuality.Hide();
                    m_QuenchingAttrs[i].m_AttrIcon.Hide();
                }
            }

            UpdateQuenchingGroup();
            OnOilChanged();
            UpdateDjCost();
        }

        /// <summary>
        /// 更新淬炼属性组合效果
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void UpdateQuenchingGroup()
        {
            var quenchingMap = QuenchingManager.Ins.QuenchingMap;
            var quenchingLevels = new Dictionary<QuenchingType, int>
            {
                { QuenchingType.Do, 0 },
                { QuenchingType.Re, 0 },
                { QuenchingType.Mi, 0 },
                { QuenchingType.Fa, 0 },
                { QuenchingType.Sol, 0 },
            };

            foreach (var quenchingType in quenchingMap.Values.Select(quenchingData =>
                         (QuenchingType)quenchingData.MelodyId))
            {
                quenchingLevels[quenchingType]++;
            }

            foreach (var quenchingType in quenchingLevels.Keys)
            {
                var level = quenchingLevels[quenchingType];
                var text = level switch
                {
                    5 => "3级",
                    >= 3 and < 5 => "2级",
                    >= 2 and < 3 => "1级",
                    _ => "0级"
                };
                switch (quenchingType)
                {
                    case QuenchingType.Do:
                        m_DoLevel.text = text;
                        break;
                    case QuenchingType.Re:
                        m_ReLevel.text = text;
                        break;
                    case QuenchingType.Mi:
                        m_MiLevel.text = text;
                        break;
                    case QuenchingType.Fa:
                        m_FaLevel.text = text;
                        break;
                    case QuenchingType.Sol:
                        m_SolLevel.text = text;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        //淬炼属性消耗货币变化
        private void OnOilChanged()
        {
            var djCount = (BigDouble)GameDataManager.Ins.Oil;
            m_DjCount.text = djCount.ToUIString();
            UpdateDjCost();
        }

        //更新淬炼属性消耗货币
        private void UpdateDjCost()
        {
            m_LockCount = m_QuenchingAttrs.Count(commonQuenchingAttr => !commonQuenchingAttr.m_IsUnLock);
            m_QuenchingCost = QuenchingManager.Ins.GetDJData(m_LockCount).Cost;
            if (m_LockCount == m_QuenchingAttrs.Count)
            {
                m_BtnQuenchingCan.Hide();
                m_BtnQuenchingCant.Show();
                m_DjCost.color = Color.gray;
            }
            else if (QuenchingManager.Ins.IsCanQuenching(m_QuenchingCost))
            {
                m_BtnQuenchingCan.Show();
                m_BtnQuenchingCant.Hide();
                m_DjCost.color = Color.white;
            }
            else
            {
                m_BtnQuenchingCan.Hide();
                m_BtnQuenchingCant.Show();
                m_DjCost.color = Color.red;
            }

            m_DjCost.text = m_QuenchingCost.ToString();
        }

        #region 按钮

        public async void OnBtnQuenchingGroupClick()
        {
            await UIManager.Ins.OpenUI<UIQuenchingGroupInfo>();
        }

        public async void OnBtnQuenchingInfoClick()
        {
            await UIManager.Ins.OpenUI<UIQuenchingInfo>();
        }

        public void OnBtnQuenchingClick()
        {
            if (m_LockCount == m_QuenchingAttrs.Count)
            {
                EventManager.Call(LogicEvent.ShowTips, "至少需要一个解锁的淬炼属性");
            }
            else if (QuenchingManager.Ins.IsCanQuenching(m_QuenchingCost))
            {
                // 可以被淬炼的属性Id
                var qIdList = m_QuenchingAttrs.Where(commonQuenchingAttr => commonQuenchingAttr.m_IsUnLock)
                    .Select(commonQuenchingAttr => commonQuenchingAttr.m_QuenchingId).ToList();

                // 锁定属性个数
                var qLockCount = m_QuenchingAttrs.Count - qIdList.Count;

                // 当前存在S级以上属性弹出确认面板
                if (m_QuenchingAttrs.Where(commonQuenchingAttr => commonQuenchingAttr.m_IsUnLock)
                    .Any(commonQuenchingAttr => commonQuenchingAttr.m_AttributeData?.Quality >= 6))
                {
                    UIMsgBox.ShowMsgBox(2, "当前存在S级以上属性", "是否确认更改",
                        confirmAction: () => { QuenchingManager.Ins.DoQuenching(qIdList,qLockCount); });
                    return;
                }

                QuenchingManager.Ins.DoQuenching(qIdList,qLockCount);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "所需数量未满足需求");
            }
        }

        #endregion

        #region 事件

        private void OnQuenchingItemClick(CommonQuenchingAttr cItem)
        {
            m_CurrentClickItem = cItem;
            m_CurrentClickItem.DoQuenchingLock();
            UpdateDjCost();
        }

        private void OnQuenching(int eventId, object data)
        {
            // 初始第一次点击淬炼属性特殊状态处理
            if (!m_IsFirst)
            {
                m_IsFirst = true;
                var count = m_QuenchingAttrs.Count;
                for (var i = 0; i < count; i++)
                {
                    m_QuenchingAttrs[i].m_ImageClick.raycastTarget = true;
                    m_QuenchingAttrs[i].m_AttrQuality.Show();
                    m_QuenchingAttrs[i].m_AttrIcon.Show();
                }
            }

            var qIdList = (List<int>)data;
            var quenchingMap = QuenchingManager.Ins.QuenchingMap;

            foreach (var qId in qIdList)
            {
                var quenchingData = quenchingMap[qId];
                m_QuenchingAttrs[qId].UpdateQuenchingAttr(quenchingData.AttributeId, quenchingData.MelodyId);
            }

            UpdateQuenchingGroup();
        }

        #endregion
    }
}