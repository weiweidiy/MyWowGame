using BreakInfinity;
using Framework.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Weapon
{
    /// <summary>
    /// 普通怪物子弹
    /// </summary>
    public class EnemyBullet : MonoBehaviour, IPoolObj
    {
        [LabelText("子弹飞行速度")]
        public float m_Speed = 25;
        [LabelText("命中特效预制体")]
        public GameObject m_HitEffectPrefab;
        
        private bool m_IsFired = false;
        private BigDouble m_Damage;
        private Transform m_TargetPos;

        public void Fire(Transform pTargetPos, BigDouble pDamage)
        {
            m_TargetPos = pTargetPos;
            m_IsFired = true;
            m_Damage = pDamage;
            
            // if(transform.position.x > m_TargetPos.x)
            //     this.ToLeft();
        }
        
        void Update()
        {
            if (!m_IsFired)
            {
                return;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPos.position, Time.deltaTime * m_Speed);

            if (Mathf.Abs(transform.position.x - m_TargetPos.position.x) < 0.03f)
            {
                DoAttack();
            }
        }
        
        private void DoAttack()
        {
            if (m_HitEffectPrefab != null)
            {
                var _HitObj = FightObjPool.Ins.Spawn(m_HitEffectPrefab);
                var _Transform = transform;
                _HitObj.transform.position = _Transform.position;
            }

            m_IsFired = false;
            FightManager.Ins.GetGJJTarget().m_Health.Damage(m_Damage, false, m_TargetPos);
            FightObjPool.Ins.Recycle(gameObject);
        }
        
        public void OnSpawn()
        {
            
        }

        public void OnRecycle()
        {
            m_IsFired = false;
        }
    }
}