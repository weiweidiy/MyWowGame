using BreakInfinity;
using Logic.Common;

namespace Logic.Fight.Data
{
    /// <summary>
    /// 攻击产生的伤害数据
    /// </summary>
    public struct FightDamageData
    {
        public FightDamageData(BigDouble pDamage, bool pIsCritical = false)
        {
            Damage = pDamage;
            IsCritical = pIsCritical;
        }

        public BigDouble Damage;
        public bool IsCritical;
    }
}