using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using Configs;
using Framework.Extension;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using Logic.UI.Common.Effect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIPlaceRewards
{
    public class UIPlaceRewards : UIPage
    {
        public TextMeshProUGUI m_PlaceRewardTime;
        public TextMeshProUGUI m_PlaceRewardPercent;
        public TextMeshProUGUI m_PlaceRewardNumber;
        public Button m_BtnConfirm;
        public Transform m_CoinTransform;

        /*
         * 临时的当前按下放置奖励按钮后的时间和金币
         * 在点击确认后再实际存储
         */
        private long m_TempBtnPlaceRewardClickTime;
        private BigDouble m_PlaceRewardCoin;

        // 放置装备奖励
        public Transform m_CommonPlaceRewardRoot;
        public CommonPlaceRewardItem m_CommonPlaceRewardItem;
        private List<CommonPlaceRewardItem> m_PlaceRewardItemList = new List<CommonPlaceRewardItem>(64);

        private void Awake()
        {
            m_BtnConfirm.onClick.AddListener(OnBtnConfirmClick);
            m_EventGroup.Register(LogicEvent.ShowPlaceReward, OnShowPlaceReward);
        }

        public override void OnShow()
        {
            base.OnShow();
            UpdatePlaceRewardTime();
        }

        private void OnBtnConfirmClick()
        {
            GameDataManager.Ins.BtnPlaceRewardClickTime = m_TempBtnPlaceRewardClickTime;
            // GameDataManager.Ins.Coin += m_PlaceRewardCoin;
            CoinEffectMgr.Ins.StartEffect(transform.position, m_PlaceRewardCoin);
            RewardManager.Ins.SendMsgC2SGetPlaceReward();
            DestroyPlaceRewardItemList();
            Hide();
        }

        /// <summary>
        /// 获得当前关卡放置奖励 /m
        /// 当前关卡掉落 = 掉落基础值(id:106) + 关卡id *（掉落成长系数(107) + 掉落成长体验系数(108)）
        /// 当前关卡放置奖励 = 当前关卡掉落 * 放置奖励倍率(116)
        /// </summary>
        /// <returns></returns>
        private BigDouble GetPlaceRewardPercent()
        {
            var _CfgData = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            var coin = _CfgData.DropBase +
                       GameDataManager.Ins.CurLevelID * (_CfgData.DropBase);
            var multiplier = GameDefine.PlaceRewardMultiplier;

            return BigDouble.Floor(coin * multiplier);
        }

        private void UpdatePlaceRewardTime()
        {
            var tempTime =
                TimeHelper.GetBetween(DateTime.UtcNow, GameDataManager.Ins.BtnPlaceRewardClickTime);
            var maxTime = GameDefine.PlaceRewardMaxTime * 60;
            var placeRewardTime = tempTime >= maxTime ? maxTime : tempTime;
            var placeRewardMinuteTime = placeRewardTime / 60;
            // 放置奖励 = 当前关卡放置奖励 * 放置时间
            m_PlaceRewardCoin = placeRewardMinuteTime * GetPlaceRewardPercent();
            // Sting.Format
            var string1 = "<color=#FFEBC3>收集 </color>";
            var string2 = $"<color=#BE8AFF>{placeRewardMinuteTime.ToString()}</color>";
            var string3 = "</color><color=#FFEBC3> 分钟</color>";
            m_PlaceRewardTime.text = $"{string1}{string2}{string3}";
            m_PlaceRewardPercent.text = $"{GetPlaceRewardPercent().ToUIString()}/m";
            m_PlaceRewardNumber.text = m_PlaceRewardCoin.ToUIString();
            // 存下当前按下放置奖励按钮后的时间
            m_TempBtnPlaceRewardClickTime = TimeHelper.GetUnixTimeStamp();

            // 放置装备奖励
            var count = (int)(GameDefine.PlaceRewardFactor * placeRewardMinuteTime);
            if (count > 0)
            {
                RewardManager.Ins.SendMsgC2SPlaceReward(count);
            }
        }

        private void OnShowPlaceReward(int eventId, object data)
        {
            StartCoroutine(OnShowPlaceRewardCoroutine(data));
        }

        private IEnumerator OnShowPlaceRewardCoroutine(object data)
        {
            var (rewardId, rewardCount) = (ValueTuple<List<int>, List<int>>)data;
            var delay = new WaitForSeconds(0.02f);
            for (var i = 0; i < rewardId.Count; i++)
            {
                var placeRewardItem = Instantiate(m_CommonPlaceRewardItem, m_CommonPlaceRewardRoot);
                placeRewardItem.Init(rewardId[i], rewardCount[i]);
                placeRewardItem.m_Click += OnClick;
                placeRewardItem.Show();
                m_PlaceRewardItemList.Add(placeRewardItem);
                yield return delay;
            }
        }

        private void DestroyPlaceRewardItemList()
        {
            foreach (var placeRewardItem in m_PlaceRewardItemList)
            {
                Destroy(placeRewardItem.gameObject);
            }

            m_PlaceRewardItemList.Clear();
        }

        private async void OnClick(CommonPlaceRewardItem pItem, ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                    var equipData = (itemType, pItem.m_EquipData.ID);
                    await UIManager.Ins.OpenUI<UICommonCardInfo>(equipData);
                    break;
                case ItemType.Skill:
                    var skillData = (itemType, pItem.m_SkillData.ID);
                    await UIManager.Ins.OpenUI<UICommonCardInfo>(skillData);
                    break;
                case ItemType.Partner:
                    var partnerData = (itemType, pItem.m_PartnerData.ID);
                    await UIManager.Ins.OpenUI<UICommonCardInfo>(partnerData);
                    break;
            }
        }
    }
}