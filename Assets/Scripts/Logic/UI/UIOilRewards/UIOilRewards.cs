using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.UI.Cells;
using Logic.UI.Common;
using Networks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIPlaceRewards
{
    public class UIOilRewards : UIPage
    {
        public Transform m_CommonPlaceRewardRoot;
        public CommonPlaceRewardItem m_CommonPlaceRewardItem;
        public TextMeshProUGUI m_Oil;
        public Button m_BtnConfirm;


        private void Awake()
        {
            m_BtnConfirm.onClick.AddListener(OnBtnConfirmClick);

        }

        private void OnBtnConfirmClick()
        {
            Close();
        }

        public override void OnShow()
        {
            base.OnShow();

            var o = m_OpenData_;
            StartCoroutine(ShowOilRewardCoroutine(m_OpenData_));
        }

        private IEnumerator ShowOilRewardCoroutine(object data)
        {
            
            var result = data as S2C_OilCopyReward;

            m_Oil.text = result.Oil.ToString();
            var delay = new WaitForSeconds(0.02f);
            for (var i = 0; i < result.LstRewardId.Count; i++)
            {
                var placeRewardItem = Instantiate(m_CommonPlaceRewardItem, m_CommonPlaceRewardRoot);
                placeRewardItem.Init(result.LstRewardId[i], result.LstRewardCount[i]);
                placeRewardItem.m_Click += OnClick;
                placeRewardItem.Show();

                yield return delay;
            }
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