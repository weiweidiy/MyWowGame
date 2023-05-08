using System;
using System.Collections.Generic;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonResearchItem : MonoBehaviour
    {
        [Header("研究类型")] public ResearchType m_ResearchType;
        [Header("研究父节点")] public List<CommonResearchItem> m_ParentNode;
        [Header("研究子节点")] public List<CommonResearchItem> m_ChildNode;
        [Header("研究状态")] public GameObject m_MinState;
        public GameObject m_MiddleState;
        public GameObject m_MaxState;
        public GameObject m_Selected;
        public GameObject m_IsResearching;
        public Image m_CanProcess;
        public TextMeshProUGUI m_ProcessText;
        public Button m_BtnItemClick;

        // 逻辑属性
        public Action<CommonResearchItem> m_ClickItem;
        private bool m_IsSelected;

        private EventGroup m_EventGroup = new();


        private void Awake()
        {
        }

        private void Init()
        {
        }

        private void OnDestroy()
        {
            m_ClickItem = null;
        }

        public void OnBtnItemClick()
        {
            m_EventGroup.Release();
            m_ClickItem?.Invoke(this);
        }

        public void ShowSelected()
        {
            m_Selected.Show();
        }

        public void HideSelected()
        {
            m_Selected.Hide();
        }
    }
}