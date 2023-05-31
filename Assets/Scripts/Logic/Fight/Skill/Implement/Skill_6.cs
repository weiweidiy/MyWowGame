
using System.Collections;
using Framework.Pool;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Weapon;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    /// <summary>
    /// 激光子弹攻击
    /// </summary>
    public class Skill_6 : SkillBase
    {
        [Tooltip("释放的个数")]
        public int m_subSkillObjectCount;
        [Tooltip("技能子物体发出间隔")]
        public float m_spawnInterval;


        public Attacker m_attacter;

        public override void Init(int pSkillId)
        {
            base.Init(pSkillId);

        }
        
        public override void OnStartSkill()
        {
            base.OnStartSkill();
            //m_skillSubObjectTemplate.SetActive(true);
            m_attacter.Ready(m_subSkillObjectCount, m_spawnInterval, GetSkillBaseDamage() * Formula.GetGJJAtk() / 100);

        }
        
        public override void OnStopSkill()
        {
            m_attacter.End();
        }

        public override void OnSkillReset()
        {
            m_SM.ToIdle();
            OnStopSkill();
        }
        
        //public override bool NeedSearchTarget()
        //{
        //    return m_NeedSearch;
        //}
        
        //public void OnAni_AppearEnd()
        //{
        //    m_NeedSearch = true;
        //}
        
        //private Vector3 m_TargetPos;
        //private Enemy m_Target;
        //public override void OnFindTarget(Enemy pTarget)
        //{
        //    m_NeedSearch = false;
        //    m_TargetPos = pTarget.GetPos();
        //    m_Target = pTarget;

        //    m_IsDoAttack = true;
        //    m_Animator.SetTrigger(Attack);
        //}
        
        //public void OnAni_Attack()
        //{
        //    var position = m_MuzzlePos.position;
        //    var _BulletObj = FightObjPool.Ins.Spawn(m_BulletPrefab).GetComponent<NormalBullet>();
        //    var transform1 = _BulletObj.transform;
            
        //    transform1.position = position;
            
        //    Vector2 direction = m_TargetPos - transform1.position;
        //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //    transform1.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
        //    _BulletObj.Fire(m_Target, m_TargetPos, GetSkillBaseDamage() * Formula.GetGJJAtk()/100 );
        //}
        
        //public void OnAni_AttackEnd()
        //{
        //    curCount++;
        //    if(curCount < m_subSkillObjectCount)
        //    {
        //        m_coroutine = StartCoroutine(WaitFire());     
        //    }
        //    else
        //    {
        //        m_Animator.enabled = false;
        //        m_skillSubObjectTemplate.SetActive(false);
        //    }

        //}

        //IEnumerator WaitFire()
        //{
        //    yield return new WaitForSeconds(m_spawnInterval);
        //    //m_Animator.SetTrigger(Attack);
        //    m_attacter.Attack();
        //}
    }
}