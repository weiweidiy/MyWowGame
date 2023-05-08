using UnityEngine;

namespace Logic.Fight.Actor
{
    /// <summary>
    /// 近战怪物实现
    /// </summary>
    public class MeleeEnemy : Enemy
    {
        //近战直接对GJJ造成伤害
        public override void OnAni_Attack()
        {
            var _GJJ = GetTargetGJJ();
            if(_GJJ == null || _GJJ.IsDead())
                return;
            _GJJ.m_Health.Damage(m_Attack * 6);
        }
    }
}