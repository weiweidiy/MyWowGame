using Configs;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonMiningReward : MonoBehaviour
    {
        public Image m_Quality;
        public Image m_Icon;
        public TextMeshProUGUI m_Count;

        public ItemType GetRewardType(int pRewardID)
        {
            return (ItemType)ItemCfg.GetData(pRewardID).Type;
        }

        public void Init(MiningType treasureType, int rewardId, int rewardCount)
        {
            m_Count.text = $"+{rewardCount}";
            switch (treasureType)
            {
                case MiningType.WeaponTreasure:
                case MiningType.ArmorTreasure:
                case MiningType.EquipTreasure:
                    var rewardType = GetRewardType(rewardId);
                    var itemData = ItemCfg.GetData(rewardId);
                    switch (rewardType)
                    {
                        case ItemType.Weapon:
                        case ItemType.Armor:
                            var equipData = EquipCfg.GetData(rewardId);
                            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
                            UICommonHelper.LoadQuality(m_Quality, equipData.Quality);
                            break;
                        case ItemType.Skill:
                            var skillData = SkillCfg.GetData(rewardId);
                            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
                            UICommonHelper.LoadQuality(m_Quality, skillData.Quality);
                            break;
                        case ItemType.Partner:
                            var partnerData = PartnerCfg.GetData(rewardId);
                            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
                            UICommonHelper.LoadQuality(m_Quality, partnerData.Quality);
                            break;
                    }

                    break;
                case MiningType.Coin:
                case MiningType.BigCoin:
                    GameDataManager.Ins.Coin += rewardCount;
                    LoadSingeRewardIcon(treasureType);
                    break;
                case MiningType.CopperMine:
                case MiningType.SilverMine:
                case MiningType.GoldMine:
                case MiningType.Diamond:
                case MiningType.Honor:
                case MiningType.Gear:
                case MiningType.Hammer:
                case MiningType.Bomb:
                case MiningType.Scope:
                    LoadSingeRewardIcon(treasureType);
                    break;
            }
        }

        private void LoadSingeRewardIcon(MiningType treasureType)
        {
            UICommonHelper.LoadMiningType(m_Icon, treasureType);
            m_Quality.Hide();
        }
    }
}