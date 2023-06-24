using BreakInfinity;
using Framework.Pool;
using Logic.Common;
using Logic.Fight.Weapon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Actor
{
    /// <summary>
    /// 特殊远程怪，可以同时发射多种武器，临时处理方案，需要重构
    /// </summary>
    public class SpecialRangeEnemy : RangeEnemy
    {
        [SerializeField] Animator m_BPAnimtor;

        protected override void OnDead()
        {
            base.OnDead();

            m_BPAnimtor.SetTrigger(AniTrigger.ToDead);
        }

        public void OnMainGunHit()
        {
            FightManager.Ins.GetGJJTarget().m_Health.Damage(m_Attack);
        }


    }
}