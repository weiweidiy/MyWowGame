using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.Helper;
using Logic.Common;
using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        public SummonData m_SummonData;
        public GroupData m_GroupData;

        #region 初始化商店数据

        public void InitShop(DummyDB pDB)
        {
            pDB.m_ShopSkillData = new GameShopCardData { ID = 3001, Level = 1, Exp = 0, TotalExp = 0 };
            pDB.m_ShopPartnerData = new GameShopCardData { ID = 4001, Level = 1, Exp = 0, TotalExp = 0 };
            pDB.m_ShopEquipData = new GameShopCardData { ID = 1001, Level = 1, Exp = 0, TotalExp = 0 };
        }

        #endregion

        #region 抽卡相关逻辑

        public void GetSummonData(int id)
        {
            m_SummonData = SummonCfg.GetData(id);
        }

        public void GetGroupData(int id)
        {
            m_GroupData = GroupCfg.GetData(id);
        }

        public int GetGroupIDFromSummon(int id)
        {
            /*
             * Summon表中
             * 权重及概率处理
             */
            GetSummonData(id);

            var qualityWeights = new[]
            {
                m_SummonData.NormalWight, // 白卡权值
                m_SummonData.AdvancedWight, // 绿卡权值
                m_SummonData.RareWight, // 蓝卡权值
                m_SummonData.EpiclWight, // 紫卡权值
                m_SummonData.LegendaryWight, // 橙卡权值
                m_SummonData.MythicWight, // 青卡权值
                m_SummonData.TransWight // 红卡权值
            };

            var groupId = m_SummonData.Type switch
            {
                1 => 1000,
                2 => 2000,
                3 => 3000,
                4 => 4000,
                _ => throw new ArgumentOutOfRangeException()
            };

            var qualityValue = GetQualityValue(qualityWeights);
            groupId += qualityValue;

            return groupId;
        }

        /// <summary>
        /// 根据groupID返回ItemID，ItemMinCount,ItemMaxCount
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<int> GetItemDetails(int groupID)
        {
            GetGroupData(groupID);
            var itemId = m_GroupData.ItemID;
            var itemWeight = m_GroupData.ItemWight;
            var itemMinCount = m_GroupData.ItemMinCount;
            var itemMaxCount = m_GroupData.ItemMaxCount;
            var qualityValue = GetQualityValue(itemWeight.ToArray());
            var list = new List<int>
            {
                itemId[qualityValue],
                itemMinCount[qualityValue],
                itemMaxCount[qualityValue]
            };
            return list;
        }

        /// <summary>
        /// 根据groupId返回ItemID
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int GetItemID(int groupID)
        {
            /*
             * Group表中
             * 权重及概率处理
             */
            GetGroupData(groupID);

            var itemId = m_GroupData.ItemID;
            var itemWeight = m_GroupData.ItemWight;
            var qualityValue = GetQualityValue(itemWeight.ToArray());
            return itemId[qualityValue];
        }

        /// <summary>
        /// 加权计算
        /// </summary>
        /// <param name="weight"></param>
        /// <returns>index</returns>
        public int GetQualityValue(int[] weight)
        {
            // 总权值
            var maxWeight = weight.Sum();
            // 随机权值
            var randomWeight = RandomHelper.Range(1, maxWeight + 1);
            // 返回品质
            var qualityValue = 0;
            var j = weight.Length;
            for (var i = 0; i < j; i++)
            {
                qualityValue += weight[i];
                if (randomWeight <= qualityValue)
                {
                    return i;
                }
            }

            return 0;
        }

        /// <summary>
        /// 抽卡
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int GetRandomItemID(int groupID)
        {
            return GetItemID(groupID);
        }

        #endregion

        #region 装备商店相关逻辑

        public void UpdateGameShopEquipData(int id, int count)
        {
            if (id >= 1010)
            {
                return;
            }

            GetSummonData(id);
            var data = m_DB.m_ShopEquipData;
            data.Exp += count;
            data.TotalExp += count;
            if (data.Exp >= m_SummonData.LevelExp)
            {
                data.Level++;
                data.ID++;
                if (data.ID >= 1010)
                {
                    data.Exp = 0;
                }
                else
                {
                    data.Exp -= m_SummonData.LevelExp;
                }
            }

            m_DB.m_ShopEquipData = data;
        }

        public void OnUpdateGameShopEquipData()
        {
            SendMsg(new S2C_UpdateDrawCardData()
            {
                DrawCardType = (int)DrawCardType.Equip,
                DrawCardId = m_DB.m_ShopEquipData.ID,
                DrawCardLevel = m_DB.m_ShopEquipData.Level,
                DrawCardExp = m_DB.m_ShopEquipData.Exp,
                DrawCardTotalExp = m_DB.m_ShopEquipData.TotalExp
            });
        }

        #endregion

        #region 技能商店相关逻辑

        public void UpdateGameShopSkillData(int id, int count)
        {
            if (id >= 3010)
            {
                return;
            }

            GetSummonData(id);
            var data = m_DB.m_ShopSkillData;
            data.Exp += count;
            data.TotalExp += count;
            if (data.Exp >= m_SummonData.LevelExp)
            {
                data.Level++;
                data.ID++;
                if (data.ID >= 3010)
                {
                    data.Exp = 0;
                }
                else
                {
                    data.Exp -= m_SummonData.LevelExp;
                }
            }

            m_DB.m_ShopSkillData = data;
        }

        public void OnUpdateGameShopSkillData()
        {
            SendMsg(new S2C_UpdateDrawCardData()
            {
                DrawCardType = (int)DrawCardType.Skill,
                DrawCardId = m_DB.m_ShopSkillData.ID,
                DrawCardLevel = m_DB.m_ShopSkillData.Level,
                DrawCardExp = m_DB.m_ShopSkillData.Exp,
                DrawCardTotalExp = m_DB.m_ShopSkillData.TotalExp
            });
        }

        #endregion

        #region 伙伴商店相关逻辑

        public void UpdateGameShopPartnerData(int id, int count)
        {
            if (id >= 4010)
            {
                return;
            }

            GetSummonData(id);
            var data = m_DB.m_ShopPartnerData;
            data.Exp += count;
            data.TotalExp += count;
            if (data.Exp >= m_SummonData.LevelExp)
            {
                data.Level++;
                data.ID++;
                if (data.ID >= 4010)
                {
                    data.Exp = 0;
                }
                else
                {
                    data.Exp -= m_SummonData.LevelExp;
                }
            }

            m_DB.m_ShopPartnerData = data;
        }

        public void OnUpdateGameShopPartnerData()
        {
            SendMsg(new S2C_UpdateDrawCardData()
            {
                DrawCardType = (int)DrawCardType.Partner,
                DrawCardId = m_DB.m_ShopPartnerData.ID,
                DrawCardLevel = m_DB.m_ShopPartnerData.Level,
                DrawCardExp = m_DB.m_ShopPartnerData.Exp,
                DrawCardTotalExp = m_DB.m_ShopPartnerData.TotalExp
            });
        }

        #endregion

        #region 商店抽数及钻石消耗相关逻辑

        private int GetDrawCardCount(DrawCardCostType type)
        {
            switch (type)
            {
                case DrawCardCostType.Draw11CardCost:
                    UpdateDiamond(-GameDefine.Draw11CardCost);
                    return 11;
                case DrawCardCostType.Draw35CardCost:
                    UpdateDiamond(-GameDefine.Draw35CardCost);
                    return 35;
            }

            return 0;
        }

        #endregion
    }
}