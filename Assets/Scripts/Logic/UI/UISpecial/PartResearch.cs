using Framework.EventKit;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpecial
{
    public class PartResearch : MonoBehaviour
    {
        public Button m_BtnEnter;
        public TextMeshProUGUI m_Number;

        public EventGroup m_EventGroup = new();

        private void Awake()
        {
            m_BtnEnter.onClick.AddListener(OnBtnEnterClick);
            m_EventGroup.Register(LogicEvent.MineChanged, (i, o) => OnMineChanged());
            Refresh();
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        private async void OnBtnEnterClick()
        {
            await UIManager.Ins.OpenUI<UIResearch.UIResearch>();
        }

        private void OnMineChanged()
        {
            Refresh();
        }

        private void Refresh()
        {
            m_Number.text = MiningManager.Ins.m_MiningData.MineCount.ToString();
        }
    }
}