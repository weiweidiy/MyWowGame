using System.Collections.Generic;
using System.Linq;
using Framework.Helper;

namespace Logic.Common
{
    /// <summary>
    /// 通用逻辑
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 根据组权重获取组Id
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static int GetWeightIndex(List<int> weight)
        {
            // 总权值
            var maxWeight = weight.Sum();
            // 随机权值
            var randomWeight = RandomHelper.Range(1, maxWeight + 1);
            // 权值区间
            var weightValue = 0;
            var j = weight.Count;

            for (var i = 0; i < j; i++)
            {
                weightValue += weight[i];
                if (randomWeight <= weightValue)
                {
                    return i;
                }
            }

            return 0;
        }

        /// <summary>
        /// 根据组权重获取组Id
        /// </summary>
        /// <param name="group"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static int GetIdFromWeight(List<int> group, List<int> weight)
        {
            return group[GetWeightIndex(weight)];
        }
    }
}