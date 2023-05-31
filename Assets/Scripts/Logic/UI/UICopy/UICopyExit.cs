using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.UI.UICopy
{
    /// <summary>
    /// 副本
    /// 这里只有成功通关副本才会调用到
    /// </summary>
    public class UICopyExit : UIPage
    {
        public GameObject m_Diamond;
        public GameObject m_Coin;
        public GameObject m_Trophy;
        public TextMeshProUGUI m_Reward;
        
        public override void OnShow()
        {
            var _Data = (S2C_ExitCopy)m_OpenData_;
            switch ((LevelType)_Data.LevelType)
            {
                case LevelType.DiamondCopy:
                    m_Diamond.Show();
                    m_Coin.Hide();
                    m_Trophy.Hide();
                    m_Reward.text = _Data.RewardCount.ToString();
                    break;
                case LevelType.CoinCopy:
                    m_Diamond.Hide();
                    m_Coin.Show();
                    m_Trophy.Hide();
                    m_Reward.text = CopyManager.Ins.GetCopyCoinReward(_Data.Level - 1).ToUIString();
                    break;
                case LevelType.OilCopy:
                    m_Diamond.Hide();
                    m_Coin.Hide();
                    m_Trophy.Hide();
                    break;
                case LevelType.TrophyCopy:
                    m_Diamond.Hide();
                    m_Coin.Hide();
                    m_Trophy.Show();
                    m_Reward.text = CopyManager.Ins.GetCopyTrophyReward(_Data.Level - 1).ToUIString();
                    break;
            }
        }

        public void OnClickGet()
        {
            Hide();
        }
    }

}