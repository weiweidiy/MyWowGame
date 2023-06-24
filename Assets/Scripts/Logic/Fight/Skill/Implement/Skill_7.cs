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
    /// 主炮攻击技能
    /// </summary>
    public class Skill_7 : SkillBase
    {
        public Animator m_Animator;
        public GameObject m_SkillObj;
        public Transform m_HitTrans;
        
        public GameObject m_SkillFireObj;
        public float m_FireInterval = 0.25f;
        public float m_FireATKRange = 2f;
        
        private static readonly int Near = Animator.StringToHash("Near");
        private static readonly int Middle = Animator.StringToHash("Middle");
        private static readonly int Far = Animator.StringToHash("Far");

        private WaitForSeconds m_FireWait;
        public override void Init(int pSkillId)
        {
            base.Init(pSkillId);
            m_FireWait = new WaitForSeconds(m_FireInterval);
            m_Animator.enabled = false;
        }

        public override void OnStartSkill()
        {
            base.OnStartSkill();
            m_NeedSearch = true;

            //如果是手动模式，需要立即搜敌人，如果没有，则放空炮
            if (!GameDataManager.Ins.AutoSkill)
            {
                m_SM.m_ContextData.m_CurrentTarget = GetSkillTarget();

                if (m_SM.m_ContextData.m_CurrentTarget == null)
                {
                    m_SkillObj.transform.SetParent(FightManager.Ins.m_GroundNode);
                    m_SkillObj.Show();
                    m_Animator.enabled = true;
                    m_Animator.SetTrigger(Middle);
                    FightManager.Ins.m_CurGJJ.PlayMainGunAni();
                    m_NeedSearch = false;
                }
                else
                {
                    OnFindTarget(m_SM.m_ContextData.m_CurrentTarget);
                }
            }
        }

        private EnemyRange m_Range;
        private bool m_NeedSearch = true;
        public override void OnFindTarget(Enemy pTarget)
        {
            m_NeedSearch = false;
            
            m_SkillObj.transform.SetParent(FightManager.Ins.m_GroundNode);
            m_SkillObj.Show();
            m_Animator.enabled = true;
            
            if (pTarget.transform.position.x < FightEnemyManager.Ins.NearPosX)
            {
                m_Range = EnemyRange.Near;
                m_Animator.SetTrigger(Near);
            }
            else if (pTarget.transform.position.x < FightEnemyManager.Ins.MiddlePosX)
            {
                m_Range = EnemyRange.Middle;
                m_Animator.SetTrigger(Middle);
            }
            else
            {
                m_Range = EnemyRange.Far;
                m_Animator.SetTrigger(Far);
            }
            
            FightManager.Ins.m_CurGJJ.PlayMainGunAni();
        }
        
        public override void OnStopSkill()
        {
            m_NeedSearch = true;
            m_Animator.enabled = false;
            StopAllCoroutines();
            
            m_SkillFireObj.Hide();
            m_SkillFireObj.transform.SetParent(transform);
            m_SkillFireObj.transform.localPosition = Vector3.zero;
        }


        public override Enemy GetSkillTarget()
        {
            return FightEnemyManager.Ins.GetOneTarget(posType);
        }

        public override void OnSkillReset()
        {
            StopAllCoroutines();
           
            m_Animator.enabled = false;
            
            m_SkillObj.Hide();
            m_SkillObj.transform.SetParent(transform);
            m_SkillObj.transform.localPosition = Vector3.zero;
            
            m_SkillFireObj.Hide();
            m_SkillFireObj.transform.SetParent(transform);
            m_SkillFireObj.transform.localPosition = Vector3.zero;
            
            m_SM.ToIdle();
        }

        public override bool NeedSearchTarget()
        {
            return m_NeedSearch;
        }

        private List<Enemy> m_HitList = new List<Enemy>(6);
        public void OnAni_Attack()
        {
            m_SkillFireObj.transform.SetParent(FightManager.Ins.m_GroundNode);
            m_SkillFireObj.Show();
            m_SkillFireObj.transform.position = m_HitTrans.position;
            StartCoroutine(ShowFire());
        }
        
        public void OnAni_AttackEnd()
        {
            m_Animator.enabled = false;
            m_SkillObj.Hide();
            m_SkillObj.transform.SetParent(transform);
            m_SkillObj.transform.localPosition = Vector3.zero;
        }
        
        private IEnumerator ShowFire()
        {
            while (true)
            {
                var _PosX = m_SkillFireObj.transform.position.x;
                FightEnemyManager.Ins.GetTargetByRange(m_HitList, _PosX - m_FireATKRange, _PosX + m_FireATKRange,Enemy.PositionType.Ground);
                BigDouble _ATK = Formula.GetGJJAtk() * GetSkillBaseDamage()/100;
                foreach (var enemy in m_HitList)
                {
                    if(enemy != null && enemy.CanAttack())
                        enemy.m_Health.Damage(_ATK);
                }

                yield return m_FireWait;
            }
        } 
    }
}