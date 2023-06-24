using System;
using Configs;
using Framework.Extension;
using Logic.Common.RedDot;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonBreakTreeItem : MonoBehaviour
    {
        [Header("突破天赋树Id")] public int Id;
        [Header("突破天赋树状态线")] public GameObject m_CompleteLine;
        [Header("突破天赋树Icon")] public Image m_Icon;
        public GameObject m_UnLockState, m_MinState, m_MiddleState, m_MaxState;
        public GameObject m_Selected;
        public Image m_CanProcess;
        public TextMeshProUGUI m_ProcessText;

        // 逻辑属性
        public Action<CommonBreakTreeItem> m_ClickItem;
        private bool m_IsSelected;

        // 突破项状态
        public enum BreakState
        {
            UnLock,
            Min,
            Middle,
            Max
        }

        #region 数据

        public HerosBreakData m_HerosBreakData { get; private set; }
        public GameBreakTreeData m_GameBreakTreeData { get; private set; }
        public ResData m_ResData { get; private set; }
        public AttributeData m_AttributeData { get; private set; }

        public bool m_IsHave { get; private set; }
        public int m_Level { get; private set; }
        public int m_LvlMax { get; private set; }
        public BreakState m_BreakState { get; private set; }

        #endregion

        // 红点
        public BigBoomCellRedDotMono m_BigBoomCellRedDotMono;

        private void Awake()
        {
            m_BigBoomCellRedDotMono.Uid = Id.ToString();
        }

        private void OnDestroy()
        {
            m_ClickItem = null;
        }

        //初始化
        public void Init()
        {
            m_HerosBreakData = RoleBreakTreeManager.Ins.GetHerosBreakData(Id);

            if (RoleBreakTreeManager.Ins.GetGameBreakTreeData(Id) == null)
            {
                m_GameBreakTreeData = new GameBreakTreeData() { Id = Id };
                m_IsHave = false;
            }
            else
            {
                m_GameBreakTreeData = RoleBreakTreeManager.Ins.GetGameBreakTreeData(Id);
                m_IsHave = true;
            }

            m_ResData = RoleBreakTreeManager.Ins.GetResData(m_HerosBreakData.ResID);
            m_AttributeData = RoleBreakTreeManager.Ins.GetAttributeData(m_HerosBreakData.BreakAttrGroup);

            UpdateDetails();
            RefreshCompleteLine();
            RefreshState();
        }

        //更新突破项详情
        private void UpdateDetails()
        {
            m_Level = m_GameBreakTreeData.Level;
            m_LvlMax = m_HerosBreakData.LvlMax;
            //突破天赋项最大等级99999
            // m_ProcessText.text = m_LvlMax >= 99999 ? $"{m_Level}/∞" : $"{m_Level}/{m_LvlMax}";
            m_ProcessText.text = $"{m_Level}/{m_LvlMax}";
            m_CanProcess.fillAmount = (float)m_Level / m_LvlMax;
            UICommonHelper.LoadIcon(m_Icon, m_ResData.Res);
        }

        //刷新天赋项面板
        private void RefreshState()
        {
            if (!m_IsHave)
            {
                m_UnLockState.Show();
                m_MinState.Hide();
                m_MiddleState.Hide();
                m_MaxState.Hide();
                m_BreakState = BreakState.UnLock;
                return;
            }

            m_UnLockState.Hide();
            m_MinState.Hide();
            m_MiddleState.Hide();
            m_MaxState.Hide();
            if (m_Level == 0)
            {
                m_MinState.Show();
                m_BreakState = BreakState.Min;
            }
            else if (m_Level < m_LvlMax)
            {
                m_MiddleState.Show();
                m_BreakState = BreakState.Middle;
            }
            else
            {
                m_MaxState.Show();
                m_BreakState = BreakState.Max;
            }
        }

        //刷新完成线面板
        private void RefreshCompleteLine()
        {
            if (m_CompleteLine == null)
            {
                return;
            }

            if (m_Level >= m_LvlMax)
            {
                m_CompleteLine.Show();
            }
            else
            {
                m_CompleteLine.Hide();
            }
        }

        //注册当前突破天赋树节点点击事件
        public void OnBtnItemClick()
        {
            m_ClickItem?.Invoke(this);
        }

        //选中显示
        public void ShowSelected()
        {
            m_Selected.Show();
        }

        //选中隐藏
        public void HideSelected()
        {
            m_Selected.Hide();
        }
    }
}