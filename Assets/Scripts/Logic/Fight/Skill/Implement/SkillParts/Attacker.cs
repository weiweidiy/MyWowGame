using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Skill.State;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{

    public class Attacker : MonoBehaviour, IAttacker
    {
        public event Action<IAttacker> onReady;
        public event Action<IAttacker> onIdle;
        public event Action<IAttacker> onSearching;
        public event Action<IAttacker,IDamagable> onEmitting;
        public event Action<IAttacker> onEnding;
        

        protected ATKERSM m_SM;

        AttackerStateData m_StateData;
        private void Awake()
        {
            m_SM = new ATKERSM();
            m_StateData = new AttackerStateData
            {
                m_SM = m_SM,
                m_Attacker = this,
            };          
        }

        protected virtual void Update()
        {
            m_SM.Update();
        }

        public void Release()
        {
            m_SM.Release();
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnInitializeParams(params object[] args) { }


        /// <summary>
        /// Attacker 默认自己不找目标，子类可以重写实现attacker自己找目标
        /// </summary>
        /// <returns></returns>
        public virtual IDamagable GetTarget()
        {
            return FightEnemyManager.Ins.GetOneTarget();
        }



        #region 公共接口

        /// <summary>
        /// 进入准备状态
        /// </summary>
        /// <param name="args"></param>
        public virtual void Ready(params object[] args)
        {
            OnInitializeParams(args);

            m_SM.Release();
            m_SM.Start(m_StateData, m_StateData.m_Ready);
        }

        /// <summary>
        /// 进入待机状态
        /// </summary>
        public virtual void Idle()
        {
            m_SM.ToIdle();
        }

        /// <summary>
        /// 搜索敌人
        /// </summary>
        public virtual void Search()
        {
            m_SM.ToSearch();
        }

        /// <summary>
        /// 进行攻击，如果没有目标，会先进行搜索敌人
        /// </summary>
        public virtual void Emit()
        {
            //找目标，找到自动打
            if (m_SM.m_ContextData.m_CurrentTarget == null || m_SM.m_ContextData.m_CurrentTarget.IsDead())
                m_SM.ToSearch();
            else
                m_SM.ToEmit();
        }

        /// <summary>
        /// 进入结束阶段
        /// </summary>
        public virtual void End()
        {
            if (m_SM == null || m_SM.GetState() == null)
                return;

            if (m_SM.GetState().m_Type != AttackerState.End)
            {
                m_SM.ToEnd();
            }
            
        }

        /// <summary>
        /// 设置了一个攻击目标，进入攻击状态
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(IDamagable target)
        {
            if (m_SM.GetState().m_Type != AttackerState.Search)
            {
                Debug.LogError("当前状态不能设置攻击目标");
            }
            else
            {
                m_SM.m_ContextData.m_CurrentTarget = target;
                m_SM.ToEmit();
            }

        }
        #endregion


        #region 状态响应接口
        public void OnReady() {
            onReady?.Invoke(this);
            DoReady();
        }

        public void OnIdle()
        {
            onIdle?.Invoke(this);
            DoIdle();
        }

        public void OnSearching()
        {
            onSearching?.Invoke(this);
            DoSearching();
        }
        public void OnEmitting(IDamagable m_CurrentTarget)
        {
            onEmitting?.Invoke(this,m_CurrentTarget);
            DoEmitting(m_CurrentTarget);
        }

        public void OnEnding() {
            onEnding?.Invoke(this);
            DoEnding();
        }

        /// <summary>
        /// 实际子类要执行的准备阶段逻辑
        /// </summary>
        protected virtual void DoReady() { m_SM.ToSearch(); }
        protected virtual void DoIdle() { }
        protected virtual void DoSearching() { }
        protected virtual void DoEmitting(IDamagable m_CurrentTarget) { }
        protected virtual void DoEnding() { }



        #endregion


    }
}