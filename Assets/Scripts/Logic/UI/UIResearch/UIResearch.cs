using System.Collections.Generic;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Cells;
using TMPro;
using UnityEngine;

namespace Logic.UI.UIResearch
{
    public class UIResearch : UIPage
    {
        public TextMeshProUGUI m_MineCount;
        [Header("研究类型组")] public Transform m_ResearchRoot;
        private List<CommonResearchItem> m_ResearchList = new List<CommonResearchItem>();
        [Header("研究选择")] private CommonResearchItem m_CurSelectResearch;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.MiningDataChanged, OnMineChanged);
        }

        private void Start()
        {
            UpdateMineCount();
            InitResearch();
        }

        #region 通用

        private void UpdateMineCount()
        {
            m_MineCount.text = MiningManager.Ins.m_MiningData.m_MineCount.ToString();
        }

        private void InitResearch()
        {
            var researchItems = m_ResearchRoot.GetComponentsInChildren<CommonResearchItem>();
            foreach (var commonResearchItem in researchItems)
            {
                commonResearchItem.m_ClickItem += OnClickResearchItem;
                m_ResearchList.Add(commonResearchItem);
            }
        }

        #endregion

        #region 事件

        private void OnMineChanged(int eventId, object data)
        {
            var miningType = (MiningType)data;
            if (miningType is MiningType.CopperMine or MiningType.SilverMine or MiningType.GoldMine)
            {
                UpdateMineCount();
            }
        }

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
        }

        #endregion

        #region 操作

        public void OnBtnCloseClick()
        {
            this.Hide();
        }

        #endregion
    }
}