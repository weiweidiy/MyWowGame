using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Extension;
using Framework.UI;
using Logic.UI.Cells;
using Networks;
using UnityEngine;

namespace Logic.UI.UIUser
{
    public class UIComposeInfo : UIPage
    {
        public Transform m_ItemRoot;
        public CommonShowComposeItem m_ComposeItem;
        private List<CommonShowComposeItem> m_CommonShowComposeItemList = new List<CommonShowComposeItem>(64);

        private WaitForSeconds m_Delay;
        private List<GameComposeData> m_ComposeList;
        private List<GameEquipUpgradeData> m_EquipUpgradeList;
        private List<GamePartnerUpgradeData> m_PartnerUpgradeList;
        private List<GameSkillUpgradeData> m_SkillUpgradeList;

        private void Awake()
        {
            m_Delay = new WaitForSeconds(0.02f);
        }

        public override void OnShow()
        {
            base.OnShow();

            m_CommonShowComposeItemList?.Clear();
            m_EquipUpgradeList?.Clear();
            m_PartnerUpgradeList?.Clear();
            m_SkillUpgradeList?.Clear();

            switch (m_OpenData_)
            {
                case List<GameComposeData> list:
                    m_ComposeList = list;
                    break;
                case (List<GameComposeData>, List<GameEquipUpgradeData>):
                    (m_ComposeList, m_EquipUpgradeList) =
                        (ValueTuple<List<GameComposeData>, List<GameEquipUpgradeData>>)m_OpenData_;
                    break;
                case (List<GameComposeData>, List<GamePartnerUpgradeData>):
                    (m_ComposeList, m_PartnerUpgradeList) =
                        (ValueTuple<List<GameComposeData>, List<GamePartnerUpgradeData>>)m_OpenData_;
                    break;
                case (List<GameComposeData>, List<GameSkillUpgradeData>):
                    (m_ComposeList, m_SkillUpgradeList) =
                        (ValueTuple<List<GameComposeData>, List<GameSkillUpgradeData>>)m_OpenData_;
                    break;
            }

            StartCoroutine(OnShowComposeItemCoroutine());
        }

        private IEnumerator OnShowComposeItemCoroutine()
        {
            foreach (var gameComposeData in m_ComposeList)
            {
                var composeFormItem = Instantiate(m_ComposeItem, m_ItemRoot);
                composeFormItem.Init(gameComposeData.FromID);
                composeFormItem.Show();
                m_CommonShowComposeItemList.Add(composeFormItem);
                yield return m_Delay;
                var composeToItem = Instantiate(m_ComposeItem, m_ItemRoot);
                composeToItem.Init(gameComposeData.ToID, gameComposeData.ToAddCount);
                composeToItem.Show();
                m_CommonShowComposeItemList.Add(composeToItem);
                yield return m_Delay;
            }
        }

        public async void OnBtnCloseClick()
        {
            foreach (var composeItem in m_CommonShowComposeItemList)
            {
                Destroy(composeItem.gameObject);
            }

            // 如果有批量升级的数据弹出升级面板
            if (m_EquipUpgradeList != null && m_EquipUpgradeList.Count != 0)
            {
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(m_EquipUpgradeList);
            }
            else if (m_PartnerUpgradeList != null && m_PartnerUpgradeList.Count != 0)
            {
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(m_PartnerUpgradeList);
            }

            else if (m_SkillUpgradeList != null && m_SkillUpgradeList.Count != 0)
            {
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(m_SkillUpgradeList);
            }

            Hide();
        }
    }
}