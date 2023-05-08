using System;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Networks;
using TMPro;
using UnityEngine;

namespace Logic.UI.Cells
{
    public class CommonEngineItem : MonoBehaviour
    {
        public Action<CommonEngineItem> m_ClickEngine;
        public GameObject m_Selected;
        public GameEngineData m_GameEngineData;
        public EngineData m_EngineData;
        public AttributeData m_AttributeData;
        public GameObject m_IsOn;
        public TextMeshProUGUI m_Level;
        public GameObject m_RemoveSelected;
        public bool m_IsRemoveSelected;

        public EventGroup m_EventGroup = new();

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.EngineOn, OnEngineOn);
            m_EventGroup.Register(LogicEvent.EngineOff, OnEngineOff);
            m_EventGroup.Register(LogicEvent.EngineRemove, OnEngineRemove);
            m_EventGroup.Register(LogicEvent.EngineIntensify, OnEngineIntensify);
        }

        public void Init(GameEngineData pData)
        {
            m_GameEngineData = pData;
            m_EngineData = EngineCfg.GetData(pData.m_TypeId);
            m_AttributeData = AttributeCfg.GetData(pData.m_AttrId);
            m_Level.text = $"Lv{m_GameEngineData.m_Level}";
            UpdateEngineInfo();
        }

        private void UpdateEngineInfo()
        {
            if (m_GameEngineData == null) return;
            if (EngineManager.Ins.IsOn(m_GameEngineData.m_Id))
            {
                m_IsOn.Show();
            }
            else
            {
                m_IsOn.Hide();
            }
        }

        private void OnEngineOn(int eventId, object data)
        {
            var engineId = (int)data;
            if (engineId == m_GameEngineData.m_Id)
            {
                m_IsOn.Show();
            }
        }

        private void OnEngineOff(int eventId, object data)
        {
            var engineId = (int)data;
            if (engineId == m_GameEngineData.m_Id)
            {
                m_IsOn.Hide();
            }
        }

        private void OnEngineRemove(int eventId, object data)
        {
            var engineId = (int)data;
            if (engineId == m_GameEngineData.m_Id)
            {
                this.gameObject.Destroy();
            }
        }

        private void OnEngineIntensify(int eventId, object data)
        {
            var (engineId, engineLevel) = (ValueTuple<int, int>)data;
            if (engineId == m_GameEngineData.m_Id)
            {
                m_Level.text = $"LV{engineLevel}";
            }
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
            m_ClickEngine = null;
        }

        public void OnClickEngine()
        {
            m_ClickEngine?.Invoke(this);
        }

        public void ShowSelected()
        {
            m_Selected.Show();
        }

        public void HideSelected()
        {
            m_Selected.Hide();
        }

        public void UpdateRemoveSelected()
        {
            m_IsRemoveSelected = !m_IsRemoveSelected;
            m_RemoveSelected.SetActive(m_IsRemoveSelected);
        }
    }
}