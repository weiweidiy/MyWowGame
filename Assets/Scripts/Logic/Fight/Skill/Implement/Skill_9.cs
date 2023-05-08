
using System.Collections.Generic;
using BreakInfinity;
using Framework.Extension;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    public class Skill_9 : SkillBase
    {
        public Animator m_Animator;
        public GameObject m_SkillObj;
        
        public GameObject m_Missiles;
        public Animator m_MissileAnimators;
        
        private static readonly int Launch = Animator.StringToHash("Launch");
        private Vector3 m_MissileOriPos;
        public override void Init(int pSkillId)
        {
            base.Init(pSkillId);
            m_MissileOriPos = m_Missiles.transform.position;
            m_Animator.enabled = false;
        }
        
        public override void OnStartSkill()
        {
            base.OnStartSkill();
            
            m_SkillObj.Show();
            m_Animator.enabled = true;
            m_Animator.SetTrigger(Launch);
        }
        
        public override void OnStopSkill()
        {
        }
        
        public override bool NeedSearchTarget()
        {
            return false;
        }

        public override void OnSkillReset()
        {
            m_Animator.ResetTrigger(Launch);
            m_MissileAnimators.ResetTrigger(Launch);
            m_Animator.enabled = false;
            m_SkillObj.Hide();
            m_Missiles.Hide();
            m_Missiles.transform.SetParent(transform);
            m_MissileAnimators.enabled = false;
            
            m_SM.ToIdle();
        }
        
        public void OnAniCallMissile()
        {
            m_Missiles.Show();
            m_Missiles.transform.SetParent(FightManager.Ins.m_GroundNode);
            m_MissileAnimators.enabled = true;
            m_MissileAnimators.SetTrigger(Launch);
        }
        
        public void OnAniEnd()
        {
            m_Animator.ResetTrigger(Launch);
            m_Animator.enabled = false;
            m_SkillObj.Hide();
        }

        private List<Enemy> m_HitList = new List<Enemy>(6);
        public void OnDoATK()
        {
            FightEnemyManager.Ins.GetTargetByRange(m_HitList, -10, 10);
            BigDouble _ATK = Formula.GetGJJAtk() * GetSkillBaseDamage()/100;
            foreach (var enemy in m_HitList)
            {
                if(enemy != null && enemy.CanAttack())
                    enemy.m_Health.Damage(_ATK);
            }
        }
        
        //导弹攻击结束
        public void OnAni_AttackEnd()
        {
            m_Missiles.Hide();
            m_MissileAnimators.ResetTrigger(Launch);
            m_Missiles.transform.SetParent(transform);
            m_Missiles.transform.position = m_MissileOriPos;
            m_MissileAnimators.enabled = false;
        }
    }
}
