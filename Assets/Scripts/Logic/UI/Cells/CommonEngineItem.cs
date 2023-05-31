using System;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonEngineItem : MonoBehaviour
    {
        public Action<CommonEngineItem> m_ClickEngine;
        public GameObject m_Selected;
        public GameEngineData m_GameEngineData;
        public EngineData m_EngineData;
        public AttributeData m_AttributeData;
        public ResData m_ResData;
        public GameObject m_IsOn;
        public Image m_Icon;
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
            m_EngineData = EngineCfg.GetData(pData.TypeId);
            m_AttributeData = AttributeCfg.GetData(pData.AttrId);
            m_ResData = ResCfg.GetData(m_EngineData.ResID);
            UICommonHelper.LoadIcon(m_Icon,m_ResData.Res);
            m_Level.text = $"Lv{m_GameEngineData.Level}";
            UpdateEngineInfo();
        }

        private void UpdateEngineInfo()
        {
            if (m_GameEngineData == null) return;
            if (EngineManager.Ins.IsOn(m_GameEngineData.Id))
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
            if (engineId == m_GameEngineData.Id)
            {
                m_IsOn.Show();
            }
        }

        private void OnEngineOff(int eventId, object data)
        {
            var engineId = (int)data;
            if (engineId == m_GameEngineData.Id)
            {
                m_IsOn.Hide();
            }
        }

        private void OnEngineRemove(int eventId, object data)
        {
            var engineId = (int)data;
            if (engineId == m_GameEngineData.Id)
            {
                this.gameObject.Destroy();
            }
        }

        private void OnEngineIntensify(int eventId, object data)
        {
            var (engineId, engineLevel) = (ValueTuple<int, int>)data;
            if (engineId == m_GameEngineData.Id)
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