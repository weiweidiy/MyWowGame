using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Logic.Common;
using Logic.UI.Cells;
using Networks;
using UnityEngine;

namespace Logic.Manager
{
    public class MiningManager : Singleton<MiningManager>
    {
        #region 考古相关数据

        public GameMiningData m_MiningData;

        public void Init(S2C_Login pMsg)
        {
            m_MiningData = pMsg.MiningData;
        }

        /// <summary>
        /// TODO:Remove BugIncrease
        /// </summary>
        /// <param name="miningDataType"></param>
        /// <param name="miningUpdateType"></param>
        public void SendMsgC2SUpdateMiningData(MiningType miningDataType, MiningUpdateType miningUpdateType)
        {
            var pMsg = new C2S_UpdateMiningData()
            {
                MiningDataType = (int)miningDataType,
                UpdateType = (int)miningUpdateType,
            };
            NetworkManager.Ins.SendMsg(pMsg);
        }

        public void On_S2C_UpdateMiningData(S2C_UpdateMiningData pMsg)
        {
            switch ((MiningType)pMsg.MiningDataType)
            {
                case MiningType.Gear:
                    m_MiningData.GearCount = pMsg.MiningData.GearCount;
                    break;
                case MiningType.Hammer:
                    m_MiningData.HammerCount = pMsg.MiningData.HammerCount;
                    break;
                case MiningType.CopperMine:
                case MiningType.SilverMine:
                case MiningType.GoldMine:
                    m_MiningData.MineCount = pMsg.MiningData.MineCount;
                    break;
                case MiningType.Bomb:
                    m_MiningData.BombCount = pMsg.MiningData.BombCount;
                    break;
                case MiningType.Scope:
                    m_MiningData.ScopeCount = pMsg.MiningData.ScopeCount;
                    break;
                case MiningType.Door:
                {
                    m_MiningData.FloorCount = pMsg.MiningData.FloorCount;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_5001, m_MiningData.FloorCount);
                }
                    break;
            }

            EventManager.Call(LogicEvent.MiningDataChanged, pMsg.MiningDataType);
        }

        #endregion

        #region 考古道具随机生成逻辑

        private DigMapData m_DigMapData;

        private void GetDigMapData(int id)
        {
            m_DigMapData = DigMapCfg.GetData(id);
        }

        private int GetRewardType(int[] weight)
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
        /// 根据groupID返回ItemID，ItemMinCount,ItemMaxCount
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<int> GetItemDetails(int groupID)
        {
            var groupData = GroupCfg.GetData(groupID);
            // 返回ItemID
            var itemId = groupData.ItemID;
            var itemWeight = groupData.ItemWight;

            // 返回ItemMinCount
            var itemMinCount = groupData.ItemMinCount;
            // 返回ItemMaxCount
            var itemMaxCount = groupData.ItemMaxCount;
            var weightIndex = GetRewardType(itemWeight.ToArray());
            var list = new List<int>
            {
                itemId[weightIndex],
                itemMinCount[weightIndex],
                itemMaxCount[weightIndex]
            };
            return list;
        }

        /// <summary>
        /// 获取随机生成的6组道具的类型
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public List<int> GetMiningPropType(int pID)
        {
            GetDigMapData(pID);

            var tempWeight = m_DigMapData.RewardWeight.ToArray();
            var rewardType = m_DigMapData.RewardType;
            var miningPropType = new List<int>();
            for (var i = 0; i < 6; i++)
            {
                var id = GetRewardType(tempWeight);
                miningPropType.Add(rewardType[id]);
                tempWeight[id] = 0;
            }

            return miningPropType;
        }


        public List<int> GetDefaultOpenMiningProp(int pID)
        {
            GetDigMapData(pID);

            var defaultOpenList = m_DigMapData.Open;

            return defaultOpenList;
        }

        #endregion

        #region 考古三消逻辑

        public Dictionary<int, int> m_PropCountDictionary = new Dictionary<int, int>();

        public List<CommonMiningProp> m_CommonMiningProp = new List<CommonMiningProp>();

        public int m_PropDoTweenCount; // 有动画播放时限制退出及进门

        public void Clear()
        {
            m_PropCountDictionary.Clear();
            // m_PropDoTweenCount = 0;
        }

        public void GetThreeMatchReward(MiningType treasureType)
        {
            m_PropDoTweenCount++;
            var tempList = m_CommonMiningProp
                .Where(commonMiningProp => commonMiningProp.m_TreasureType == treasureType).ToList();
            var tempValue = tempList.Count / 2;
            var tempMiningProp = tempList[tempValue];
            foreach (var commonMiningProp in tempList)
            {
                commonMiningProp.OnThreeMatchReward(tempMiningProp.transform.position, tempMiningProp.m_ID);
            }
        }

        public Vector3 GetThreeMatchPosition(MiningType treasureType)
        {
            var tempList = m_CommonMiningProp
                .Where(commonMiningProp => commonMiningProp.m_TreasureType == treasureType).ToList();
            var tempValue = tempList.Count / 2;
            var targetPosition = tempList[tempValue].transform.position;
            return targetPosition;
        }

        #endregion
    }
}