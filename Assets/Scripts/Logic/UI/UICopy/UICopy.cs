using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;

namespace Logic.UI.UICopy
{
    public class UICopy : UIPage
    {
        public TextMeshProUGUI m_DiamondKeyCount;
        public TextMeshProUGUI m_CoinKeyCount;
        public TextMeshProUGUI m_OilKeyCount;
        public TextMeshProUGUI m_TrophyKeyCount;
        public TextMeshProUGUI m_CDTimer;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.CopyKeyChanged, (i, o) => OnCopyKeyChanged());
            m_EventGroup.Register(LogicEvent.TimeNextDaySecondsChanged, OnTimeNextDaySecondsChanged);
        }

        public override void OnShow()
        {
            base.OnShow();
            OnCopyKeyChanged();
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

        /// <summary>
        /// 副本跨天倒计时刷新
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="data"></param>
        private void OnTimeNextDaySecondsChanged(int eventId, object data)
        {
            var secondsLeft = (int)data;
            m_CDTimer.text = TimeHelper.FormatSecond(secondsLeft);
        }
    }
}