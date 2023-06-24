using System;
using System.Collections.Generic;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTimer;

namespace Logic.UI.UIResearch
{
    public class UIResearch : UIPage
    {
        public TextMeshProUGUI m_MineCount;
        [Header("研究类型组")] public Transform m_ResearchRoot;
        private List<CommonResearchItem> m_ResearchList = new List<CommonResearchItem>();
        [Header("研究选择")] private CommonResearchItem m_CurSelectResearch;
        private CommonResearchItem m_OnResearchItem;
        [Header("研究详情")] public TextMeshProUGUI m_ResearchName;
        public TextMeshProUGUI m_CurrentAttrText, m_NextAttrText, m_MaxAttrText;
        public GameObject m_CurrentAttr, m_NextAttr, m_MaxAttr;
        public GameObject m_UnLockState, m_MinState, m_MiddleState, m_MaxState;
        public Image m_Icon;
        public Image m_CanProcess;
        public TextMeshProUGUI m_ProcessText;
        public TextMeshProUGUI m_ResearchCost;
        public GameObject m_ResearchTime;
        public TextMeshProUGUI m_TimerText;
        [Header("研究按钮")] public GameObject m_BtnCantResearch;
        public GameObject m_BtnResearch, m_BtnResearchCan, m_BtnResearchCant;
        public GameObject m_BtnMaxResearch;
        [Header("研究遮罩")] public GameObject m_ResearchMask;
        public TextMeshProUGUI m_ResearchTimerText;
        public TextMeshProUGUI m_ResearchText;
        public GameObject m_BtnCanAccelerate, m_BtnCantAccelerate;
        public TextMeshProUGUI m_AccelerateCost;
        private Timer m_ResearchTimer;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.OnUpdateResearchTime, OnUpdateResearchTime);
            m_EventGroup.Register(LogicEvent.OnResearching, OnResearching);
            m_EventGroup.Register(LogicEvent.MineChanged, (i, o) => OnMineChanged());
            m_EventGroup.Register(LogicEvent.ResearchLevelChanged, OnResearchLevelChanged);
            m_EventGroup.Register(LogicEvent.DiamondChanged, (i, o) => { OnDiamondChanged(); });
        }

        private void Start()
        {
            InitResearch();
            UpdateMineCount();
        }

        #region 通用

        private void UpdateMineCount()
        {
            m_MineCount.text = MiningManager.Ins.m_MiningData.MineCount.ToString();
            RefreshBtnResearch();
        }

        private void InitResearch()
        {
            var researchItems = m_ResearchRoot.GetComponentsInChildren<CommonResearchItem>();
            foreach (var commonResearchItem in researchItems)
            {
                commonResearchItem.m_ClickItem += OnClickResearchItem;
                m_ResearchList.Add(commonResearchItem);

                if (commonResearchItem.m_IsResearching == 1)
                {
                    m_OnResearchItem = commonResearchItem;
                    UpdateResearchTime(commonResearchItem.m_ResearchId, commonResearchItem.m_researchTimeStamp);
                }
            }

            OnClickResearchItem(m_OnResearchItem != null ? m_OnResearchItem : m_ResearchList[0]);
        }

        /// <summary>
        /// 更新研究详情
        /// </summary>
        /// <param name="sItem"></param>
        private void UpdateResearch(CommonResearchItem cItem)
        {
            // 获取研究相关数据
            var researchData = cItem.m_ResearchData;
            var attributeData = cItem.m_AttributeData;

            // 获取研究等级
            var curLevel = cItem.m_ResearchLevel;
            var maxLevel = researchData.LvlMax;
            var nextLevel = curLevel + 1 >= maxLevel ? maxLevel : curLevel + 1;

            // 显示相关数据
            m_ResearchName.text = researchData.ResearchProject;

            if ((AttributeType)attributeData.Type == AttributeType.HammerLimit)
            {
                m_CurrentAttrText.text = $"{attributeData.Name}增加{curLevel * researchData.ResearchGrow}";
                m_NextAttrText.text = $"{attributeData.Name}增加{nextLevel * researchData.ResearchGrow}";
                m_MaxAttrText.text = $"{attributeData.Name}增加{maxLevel * researchData.ResearchGrow}";
            }
            else
            {
                m_CurrentAttrText.text = $"{attributeData.Name}增加{curLevel * researchData.ResearchGrow}%";
                m_NextAttrText.text = $"{attributeData.Name}增加{nextLevel * researchData.ResearchGrow}%";
                m_MaxAttrText.text = $"{attributeData.Name}增加{maxLevel * researchData.ResearchGrow}%";
            }

            m_ResearchCost.text = Formula.GetResearchMineCost(researchData.ID, curLevel).ToString();
            m_TimerText.text = TimeHelper.FormatSecond((int)Formula.GetResearchTimeCost(researchData.ID, curLevel));

            RefreshDetails(cItem);
            RefreshAttrs(cItem.m_ResearchState);
        }

        private void RefreshDetails(CommonResearchItem cItem)
        {
            var level = cItem.m_ResearchLevel;
            var maxLevel = cItem.m_ResearchMaxLevel;
            var state = cItem.m_ResearchState;

            UICommonHelper.LoadIcon(m_Icon, cItem.m_ResPath);
            m_ProcessText.text = $"{level}/{maxLevel}";
            m_CanProcess.fillAmount = (float)level / maxLevel;

            switch (state)
            {
                case ResearchState.UnLock:
                    m_UnLockState.Show();
                    m_MinState.Hide();
                    m_MiddleState.Hide();
                    m_MaxState.Hide();
                    m_ResearchTime.Hide();
                    break;
                case ResearchState.Min:
                    m_UnLockState.Hide();
                    m_MinState.Show();
                    m_MiddleState.Hide();
                    m_MaxState.Hide();
                    m_ResearchTime.Show();
                    break;
                case ResearchState.Middle:
                    m_UnLockState.Hide();
                    m_MinState.Hide();
                    m_MiddleState.Show();
                    m_MaxState.Hide();
                    m_ResearchTime.Show();
                    break;
                case ResearchState.Max:
                    m_UnLockState.Hide();
                    m_MinState.Hide();
                    m_MiddleState.Hide();
                    m_MaxState.Show();
                    m_ResearchTime.Hide();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RefreshAttrs(ResearchState researchState)
        {
            switch (researchState)
            {
                case ResearchState.UnLock:
                    m_CurrentAttr.Hide();
                    m_NextAttr.Hide();
                    m_MaxAttr.Show();
                    m_BtnCantResearch.Show();
                    m_BtnResearch.Hide();
                    m_BtnMaxResearch.Hide();
                    break;
                case ResearchState.Min:
                case ResearchState.Middle:
                    m_CurrentAttr.Show();
                    m_NextAttr.Show();
                    m_MaxAttr.Hide();
                    m_BtnCantResearch.Hide();
                    m_BtnResearch.Show();
                    RefreshBtnResearch();
                    m_BtnMaxResearch.Hide();
                    break;
                case ResearchState.Max:
                    m_CurrentAttr.Hide();
                    m_NextAttr.Hide();
                    m_MaxAttr.Show();
                    m_BtnCantResearch.Hide();
                    m_BtnResearch.Hide();
                    m_BtnMaxResearch.Show();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(researchState), researchState, null);
            }
        }

        private void RefreshBtnResearch()
        {
            if (m_CurSelectResearch == null) return;

            // var cost = Formula.GetResearchMineCost(m_CurSelectResearch.m_ResearchId,
            //     m_CurSelectResearch.m_ResearchLevel);
            if (ResearchManager.Ins.IsMineCanResearch(m_CurSelectResearch.m_ResearchId))
            {
                m_BtnResearchCan.Show();
                m_BtnResearchCant.Hide();
                m_ResearchCost.color = Color.white;
            }
            else
            {
                m_BtnResearchCan.Hide();
                m_BtnResearchCant.Show();
                m_ResearchCost.color = Color.red;
            }

            RefreshBtnAccelerate();
        }

        private void RefreshBtnAccelerate()
        {
            if (m_CurSelectResearch == null) return;
            var researchId = m_CurSelectResearch.m_ResearchId;
            var researchLevel = m_CurSelectResearch.m_ResearchLevel;
            var cost = Formula.GetResearchDiamondCost(researchId, researchLevel);
            if (ResearchManager.Ins.IsCanAccelerate(cost))
            {
                m_BtnCanAccelerate.Show();
                m_BtnCantAccelerate.Hide();
                m_AccelerateCost.color = Color.white;
            }
            else
            {
                m_BtnCanAccelerate.Hide();
                m_BtnCantAccelerate.Show();
                m_AccelerateCost.color = Color.red;
            }
        }

        //初始更新研究状态
        private void UpdateResearchTime(int id, long time)
        {
            m_BtnResearch.Hide();
            m_ResearchMask.Show();
            var researchData = ResearchManager.Ins.GetResearchData(id);
            var researchItem = m_ResearchList.Find(r => r.m_ResearchId == id);
            var researchProject = researchData.ResearchProject;
            m_ResearchText.text = $"正在研究{researchProject}";
            m_AccelerateCost.text =
                $"{Formula.GetResearchDiamondCost(researchItem.m_ResearchId, researchItem.m_ResearchLevel)}";
            var timeLeft = time - TimeHelper.GetUnixTimeStamp();
            var secondLeft = (int)timeLeft;
            if (secondLeft <= 0)
            {
                secondLeft = 0;
            }

            m_ResearchTimerText.text = TimeHelper.FormatSecond(secondLeft);
            m_ResearchTimer = Timer.Register(1f, () =>
            {
                secondLeft--;
                if (secondLeft <= 0 && this.gameObject.activeSelf)
                {
                    secondLeft = 0;
                    m_ResearchTimer.Cancel();
                    ResearchManager.Ins.DoResearching(id, (int)ResearchCompleteType.TimeComplete);
                }

                m_ResearchTimerText.text = TimeHelper.FormatSecond(secondLeft);
            }, isLooped: true, useRealTime: true);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 矿石数量变化
        /// </summary>
        private void OnMineChanged()
        {
            UpdateMineCount();
        }

        /// <summary>
        /// 选中研究类型
        /// </summary>
        /// <param name="pItem"></param>
        private void OnClickResearchItem(CommonResearchItem pItem)
        {
            if (m_CurSelectResearch == pItem)
            {
                return;
            }

            if (m_CurSelectResearch != null)
            {
                m_CurSelectResearch.HideSelected();
            }

            m_CurSelectResearch = pItem;
            m_CurSelectResearch.ShowSelected();

            UpdateResearch(m_CurSelectResearch);
        }

        private void OnUpdateResearchTime(int eventId, object data)
        {
            var (id, time) = (ValueTuple<int, long>)data;
            m_OnResearchItem = m_ResearchList.Find(r => r.m_ResearchId == id);
            UpdateResearchTime(id, time);
        }

        private void OnResearching(int eventId, object data)
        {
            var (id, level) = (ValueTuple<int, int>)data;
            m_ResearchMask.Hide();
            m_BtnResearch.Show();

            //研究成功弹出面板
            var researchData = ResearchManager.Ins.GetResearchData(id);
            var resId = researchData.ResID;
            var resPath = ResearchManager.Ins.GetResData(resId).Res;
            var researchProject = researchData.ResearchProject;
            var arg = new UICommonObtain.Args
            {
                title = "已经完成研究",
                resPath = resPath,
                name = researchProject,
            };
            EventManager.Call(LogicEvent.ShowObtain, arg);
            // EventManager.Call(LogicEvent.ShowTips, $"已经完成{researchProject}研究");
        }

        private void OnResearchLevelChanged(int eventId, object data)
        {
            var cItem = (CommonResearchItem)data;
            if (cItem == m_CurSelectResearch)
            {
                UpdateResearch(cItem);
            }
        }

        private void OnDiamondChanged()
        {
            RefreshBtnAccelerate();
        }

        #endregion

        #region 操作

        public void OnBtnCloseClick()
        {
            this.Hide();
        }

        public void OnBtnResearchClick()
        {
            if (m_CurSelectResearch == null) return;
            if (m_CurSelectResearch.m_ResearchLevel >= m_CurSelectResearch.m_ResearchMaxLevel)
            {
                EventManager.Call(LogicEvent.ShowTips, "当前已经达到最大研究");
            }
            else
            {
                // var cost = Formula.GetResearchMineCost(m_CurSelectResearch.m_ResearchId,
                //     m_CurSelectResearch.m_ResearchLevel);
                if (ResearchManager.Ins.IsMineCanResearch(m_CurSelectResearch.m_ResearchId))
                {
                    ResearchManager.Ins.DoUpdateResearchTime(m_CurSelectResearch.m_ResearchId);
                }
                else
                {
                    EventManager.Call(LogicEvent.ShowTips, "所需数量未满足需求");
                }
            }
        }

        public void OnBtnCantResearchClick()
        {
            EventManager.Call(LogicEvent.ShowTips, "需要提前研究");
        }

        public void OnBtnMaxResearchClick()
        {
            EventManager.Call(LogicEvent.ShowTips, "当前已经达到最大研究");
        }

        public void OnBtnAccelerateClick()
        {
            if (m_OnResearchItem == null) return;
            var researchId = m_OnResearchItem.m_ResearchId;
            var researchLevel = m_OnResearchItem.m_ResearchLevel;
            var cost = Formula.GetResearchDiamondCost(researchId, researchLevel);
            if (ResearchManager.Ins.IsCanAccelerate(cost))
            {
                m_ResearchTimer?.Cancel();
                m_ResearchTimer = null;
                ResearchManager.Ins.DoResearching(researchId, (int)ResearchCompleteType.DiamondComplete);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "所需数量未满足需求");
            }
        }

        #endregion
    }
}