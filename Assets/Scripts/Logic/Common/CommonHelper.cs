using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.Helper;

namespace Logic.Common
{
    /// <summary>
    /// 通用逻辑
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 加权计算返回值
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static int GetWeightIndex(int[] weight)
        {
            // 总权值
            var maxWeight = weight.Sum();
            // 随机权值
            var randomWeight = RandomHelper.Range(1, maxWeight + 1);
            // 返回值
            var indexValue = 0;
            var j = weight.Length;
            for (var i = 0; i < j; i++)
            {
                indexValue += weight[i];
                if (randomWeight <= indexValue)
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
        public static List<int> GetItemDetails(int groupID)
        {
            var groupData = GroupCfg.GetData(groupID);
            // 返回ItemID
            var itemId = groupData.ItemID;
            var itemWeight = groupData.ItemWight;

            // 返回ItemMinCount
            var itemMinCount = groupData.ItemMinCount;
            // 返回ItemMaxCount
            var itemMaxCount = groupData.ItemMaxCount;
            var weightIndex = GetWeightIndex(itemWeight.ToArray());
            var list = new List<int>
            {
                itemId[weightIndex],
                itemMinCount[weightIndex],
                itemMaxCount[weightIndex]
            };
            return list;
        }
    }
}