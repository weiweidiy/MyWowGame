using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonResearchItem : MonoBehaviour
    {
        [Header("研究数据")] public int m_ResearchId;
        public string m_ResPath { get; private set; }
        public int m_ResearchLevel { get; private set; }
        public int m_ResearchMaxLevel { get; private set; }
        public int m_IsResearching { get; private set; }
        public long m_researchTimeStamp { get; private set; }
        public AttributeType m_ResearchType { get; private set; }
        public DigResearchData m_ResearchData { get; private set; }
        public AttributeData m_AttributeData { get; private set; }

        [Header("研究父节点")] public List<CommonResearchItem> m_ParentNode;

        // [Header("研究子节点")] public List<CommonResearchItem> m_ChildNode;
        [Header("研究状态线")] public GameObject[] m_CompleteLine;

        [Header("研究状态")] public ResearchState m_ResearchState;
        public GameObject m_UnLockState, m_MinState, m_MiddleState, m_MaxState;
        public GameObject m_Selected;
        public GameObject m_Researching;
        public Image m_CanProcess;
        public TextMeshProUGUI m_ProcessText;
        [Header("研究Icon")] public Image m_Icon;

        // 逻辑属性
        public Action<CommonResearchItem> m_ClickItem;
        private bool m_IsSelected;
        private EventGroup m_EventGroup = new();


        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.OnUpdateResearchTime, OnUpdateResearchTime);
            m_EventGroup.Register(LogicEvent.OnResearching, OnResearching);
            Init();
        }

        //初始化研究
        private void Init()
        {
            m_ResearchData = ResearchManager.Ins.GetResearchData(m_ResearchId);
            var attributeId = m_ResearchData.ResearchAttrGroup;
            var resId = m_ResearchData.ResID;
            m_ResPath = ResearchManager.Ins.GetResData(resId).Res;
            m_ResearchType = (AttributeType)ResearchManager.Ins.GetAttributeData(attributeId).Type;
            m_AttributeData = ResearchManager.Ins.GetAttributeData(attributeId);
            if (ResearchManager.Ins.ResearchMap.ContainsKey(m_ResearchId))
            {
                var gameResearchData = ResearchManager.Ins.ResearchMap[m_ResearchId];
                m_ResearchLevel = gameResearchData.ResearchLevel;
                m_IsResearching = gameResearchData.IsResearching;
                m_researchTimeStamp = gameResearchData.ResearchTimeStamp;
            }

            m_ResearchMaxLevel = m_ResearchData.LvlMax;
            UpdateResearchIcon(m_ResPath);
            UpdateResearchState(m_ResearchLevel);
            UpdateResearchTime();
        }

        /// <summary>
        /// 更新研究Icon
        /// </summary>
        /// <param name="resPath"></param>
        private void UpdateResearchIcon(string resPath)
        {
            UICommonHelper.LoadIcon(m_Icon, resPath);
        }

        //更新研究状态
        private void UpdateResearchState(int level)
        {
            UpdateResearchLevel();

            if (level <= 0)
            {
                if (m_ParentNode.Any(parentItems => parentItems.m_ResearchState != ResearchState.Max))
                {
                    m_ResearchState = ResearchState.UnLock;
                    m_UnLockState.Show();
                    m_MinState.Hide();
                    m_MiddleState.Hide();
                    m_MaxState.Hide();
                    return;
                }

                m_ResearchState = ResearchState.Min;
                m_UnLockState.Hide();
                m_MinState.Show();
                m_MiddleState.Hide();
                m_MaxState.Hide();
            }
            else if (level == m_ResearchMaxLevel)
            {
                m_ResearchState = ResearchState.Max;
                m_UnLockState.Hide();
                m_MinState.Hide();
                m_MiddleState.Hide();
                m_MaxState.Show();
                RefreshCompleteLineState();
            }
            else
            {
                m_ResearchState = ResearchState.Middle;
                m_UnLockState.Hide();
                m_MinState.Hide();
                m_MiddleState.Show();
                m_MaxState.Hide();
            }
        }

        private void RefreshCompleteLineState()
        {
            if (m_CompleteLine.Length == 0) return;

            foreach (var completeLine in m_CompleteLine)
            {
                completeLine.Show();
            }
        }

        //更新研究等级
        private void UpdateResearchLevel()
        {
            m_ProcessText.text = $"{m_ResearchLevel}/{m_ResearchMaxLevel}";
            m_CanProcess.fillAmount = (float)m_ResearchLevel / m_ResearchMaxLevel;
        }

        /// <summary>
        /// 初始更新是否处于研究状态
        /// 0 false 1 true
        /// </summary>
        private void UpdateResearchTime()
        {
            switch (m_IsResearching)
            {
                case 0:
                    m_Researching.Hide();
                    break;
                case 1:
                    m_Researching.Show();
                    break;
            }
        }

        //游戏中接收事件更新是否处于研究状态
        private void OnUpdateResearchTime(int eventId, object data)
        {
            var (id, time) = (ValueTuple<int, long>)data;

            if (m_ResearchId == id)
            {
                m_Researching.Show();
            }
        }

        //接收事件更新已完成的研究
        private void OnResearching(int eventId, object data)
        {
            var (id, level) = (ValueTuple<int, int>)data;
            if (m_ResearchId == id)
            {
                m_Researching.Hide();
                m_ResearchLevel = level;
                EventManager.Call(LogicEvent.ResearchLevelChanged, this);
            }

            UpdateResearchState(m_ResearchLevel);
            // else
            // {
            //     UpdateResearchState(m_ResearchLevel);
            // }
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
            m_ClickItem = null;
        }

        public void OnBtnItemClick()
        {
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