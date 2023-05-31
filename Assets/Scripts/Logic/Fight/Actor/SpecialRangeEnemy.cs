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

        [LabelText("BP枪口位置")]
        public Transform m_BPMuzzlePos;
        [LabelText("BP子弹预制体")]
        public GameObject m_BPBulletPrefab;
        [LabelText("BP枪口特效预制体")]
        public GameObject m_BPMuzzleEffectPrefab;


        

        public override void PlayAttack()
        {
            base.PlayAttack();

            m_BPAnimtor.SetTrigger(AniTrigger.ToAtk);

        }



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