using System;
using Chronos;
using Framework.Helper;
using Logic.Common;
using Logic.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Actor
{
    /// <summary>
    /// 战斗内怪物基类
    /// </summary>
    [RequireComponent(typeof(ActorHealth))]
    public class ActorBase : MonoBehaviour
    {
        [ReadOnly] public int m_ActorID; //怪物的话为怪物CfgID
        [ReadOnly] public long m_InsID; //实例ID 客户端启动后就永远不会重复

        [NonSerialized] public ActorHealth m_Health;
        [NonSerialized] public Animator m_Animator;
        
        [LabelText("坐标偏移量")] public float OffsetY = 0.5f;
        
        protected Vector3 m_OffsetY;

        protected Timeline m_TimeLine;



        protected float m_AttackSpeed;
        public bool IsUnderAttackCD { get; private set; }

        protected virtual void Awake()
        {
            m_OffsetY = new Vector3(0, OffsetY, 0);
            
            m_Health = GetComponent<ActorHealth>();
            m_Health.OnDeath.AddListener(OnDead);
            
            m_Animator = GetComponentInChildren<Animator>();

            m_TimeLine = GetComponent<Timeline>();
        }

        #region 逻辑接口
        
        //自己是否已经死亡
        public virtual bool IsDead()
        {
            return m_Health.IsDead;
        }
        
        //能否被攻击
        public virtual bool CanAttack()
        {
            return !IsDead();
        }
        
        //死亡事件
        protected virtual void OnDead()
        {
            
        }

        /// <summary>
        /// 进入战斗状态时候调用
        /// </summary>
        public virtual void OnEnterAttack()
        {

        }

        /// <summary>
        /// 这个接口主要给表现逻辑使用, 特殊是参数pNeedRandom为true时!
        /// </summary>
        public Vector3 GetPos(bool pNeedRandom = true)
        {
            if (pNeedRandom)
            {
                var _Pos = RandomHelper.NextVector3(m_OffsetY.y > 0 ? m_OffsetY.y / 2f : 0.1f) + m_OffsetY;
                return transform.position + _Pos;
            }
            
            return transform.position + m_OffsetY;
        }
        
        #endregion
        
        #region 动画接口

        public void PlayIdle()
        {
            m_Animator.SetTrigger(AniTrigger.ToIdle);
        }
        
        public void PlayMove()
        {
            m_Animator.SetTrigger(AniTrigger.ToMove);
        }
        
        public virtual void PlayAttack()
        {
            m_Animator.SetTrigger(AniTrigger.ToAtk);
            IsUnderAttackCD = true;
            DotweenManager.Ins.DOTweenDelay(1 / m_AttackSpeed, 1, () =>
            {
                IsUnderAttackCD = false;
            });
        }
        
        public void PlayDead()
        {
            m_Animator.SetTrigger(AniTrigger.ToDead);
        }

        #endregion
    }
}
