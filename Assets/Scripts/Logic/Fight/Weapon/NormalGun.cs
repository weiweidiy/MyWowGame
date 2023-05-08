using System;
using Framework.Pool;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityTimer;

namespace Logic.Fight.Weapon
{
    /// <summary>
    /// 普通攻击 臂炮
    /// </summary>
    public class NormalGun : MonoBehaviour
    {
        [LabelText("枪口位置")]
        public Transform m_MuzzlePos;
        [LabelText("子弹预制体")]
        public GameObject m_BulletPrefab;
        [LabelText("枪口特效预制体")]
        public GameObject m_MuzzleEffectPrefab;
        
        private Animator m_Animator;
        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private bool InStandBy = true;
        public void ToStandby()
        {
            InStandBy = true;
            m_CurrentTarget = null;
            m_Animator.SetTrigger(AniTrigger.ToIdle);
        }

        private Enemy m_CurrentTarget;
        private Vector3 m_TargetPos;
        public void ToAttack(Enemy pTarget ,Vector3 pTargetPos)
        {
            InStandBy = false;
            m_CurrentTarget = pTarget;
            m_TargetPos = pTargetPos;
            
            m_Animator.SetTrigger(AniTrigger.ToAtk);
        }
        
        private bool IsInvalidTarget()
        {
            if (m_CurrentTarget != null && !m_CurrentTarget.IsDead())
                return false;
            return true;
        }

        private void Update()
        {
            if (InStandBy)
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
                return;
            }
            if (!IsInvalidTarget())
            {
                Vector2 direction = m_TargetPos - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
            }
            
        }

        protected void RotateToDir()
        {
            // float angle = Vector3.Angle(transform.position, dir);
            // Debug.LogError(angle);
            // transform.rotation = Quaternion.Euler(0, 0, angle);
            //
            Vector2 direction = m_TargetPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        #region 动画

        //攻击动画开始 回调
        public virtual void OnAni_AttackStart()
        {

        }
        //攻击动画触发 回调
        public virtual void OnAni_Attack()
        {
            RotateToDir();
            
            var _MuzzleObj = FightObjPool.Ins.Spawn(m_MuzzleEffectPrefab);
            var position = m_MuzzlePos.position;
            var rotation = m_MuzzlePos.rotation;
            
            _MuzzleObj.transform.position = position;
            _MuzzleObj.transform.rotation = rotation;

            var _BulletObj = FightObjPool.Ins.Spawn(m_BulletPrefab).GetComponent<NormalBullet>();

            var transform1 = _BulletObj.transform;
            transform1.position = position;
            transform1.rotation = rotation;

            _BulletObj.Fire(m_CurrentTarget, m_TargetPos, Formula.GetGJJAtk());
        }
        //攻击动画结束 回调
        public virtual  void OnAni_AttackEnd()
        {
            
        }

        #endregion
    }
}
