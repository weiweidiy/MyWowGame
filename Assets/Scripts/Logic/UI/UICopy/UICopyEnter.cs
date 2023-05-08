using System;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;
using UnityEngine;

namespace Logic.UI.UICopy
{
    public class UICopyEnter : UIPage
    {
        public GameObject m_DiamondNode;
        public GameObject m_CoinNode;
        public TextMeshProUGUI m_KeyCount;
        public TextMeshProUGUI m_Level;
        public TextMeshProUGUI m_Reward;
        public GameObject m_ImgYet;
        public GameObject m_ImgNow;
        public GameObject m_CantRight;
        public GameObject m_CantLeft;
        public GameObject m_BtnRight;
        public GameObject m_BtnLeft;

        private LevelType m_LevelType;

        public override void OnShow()
        {
            m_LevelType = (LevelType)m_OpenData_;
            switch (m_LevelType)
            {
                case LevelType.DiamondCopy:
                    m_DiamondNode.Show();
                    m_CoinNode.Hide();
                    m_CurSelectLevel = CopyManager.Ins.m_DiamondCopyData.m_Level;
                    m_BaseLevel = CopyManager.Ins.m_DiamondCopyData.m_Level;
                    m_KeyCount.text = $"{CopyManager.Ins.m_DiamondCopyData.m_KeyCount}/2";
                    ShowLevel();
                    break;
                case LevelType.CoinCopy:
                    m_DiamondNode.Hide();
                    m_CoinNode.Show();
                    m_CurSelectLevel = CopyManager.Ins.m_CoinCopyData.m_Level;
                    m_BaseLevel = CopyManager.Ins.m_CoinCopyData.m_Level;
                    m_KeyCount.text = $"{CopyManager.Ins.m_CoinCopyData.m_KeyCount}/2";
                    ShowLevel();
                    break;
                case LevelType.EngineCopy:
                    m_DiamondNode.Hide();
                    m_CoinNode.Hide();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int m_CurSelectLevel = 0;
        private int m_BaseLevel;

        private void ShowLevel()
        {
            m_Level.text = m_CurSelectLevel.ToString();

            switch (m_LevelType)
            {
                case LevelType.DiamondCopy:
                    m_Reward.text = CopyManager.Ins.GetCopyDiamondReward(m_CurSelectLevel) + "";
                    break;
                case LevelType.CoinCopy:
                    m_Reward.text = CopyManager.Ins.GetCopyCoinReward(m_CurSelectLevel).ToUIString();
                    break;
                case LevelType.EngineCopy:
                    m_Reward.text = "";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (m_CurSelectLevel <= 1)
            {
                m_CantLeft.Show();
                m_BtnLeft.Hide();
            }
            else
            {
                m_CantLeft.Hide();
                m_BtnLeft.Show();
            }

            if (m_CurSelectLevel >= m_BaseLevel)
            {
                m_CantRight.Show();
                m_BtnRight.Hide();
            }
            else
            {
                m_CantRight.Hide();
                m_BtnRight.Show();
            }

            if (m_CurSelectLevel == m_BaseLevel)
            {
                m_ImgYet.Hide();
                m_ImgNow.Show();
            }
            else
            {
                m_ImgYet.Show();
                m_ImgNow.Hide();
            }
        }

        public void OnClickLeft()
        {
            m_CurSelectLevel--;
            ShowLevel();
        }

        public void OnClickRight()
        {
            m_CurSelectLevel++;
            ShowLevel();
        }

        public void OnClickEnter()
        {
            m_LevelType = (LevelType)m_OpenData_;
            int _KeyCount = 0;
            switch (m_LevelType)
            {
                case LevelType.DiamondCopy:
                    _KeyCount = CopyManager.Ins.m_DiamondCopyData.m_KeyCount;
                    break;
                case LevelType.CoinCopy:
                    _KeyCount = CopyManager.Ins.m_CoinCopyData.m_KeyCount;
                    break;
                case LevelType.EngineCopy:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_KeyCount <= 0)
            {
                EventManager.Call(LogicEvent.ShowTips, "钥匙不足");
                return;
            }

            CopyManager.Ins.SendEnterCopy(m_LevelType, m_CurSelectLevel);
            Hide();
        }
    }
}