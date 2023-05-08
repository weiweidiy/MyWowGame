
using System.Collections;
using Framework.Pool;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Weapon;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    /// <summary>
    /// 绞肉机 机枪攻击
    /// </summary>
    public class Skill_1 : SkillBase
    {
        public Animator m_Animator;
        public GameObject m_SkillObj;
        
        //枪口位置
        public Transform m_MuzzlePos;
        //子弹预制体
        public GameObject m_BulletPrefab;
        //跑口预制体
        public GameObject m_MuzzleEffectPrefab;
        
        private static readonly int Appear = Animator.StringToHash("Appear");
        private static readonly int DisAppear = Animator.StringToHash("DisAppear");

        private readonly WaitForSeconds m_WaitForSeconds = new WaitForSeconds(0.24f);
        public override void Init(int pSkillId)
        {
            base.Init(pSkillId);
        }
        
        private bool m_NeedSearch = true;
        public override void OnStartSkill()
        {
            base.OnStartSkill();
            m_NeedSearch = false;
            
            m_SkillObj.SetActive(true);
            m_Animator.enabled = true;
            m_Animator.SetTrigger(Appear);
        }
        
        public override void OnStopSkill()
        {
            m_NeedSearch = false;
            m_Animator.SetTrigger(DisAppear);
        }

        public override void OnSkillReset()
        {
            m_Animator.ResetTrigger(Appear);
            m_Animator.ResetTrigger(DisAppear);
            m_Animator.enabled = false;
            m_SkillObj.SetActive(false);
            
            StopAllCoroutines();
            
            m_SM.ToIdle();
        }
        
        public override bool NeedSearchTarget()
        {
            return m_NeedSearch;
        }
        
        public void OnAni_AppearEnd()
        {
            m_NeedSearch = true;
        }
        
        public void OnAni_DisAppearEnd()
        {
            m_Animator.enabled = false;
            m_SkillObj.SetActive(false);
        }
        
        protected void RotateToDir()
        {
            Vector2 direction = m_TargetPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            m_SkillObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        private Vector3 m_TargetPos;
        public override void OnFindTarget(Enemy pTarget)
        {
            m_NeedSearch = false;
            m_TargetPos = pTarget.GetPos();
            RotateToDir();
            
            var position = m_MuzzlePos.position;
            var rotation = m_MuzzlePos.rotation;
            
            var _MuzzleObj = FightObjPool.Ins.Spawn(m_MuzzleEffectPrefab);
            _MuzzleObj.transform.position = position;
            _MuzzleObj.transform.rotation = rotation;

            var _BulletObj = FightObjPool.Ins.Spawn(m_BulletPrefab).GetComponent<NormalBullet>();

            var transform1 = _BulletObj.transform;
            transform1.position = position;
            transform1.rotation = rotation;
            
            _BulletObj.Fire(pTarget, m_TargetPos, GetSkillBaseDamage() * Formula.GetGJJAtk()/100 );

            StartCoroutine(WaitNextAttack());
        }
        
        IEnumerator WaitNextAttack()
        {
            yield return m_WaitForSeconds;
            m_SM.m_ContextData.m_CurrentTarget = null;
            m_NeedSearch = true;
        }
    }
}