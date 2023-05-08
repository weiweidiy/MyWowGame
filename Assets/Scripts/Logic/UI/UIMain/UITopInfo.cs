using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Networks;
using TMPro;
using UnityEngine;

namespace Logic.UI.UIMain
{
    public class UITopInfo : UIPage
    {
        public TextMeshProUGUI m_Coin;
        public TextMeshProUGUI m_Diamond;

        // 点击解锁全部开放功能
        public GameObject m_BtnUnlock;

        // 点击增加金币钻石等
        public GameObject m_BtnAdd;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.CoinChanged, OnCoinChanged)
                .Register(LogicEvent.DiamondChanged, OnDiamondChanged);
        }

        private void OnCoinChanged(int arg1, object arg2)
        {
            m_Coin.text = GameDataManager.Ins.Coin.ToUIString();
        }

        private void OnDiamondChanged(int arg1, object arg2)
        {
            m_Diamond.text = GameDataManager.Ins.Diamond.ToString();
        }

        public override void OnShow()
        {
            m_Coin.text = GameDataManager.Ins.Coin.ToUIString();
            m_Diamond.text = GameDataManager.Ins.Diamond.ToString();
        }

        #region 便捷测试相关功能

        public void OnBtnUnlockClick()
        {
            LockStoryManager.Ins.m_IsUnlockAll = true; //开放所有功能和剧情
            EventManager.Call(LogicEvent.UpdateUnlockAll); //开放所有解锁
            PartnerManager.Ins.UpdateAllDoOnCount(); //开放所有上阵伙伴
            SkillManager.Ins.UpdateAllDoOnCount(); //开放所有上阵技能
            EventManager.Call(LogicEvent.ShowTips, "所有开放功能已解锁");
            m_BtnUnlock.Hide();
        }

        public void OnBtnAddClick()
        {
            // 加金币
            GameDataManager.Ins.Coin += 1000000;
            // 加钻石
            NetworkManager.Ins.SendMsg(new C2S_GMAccount());
        }

        #endregion
    }
}