using BreakInfinity;
using Framework.Extension;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Fight.Skill.Implement.SkillParts
{
    public class Skill_3Missiles : MonoBehaviour
    {
        public Skill_3 m_Owner;
        [LabelText("攻击范围")]
        public EnemyRange pRange;
        [LabelText("序列号")]
        public int pIndex;

        public Animator m_Hit;
        public Transform m_HitPos;

        //public Skill_3MissilesHit m_HitEffect;

        Animator m_MissleAnimator;

        GameObject m_goHitEffect;

        private void Awake()
        {
            m_MissleAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            var animHelper = new AnimatorHelper();
            StartCoroutine(animHelper.CheckAnimationComplete(m_MissleAnimator, "Skill_3_Down", () =>
            {
                OnAni_Attack(pRange);

                m_goHitEffect =Instantiate(m_Hit.gameObject, transform);
                m_goHitEffect.transform.position = transform.position + new Vector3(0,-1f,0);
                m_goHitEffect.Show();
                var component = m_goHitEffect.GetComponent<Skill_3MissilesHit>();
                component.PlayAni(m_goHitEffect.transform.position);
                ////检查爆炸动画
                //var _hitAnimator = _goHit.GetComponent<Animator>();
                //StartCoroutine(animHelper.CheckAnimationComplete(_hitAnimator, "Skill_3_Down_Hit", () =>
                //{
                //    _goHit.Destroy();
                //    //Destroy(_goHit);
                //}));

            }));
        }

        private void OnDisable()
        {
            Destroy(m_goHitEffect);
        }

        public void OnAni_Attack(EnemyRange pRange)
        {
            var m_HitList = new List<Enemy>();
            switch (pRange)
            {
                case EnemyRange.Near:
                    FightEnemyManager.Ins.GetTargetByDistance(m_HitList, EnemyRange.Near);
                    break;
                case EnemyRange.Middle:
                    FightEnemyManager.Ins.GetTargetByDistance(m_HitList, EnemyRange.Middle);
                    break;
                case EnemyRange.Far:
                    FightEnemyManager.Ins.GetTargetByDistance(m_HitList, EnemyRange.Far);
                    break;
            }

            BigDouble _ATK = Formula.GetGJJAtk() * m_Owner.GetSkillBaseDamage() / 100;

            foreach (var enemy in m_HitList)
            {
                if (enemy != null && enemy.CanAttack())
                    enemy.m_Health.Damage(_ATK);
            }


        }

        ///// <summary>
        ///// 下落动画的帧事件
        ///// </summary>
        //public void OnMissileATK()
        //{
        //    m_HitEffect.Show();
        //    m_HitEffect.PlayAni(m_HitPos.position);

        //    //var animHelper = new AnimatorHelper();
        //    //StartCoroutine(animHelper.CheckAnimationComplete(m_HitEffect.m_Animator, "Skill_3_Down", () =>
        //    //{
        //    //    m_HitEffect.Hide();
        //    //    m_HitEffect.m_Animator.enabled = false;
        //    //}));

        //    m_Owner.OnAni_Attack(pRange);
        //}
    }
}
