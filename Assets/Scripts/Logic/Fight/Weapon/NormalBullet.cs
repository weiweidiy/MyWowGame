using BreakInfinity;
using Chronos;
using Framework.Pool;
using Logic.Fight.Actor;
using Logic.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Weapon
{
    /// <summary>
    /// 普通攻击的子弹
    /// GJJ 普通攻击 等
    /// </summary>
    public class NormalBullet : MonoBehaviour, IPoolObj
    {
        [LabelText("子弹飞行速度")]
        public float m_Speed = 10;
        [LabelText("命中特效预制体")]
        public GameObject m_HitEffectPrefab;
        
        private bool m_IsFired = false;
        //private Vector3 m_Dir;
        private Vector3 m_TargetPos;
        private ActorBase m_TargetActor;
        private BigDouble m_Damage;

        Timeline m_TimeLine;
        private void Awake()
        {
            m_TimeLine = GetComponent<Timeline>();
        }

        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="pTarget">目标实体</param>
        /// <param name="pTargetPos">开枪时目标的具体坐标</param>
        /// <param name="pDamage">本次伤害</param>
        public void Fire(ActorBase pTarget, Vector3 pTargetPos, BigDouble pDamage)
        {
            //m_Dir = (pTarget.GetPos() - transform.position).normalized;
            m_TargetActor = pTarget;
            m_IsFired = true;
            m_Damage = pDamage;
            m_TargetPos = pTargetPos;
        }

        
        void Update()
        {
            if (!m_IsFired)
            {
                return;
            }

            //transform.Translate(Time.deltaTime * m_Speed * m_Dir, Space.World);
            //var _TargetPos = m_TargetActor.GetPos();
            Debug.Assert(m_TimeLine != null, "没有找到timeline" + gameObject.name);
            float deltaTime = m_TimeLine ? m_TimeLine.deltaTime : Time.deltaTime;
            deltaTime *= Time.timeScale;
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, deltaTime * m_Speed);
            
            if (Mathf.Abs(transform.position.x - m_TargetPos.x) < 0.03f &&
                Mathf.Abs(transform.position.y - m_TargetPos.y) < 0.03f)
                DoAttack();
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
            if(m_TargetActor != null && m_TargetActor.IsDead() == false)
                m_TargetActor.m_Health.Damage(m_Damage);
            
            //Destroy(gameObject);
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
