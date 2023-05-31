using Configs;
using Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DummyServer
{
    /// <summary>
    /// 游戏内根据组内的物品权重列表，随机选中一个物品的算法类
    /// </summary>
    public class GroupRewardGeneraterHelper
    {
        /// <summary>
        /// 根据组列表，生成一组随机物品
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static Tuple<List<int>, List<int>> GenerateByGroups(List<int> groups)
        {
            List<int> _LstId = new List<int>();
            List<int> _LstCount = new List<int>();

            var tempRewardDictionary = new Dictionary<int, int>();
            foreach (var groupId in groups)
            {
                var tupleItem = Generate(groupId); //item1 = itemId, item2 = count

                if (!tempRewardDictionary.ContainsKey(tupleItem.Item1))
                {
                    tempRewardDictionary.Add(tupleItem.Item1, tupleItem.Item2);
                }
                else
                {
                    //同类物品合并
                    tempRewardDictionary[tupleItem.Item1] += tupleItem.Item2;
                }
            }

            foreach (var tempReward in tempRewardDictionary)
            {
                _LstId.Add(tempReward.Key);
                _LstCount.Add(tempReward.Value);
            }

            return new Tuple<List<int>, List<int>>(_LstId, _LstCount);
        }

        /// <summary>
        /// 根据组ID生成一个随机物品
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static Tuple<int, int> Generate(int groupId)
        {
            var groupCfg = GroupCfg.GetData(groupId);
            var arrItemId = groupCfg.ItemID;
            var arrItemWeight = groupCfg.ItemWight;
            var index = GetQualityValue(arrItemWeight.ToArray());
            Debug.Assert(index < arrItemId.Count, "奖励宝箱索引越界:" + "index " + index + " groupId " + groupId);
            var itemId = arrItemId[index];
            var randomCount = RandomHelper.Range(groupCfg.ItemMinCount[index], groupCfg.ItemMaxCount[index]);
            return new Tuple<int, int>(itemId, randomCount);
        }

        /// <summary>
        /// 根据权重获取随机索引
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        static int GetQualityValue(int[] weight)
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
    }
}