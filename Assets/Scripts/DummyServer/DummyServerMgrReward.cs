using System.Collections.Generic;
using Configs;
using Framework.Helper;
using Logic.Common;
using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        #region 考古奖励相关逻辑

        public ItemType GetItemType(int pRewardID)
        {
            return (ItemType)ItemCfg.GetData(pRewardID).Type;
        }

        public void OnMiningReward(int treasureType)
        {
            var type = (MiningType)treasureType;
            switch (type)
            {
                case MiningType.WeaponTreasure:
                case MiningType.ArmorTreasure:
                case MiningType.EquipTreasure:
                    EquipTreasureReward(treasureType);
                    break;
                case MiningType.Hammer:
                case MiningType.Bomb:
                case MiningType.Scope:
                case MiningType.CopperMine:
                case MiningType.SilverMine:
                case MiningType.GoldMine:
                case MiningType.Coin:
                case MiningType.Diamond:
                case MiningType.BigCoin:
                case MiningType.Honor:
                case MiningType.Gear:
                    TreasureReward(treasureType);
                    break;
            }
        }

        private void TreasureReward(int treasureType)
        {
            var type = (MiningType)treasureType;
            var groupId = treasureType;
            var itemDetails = GetItemDetails(groupId);
            var itemID = itemDetails[0];
            var count = RandomHelper.Range(itemDetails[1], itemDetails[2] + 1);
            switch (type)
            {
                case MiningType.CopperMine:
                case MiningType.SilverMine:
                case MiningType.GoldMine:
                    // 矿石获得量增加 %
                    var researchMineObtainAmount = m_DB.m_ResearchEffectData.ResearchMineObtainAmount;
                    count = (int)(count * (1 + researchMineObtainAmount));
                    OnIncreaseMiningData((int)treasureType, count);
                    break;
                case MiningType.Hammer:
                case MiningType.Bomb:
                case MiningType.Scope:
                case MiningType.Honor:
                case MiningType.Gear:
                    OnIncreaseMiningData((int)treasureType, count);
                    break;
                case MiningType.Coin:
                case MiningType.BigCoin:
                    //TODO:当前在客户端更新获取的金币数据,服务器存储金币时需更新服务器金币数据
                    break;
                case MiningType.Diamond:
                    UpdateDiamond(count);
                    break;
            }

            SendMsg(new S2C_MiningReward()
            {
                TreasureType = treasureType,
                RewardId = itemID,
                RewardCount = count,
            });
        }

        private void EquipTreasureReward(int treasureType)
        {
            var groupId = treasureType;
            var itemDetails = GetItemDetails(groupId);
            var itemID = itemDetails[0];
            var count = RandomHelper.Range(itemDetails[1], itemDetails[2] + 1);

            UpdateEquipData(itemID, count);

            SendMsg(new S2C_MiningReward()
            {
                TreasureType = treasureType,
                RewardId = itemID,
                RewardCount = count,
            });
        }

        #endregion

        #region 装备奖励相关逻辑

        /// <summary>
        /// 更新武器防具技能伙伴数据
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="count"></param>
        public void UpdateEquipData(int itemID, int count)
        {
            var itemType = GetItemType(itemID);
            switch (itemType)
            {
                case ItemType.Weapon:
                    //更新武器数据
                    var weaponData = m_DB.m_WeaponList.Find(pData =>
                    {
                        if (pData.EquipID == itemID)
                        {
                            pData.Count += count;
                            return true;
                        }

                        return false;
                    });

                    if (weaponData == null)
                    {
                        m_DB.m_WeaponList.Add(new GameEquipData { EquipID = itemID, Count = count, Level = 1 });
                    }

                    SendMsg(new S2C_EquipListUpdate()
                    {
                        WeaponList = m_DB.m_WeaponList,
                        ArmorList = m_DB.m_ArmorList
                    });
                    break;
                case ItemType.Armor:
                    //更新防具数据
                    var armorData = m_DB.m_ArmorList.Find(pData =>
                    {
                        if (pData.EquipID == itemID)
                        {
                            pData.Count += count;
                            return true;
                        }

                        return false;
                    });

                    if (armorData == null)
                    {
                        m_DB.m_ArmorList.Add(new GameEquipData { EquipID = itemID, Count = count, Level = 1 });
                    }

                    SendMsg(new S2C_EquipListUpdate()
                    {
                        WeaponList = m_DB.m_WeaponList,
                        ArmorList = m_DB.m_ArmorList
                    });

                    break;
                case ItemType.Skill:
                    //更新技能数据
                    var gameSkillData = m_DB.m_SkillList.Find(pData =>
                    {
                        if (pData.SkillID == itemID)
                        {
                            pData.Count += count;
                            return true;
                        }

                        return false;
                    });

                    if (gameSkillData == null)
                    {
                        m_DB.m_SkillList.Add(new GameSkillData { SkillID = itemID, Count = count, Level = 1 });
                    }

                    SendMsg(new S2C_SkillListUpdate()
                    {
                        SkillList = m_DB.m_SkillList
                    });

                    break;
                case ItemType.Partner:
                    //更新伙伴数据
                    var gamePartnerData = m_DB.m_PartnerList.Find(pData =>
                    {
                        if (pData.PartnerID == itemID)
                        {
                            pData.Count += count;
                            return true;
                        }

                        return false;
                    });

                    if (gamePartnerData == null)
                    {
                        m_DB.m_PartnerList.Add(new GamePartnerData
                            { PartnerID = itemID, Count = count, Level = 1 });
                    }

                    SendMsg(new S2C_PartnerListUpdate()
                    {
                        PartnerList = m_DB.m_PartnerList
                    });

                    break;
            }
        }

        #endregion

        #region 放置奖励相关逻辑

        public HandUpData m_HandUpData;

        private int[] m_HandUpLevel =
        {
            10, 20, 30, 40, 50, 60, 70, 80, 90, 100,
            110, 120, 130, 140, 150, 160, 170, 180, 190, 200,
            210, 220, 230, 240, 250, 260, 270, 280, 290, 300,
            310, 320, 330, 340, 350, 360, 370, 380, 390, 400,
            410, 420, 430, 440, 450, 460, 470, 480,
            9999
        };

        private int[] m_HandUpId =
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
            21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48,
            9999
        };

        /// <summary>
        /// 获取HandUp表中对应的GroupId
        /// </summary>
        /// <returns></returns>
        public int GetHandUpGroupId()
        {
            var index = 0;
            while (index < m_HandUpLevel.Length && m_DB.m_CurLevelID > m_HandUpLevel[index])
            {
                index++;
            }

            m_HandUpData = HandUpCfg.GetData(m_HandUpId[index]);
            var handUpGroupID = m_HandUpData.GroupID;
            var groupId = handUpGroupID[GetQualityValue(m_HandUpData.Wight.ToArray())];
            return groupId;
        }

        // 放置奖励临时获取奖励
        public List<int> m_TempRewardId = new List<int>();
        public List<int> m_TempRewardCount = new List<int>();

        public void OnPlaceReward(int count)
        {
            m_TempRewardId.Clear();
            m_TempRewardCount.Clear();

            // 获取groupId个数
            var groupIdList = new List<int>();
            for (var i = 0; i < count; i++)
            {
                var groupId = GetHandUpGroupId();
                groupIdList.Add(groupId);
            }

            // 获取放置奖励及个数
            var tempRewardDictionary = new Dictionary<int, int>();
            foreach (var groupId in groupIdList)
            {
                var itemId = GetItemID(groupId);
                if (!tempRewardDictionary.ContainsKey(itemId))
                {
                    tempRewardDictionary.Add(itemId, 1);
                }
                else
                {
                    tempRewardDictionary[itemId]++;
                }
            }

            foreach (var tempReward in tempRewardDictionary)
            {
                m_TempRewardId.Add(tempReward.Key);
                m_TempRewardCount.Add(tempReward.Value);
            }

            SendMsg(new S2C_PlaceReward()
            {
                PlaceRewardId = m_TempRewardId,
                PlaceRewardCount = m_TempRewardCount,
            });
        }

        public void OnGetPlaceReward()
        {
            for (int i = 0; i < m_TempRewardId.Count; i++)
            {
                UpdateEquipData(m_TempRewardId[i], m_TempRewardCount[i]);
            }

            DummyDB.Save(m_DB);
        }

        #endregion

        #region 通用奖励相关逻辑

        public void OnCommonReward(C2S_CommonReward pMsg)
        {
            // TODO: 通用奖励获取逻辑
            SendMsg(new S2C_CommonReward()
            {
            });
        }

        #endregion
    }
}