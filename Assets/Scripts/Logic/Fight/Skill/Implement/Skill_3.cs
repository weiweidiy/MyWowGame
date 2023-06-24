using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    /// <summary>
    /// 对地导弹攻击技能
    /// </summary>
    public class Skill_3 : SkillBase
    {
        public Animator m_Animator;
        public Animator m_AnimatorBack;
        public GameObject m_SkillObj;
        public GameObject m_SkillObjBack;
        public GameObject[] m_Missiles;
        public Animator[] m_MissileAnimators;

        public int m_missileCount;
        
        private static readonly int Launch = Animator.StringToHash("Launch");

        Coroutine coStart;

        public override void Init(int pSkillId)
        {
            base.Init(pSkillId);
            m_Animator.enabled = false;
            m_AnimatorBack.enabled = false;
        }
        
        public override void OnStartSkill()
        {
            base.OnStartSkill();
            
            m_missileCount = Mathf.Max(6, m_missileCount);
            m_missileCount = Mathf.Min(m_missileCount, m_MissileAnimators.Length);

            coStart = StartCoroutine(DoStartSkill());
        }

        IEnumerator DoStartSkill()
        {
            m_SkillObj.SetActive(true);
            m_Animator.enabled = true;
            m_Animator.SetTrigger(Launch);
            yield return new WaitForSeconds(0.25f);
            m_SkillObjBack.SetActive(true);
            m_AnimatorBack.enabled = true;
            m_AnimatorBack.SetTrigger(Launch);
        }

        public override void OnStopSkill()
        {
            m_Animator.enabled = false;
            m_AnimatorBack.enabled = false;
            
            m_SkillObj.SetActive(false);
            m_SkillObjBack.SetActive(false);

            for (int i = 0; i < m_missileCount; i++)
            {
                m_Missiles[i].Hide();
                m_MissileAnimators[i].enabled = false;
            }
        }

        public override void OnSkillReset()
        {
            StopAllCoroutines();

            OnStopSkill();

            m_SM.ToIdle();
        }

        private readonly WaitForSeconds WaitSecond = new(0.25f);
        public IEnumerator OnAni_StepOneEnd()
        {    
            for (int i = 0; i < m_missileCount; i++)
            {
                m_Missiles[i].Show();
                m_MissileAnimators[i].enabled = true;
                m_MissileAnimators[i].SetTrigger(Launch);
                yield return WaitSecond;
            }
        }

        public override Enemy GetSkillTarget()
        {
            return FightEnemyManager.Ins.GetOneTarget(posType);
        }

        //private List<Enemy> m_HitList = new List<Enemy>(6);

        ////导弹攻击落地
        //public void OnAni_Attack(EnemyRange pRange)
        //{
        //    switch (pRange)
        //    {
        //        case EnemyRange.Near:
        //            FightEnemyManager.Ins.GetTargetByDistance(m_HitList, EnemyRange.Near);
        //            break;
        //        case EnemyRange.Middle:
        //            FightEnemyManager.Ins.GetTargetByDistance(m_HitList, EnemyRange.Middle);
        //            break;
        //        case EnemyRange.Far:
        //            FightEnemyManager.Ins.GetTargetByDistance(m_HitList, EnemyRange.Far);
        //            break;
        //    }

        //    BigDouble _ATK = Formula.GetGJJAtk() * GetSkillBaseDamage()/100 ;

        //    foreach (var enemy in m_HitList)
        //    {
        //        if(enemy != null && enemy.CanAttack())
        //            enemy.m_Health.Damage(_ATK);
        //    }


        //}

        ////导弹攻击结束
        //public void OnAni_AttackEnd(int pIndex)
        //{
        //    m_Missiles[pIndex].Hide();
        //    m_MissileAnimators[pIndex].enabled = false;
        //}
    }
}
