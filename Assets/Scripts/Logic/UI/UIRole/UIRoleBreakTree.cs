using System;
using System.Collections.Generic;
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

namespace Logic.UI.UIRole
{
    public class UIRoleBreakTree : UIPage
    {
        //英雄突破天赋点相关
        public TextMeshProUGUI m_TPCount;

        //英雄突破天赋树列表相关
        public Transform m_BreakTreeRoot;
        private List<CommonBreakTreeItem> m_BreakTreeList = new List<CommonBreakTreeItem>();
        private CommonBreakTreeItem m_CurSelectItem;

        //英雄突破天赋树属性相关
        public TextMeshProUGUI m_CurrentAttrText, m_NextAttrText, m_MaxAttrText;
        public GameObject m_CurrentAttr, m_NextAttr, m_MaxAttr;
        public GameObject m_UnLockState, m_MinState, m_MiddleState, m_MaxState;
        public Image m_Icon;
        public Image m_CanProcess;
        public TextMeshProUGUI m_ProcessText;

        //英雄突破重置相关
        public GameObject m_BtnResetCan, m_BtnResetCant;
        public TextMeshProUGUI m_ResetCost;

        //英雄突破强化相关
        public GameObject m_BtnIntensifyCan, m_BtnIntensifyCant;
        public TextMeshProUGUI m_IntensifyCost;
        public GameObject m_BtnIntensifyMax;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.DiamondChanged, (i, o) => OnDiamondChanged());
            m_EventGroup.Register(LogicEvent.RoleBreakTPChanged, (i, o) => OnBreakTPChanged());
            m_EventGroup.Register(LogicEvent.RoleBreakTreeReset, (i, o) => OnRoleBreakTreeReset());
            m_EventGroup.Register(LogicEvent.RoleBreakTreeIntensify, (i, o) => OnRoleBreakTreeIntensify());
        }

        private void Start()
        {
            Init();
            OnBreakTPChanged();
        }

        #region 通用

        //初始化
        private void Init()
        {
            var breakTreeItems = m_BreakTreeRoot.GetComponentsInChildren<CommonBreakTreeItem>();
            foreach (var commonBreakTreeItem in breakTreeItems)
            {
                commonBreakTreeItem.m_ClickItem += OnClickBreakTreeItem;
                commonBreakTreeItem.Init();
                m_BreakTreeList.Add(commonBreakTreeItem);

                //选中默认天赋项
                if (commonBreakTreeItem.Id == GameDefine.RoleBreakTreeDefaultId)
                {
                    OnClickBreakTreeItem(commonBreakTreeItem);
                }
            }
        }

        //更新当前突破天赋树节点相关信息
        private void UpdateCurSelectBreakTreeItem()
        {
            //更新突破天赋项强化进度
            var level = m_CurSelectItem.m_Level;
            var lvlMax = m_CurSelectItem.m_LvlMax;
            //突破天赋项最大等级99999
            // m_ProcessText.text = lvlMax >= 99999 ? $"{level}/∞" : $"{level}/{lvlMax}";
            m_ProcessText.text = $"{level}/{lvlMax}";
            m_CanProcess.fillAmount = (float)level / lvlMax;
            //更新重置突破天赋树钻石消耗
            m_ResetCost.text = GameDefine.RoleBreakTreeDiamondCost.ToString();
            //更新强化突破天赋项突破点消耗
            m_IntensifyCost.text = m_CurSelectItem.m_HerosBreakData.BaseCost.ToString();
            //更新突破天赋项Icon
            UICommonHelper.LoadIcon(m_Icon, m_CurSelectItem.m_ResData.Res);
            //更新突破天赋项属性
            var des = m_CurSelectItem.m_AttributeData.Des;
            var value = m_CurSelectItem.m_AttributeData.Value;
            var grow = m_CurSelectItem.m_HerosBreakData.ResearchGrow;
            var curLevel = m_CurSelectItem.m_Level;
            var maxLevel = m_CurSelectItem.m_LvlMax;
            var nextLevel = curLevel + 1 >= maxLevel ? maxLevel : curLevel + 1;
            m_CurrentAttrText.text = string.Format(des, value + curLevel * grow);
            m_MaxAttrText.text = string.Format(des, value + maxLevel * grow);
            m_NextAttrText.text = string.Format(des, value + nextLevel * grow);
            //刷新相关面板
            RefreshAttr();
            RefreshState();
            RefreshBtnReset();
            RefreshBtnIntensify();
        }

        //刷新所有天赋树项数据
        private void RefreshBreakTreeList()
        {
            foreach (var combCommonBreakTreeItem in m_BreakTreeList)
            {
                combCommonBreakTreeItem.Init();
            }
        }

        //刷新突破项所有属性面板
        private void RefreshAttr()
        {
            m_CurrentAttr.Show();
            m_NextAttr.Show();
            m_MaxAttr.SetActive(m_CurSelectItem.m_Level >= m_CurSelectItem.m_LvlMax);
        }

        //刷新突破项状态面板
        private void RefreshState()
        {
            m_UnLockState.Hide();
            m_MinState.Hide();
            m_MiddleState.Hide();
            m_MaxState.Hide();

            switch (m_CurSelectItem.m_BreakState)
            {
                case CommonBreakTreeItem.BreakState.UnLock:
                    m_UnLockState.Show();
                    break;
                case CommonBreakTreeItem.BreakState.Min:
                    m_MinState.Show();
                    break;
                case CommonBreakTreeItem.BreakState.Middle:
                    m_MiddleState.Show();
                    break;
                case CommonBreakTreeItem.BreakState.Max:
                    m_MaxState.Show();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //刷新重置突破天赋树按钮面板
        private void RefreshBtnReset()
        {
            if (RoleBreakTreeManager.Ins.IsCanResetBreakTree())
            {
                m_BtnResetCan.Show();
                m_BtnResetCant.Hide();
            }
            else
            {
                m_BtnResetCan.Hide();
                m_BtnResetCant.Show();
            }
        }

        //刷新强化突破天赋树按钮面板
        private void RefreshBtnIntensify()
        {
            if (RoleBreakTreeManager.Ins.IsCanUpgradeLevel(m_CurSelectItem.Id))
            {
                m_BtnIntensifyCan.Show();
                m_BtnIntensifyCant.Hide();
            }
            else
            {
                m_BtnIntensifyCan.Hide();
                m_BtnIntensifyCant.Show();
            }

            m_BtnIntensifyMax.SetActive(m_CurSelectItem.m_Level >= m_CurSelectItem.m_LvlMax);
        }

        #endregion

        #region 事件处理

        //钻石变化
        private void OnDiamondChanged()
        {
            RefreshBtnReset();
        }

        //更新英雄突破天赋点
        private void OnBreakTPChanged()
        {
            m_TPCount.text = GameDataManager.Ins.BreakTP.ToString();
            RefreshBtnIntensify();
        }

        //重置英雄突破天赋树
        private void OnRoleBreakTreeReset()
        {
            RefreshBreakTreeList();
            UpdateCurSelectBreakTreeItem();
        }

        //强化英雄突破天赋项
        private void OnRoleBreakTreeIntensify()
        {
            RefreshBreakTreeList();
            UpdateCurSelectBreakTreeItem();
        }

        //点击当前突破天赋树节点
        private void OnClickBreakTreeItem(CommonBreakTreeItem cItem)
        {
            if (m_CurSelectItem == cItem) return;

            if (m_CurSelectItem != null)
            {
                m_CurSelectItem.HideSelected();
            }

            m_CurSelectItem = cItem;
            m_CurSelectItem.ShowSelected();
            UpdateCurSelectBreakTreeItem();
        }

        #endregion


        #region 按钮

        // 点击关闭按钮
        public void OnBtnCloseClick()
        {
            this.Hide();
        }

        // 点击重置按钮
        public void OnBtnResetClick()
        {
            if (m_CurSelectItem == null) return;

            if (RoleBreakTreeManager.Ins.IsCanResetBreakTree())
            {
                RoleBreakTreeManager.Ins.DoReset();
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "未满足重置条件");
            }
        }

        // 点击强化按钮
        public void OnBtnIntensifyClick()
        {
            if (m_CurSelectItem == null) return;

            if (RoleBreakTreeManager.Ins.IsCanUpgradeLevel(m_CurSelectItem.Id))
            {
                RoleBreakTreeManager.Ins.DoIntensify(m_CurSelectItem.Id);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "未满足强化条件");
            }
        }

        // 点击强化已达最大按钮
        public void OnBtnMaxClick()
        {
            EventManager.Call(LogicEvent.ShowTips, "已达到最大等级");
        }

        #endregion
    }
}