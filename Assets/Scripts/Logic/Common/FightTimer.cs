using UnityEngine;

namespace Logic.Common
{
    /// <summary>
    /// 技能 / 攻击 CD
    /// </summary>
    public class FightTimer
    {
        private float CD;
        public FightTimer(float pCD)
        {
            CD = pCD;
        }
        
        /// <summary>
        /// 下次可攻击时间
        /// </summary>
        public float NextTime;

        public void Increase()
        {
            NextTime = Time.timeSinceLevelLoad + CD;
        }

        public bool Can()
        {
            return Time.timeSinceLevelLoad >= NextTime;
        }
    }
}