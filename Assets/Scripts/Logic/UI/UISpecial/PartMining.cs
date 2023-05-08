using Framework.EventKit;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpecial
{
    public class PartMining : MonoBehaviour
    {
        public Button m_BtnEnter;
        public TextMeshProUGUI m_Number;

        public EventGroup m_EventGroup = new();

        private void Awake()
        {
            m_BtnEnter.onClick.AddListener(OnBtnEnterClick);
            m_EventGroup.Register(LogicEvent.MiningDataChanged, OnHammerChanged);
            Refresh();
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        private async void OnBtnEnterClick()
        {
            await UIManager.Ins.OpenUI<UIMining.UIMining>();
        }

        private void OnHammerChanged(int eventId, object data)
        {
            if ((MiningType)data == MiningType.Hammer)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            m_Number.text = $"{MiningManager.Ins.m_MiningData.m_HammerCount}/{GameDefine.MaxHammerCount}";
        }
    }
}