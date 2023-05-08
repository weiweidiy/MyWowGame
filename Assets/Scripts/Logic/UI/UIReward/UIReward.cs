using System.Collections.Generic;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIReward
{
    public class UIReward : UIPage
    {
        public GameObject m_RewardRoot;
        public GameObject m_RewardItem;
        private List<GameObject> m_RewardItemList = new List<GameObject>(64);
        public Button m_BtnClose;

        private void Awake()
        {
            m_BtnClose.onClick.AddListener(OnBtnCloseClick);
        }

        public override void OnShow()
        {
            base.OnShow();
        }

        private void OnBtnCloseClick()
        {
            Hide();
        }

        private void ShowReward()
        {
        }
    }
}