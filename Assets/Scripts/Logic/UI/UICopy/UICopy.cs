using System;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;
using UnityTimer;

namespace Logic.UI.UICopy
{
    public class UICopy : UIPage
    {
        public TextMeshProUGUI m_DiamondKeyCount;
        public TextMeshProUGUI m_CoinKeyCount;
        public TextMeshProUGUI m_OilKeyCount;
        public TextMeshProUGUI m_TrophyKeyCount;
        public TextMeshProUGUI m_CDTimer;

        private Timer m_Timer;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.CopyKeyChanged, (i, o) => OnCopyKeyChanged());
        }

        private void OnEnable()
        {
            DateTime now = DateTime.Now;
            DateTime midnight = DateTime.Today.AddDays(1);
            TimeSpan timeLeft = midnight - now;
            int secondsLeft = (int)timeLeft.TotalSeconds;

            m_CDTimer.text = TimeHelper.FormatSecond(secondsLeft);

            m_Timer = Timer.Register(1f, () =>
            {
                secondsLeft--;
                m_CDTimer.text = TimeHelper.FormatSecond(secondsLeft);
                if (secondsLeft <= 0)
                {
                    CopyManager.Ins.SendC2SUpdateCopyKeyCount();
                    secondsLeft = 24 * 60 * 60;
                }
            }, null, true, true, this);

            OnCopyKeyChanged();
        }

        private void OnDisable()
        {
            m_Timer?.Cancel();
        }

        public async void OnClick_EnterDiamondCopy()
        {
            await UIManager.Ins.OpenUI<UICopyEnter>(LevelType.DiamondCopy);
        }

        public async void OnClick_EnterCoinCopy()
        {
            await UIManager.Ins.OpenUI<UICopyEnter>(LevelType.CoinCopy);
        }

        public async void OnClick_EnterOilCopy()
        {
            await UIManager.Ins.OpenUI<UIOilCopyEnter>();
        }

        public async void OnClick_EnterTrophyCopy()
        {
            await UIManager.Ins.OpenUI<UICopyEnter>(LevelType.TrophyCopy);
        }

        /// <summary>
        /// 副本钥匙变化
        /// </summary>
        private void OnCopyKeyChanged()
        {
            m_DiamondKeyCount.text = $"{CopyManager.Ins.m_DiamondCopyData.KeyCount}/2";
            m_CoinKeyCount.text = $"{CopyManager.Ins.m_CoinCopyData.KeyCount}/2";
            m_OilKeyCount.text = $"{CopyManager.Ins.m_OilCopyData.KeyCount}/2";
            m_TrophyKeyCount.text = $"{CopyManager.Ins.m_TrophyCopyData.KeyCount}/2";
        }
    }
}