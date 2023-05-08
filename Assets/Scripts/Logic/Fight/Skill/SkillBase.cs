
using System;
using BreakInfinity;
using Configs;
using Framework.EventKit;
using Logic.Common;
using Logic.Data;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Skill.State;
using Logic.Manager;
using Networks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Skill
{
    /// <summary>
    /// 战斗技能实例基类
    /// </summary>
    public class SkillBase : MonoBehaviour
    {
        [LabelText("技能ID")]
        public int m_SkillId;
        [LabelText("自动阈值"), Tooltip("自动释放技能时 怪物与玩家的距离小于等于这个值时会自动释放技能")]
        public float m_AutoSkillThreshold = 5f;
        
        //技能数据
        [NonSerialized]
        public SkillData m_SkillData;
        [NonSerialized]
        public GameSkillData m_GameSkillData;
        
        // //状态
        // private bool m_IsDoing = false;
        // private bool m_IsDisable = false;
        // private float m_CurTime = 0;

        protected readonly EventGroup m_EventGroup = new();
        private void Awake()
        {
            Init(m_SkillId);
            m_EventGroup.Register(LogicEvent.SkillReset, OnSkillReset); //战斗切换会重置技能
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        // private void OnDisable()
        // {
        //     m_SM.m_ContextData.m_CurrentTarget = null;
        //     m_SM.ToIdle();
        // }

        protected SkillSM m_SM;
        public virtual void Init(int pSkillId)
        {
            m_SkillId = pSkillId;
            m_SkillData = SkillCfg.GetData(pSkillId);
            m_GameSkillData = SkillManager.Ins.GetSkillData(pSkillId);

            m_SM = new SkillSM();
            var _StateData = new SkillStateData
            {
                m_SM = m_SM,
                m_SkillBase = this,
            };
            m_SM.Start(_StateData, _StateData.m_Idle);
        }
        
        /// <summary>
        /// 技能搜寻目标
        /// </summary>
        public virtual Enemy GetSkillTarget()
        {
            return FightEnemyManager.Ins.GetOneTarget();
        }

        /// <summary>
        /// 释放技能 / 通常是手动会从这里调用
        /// </summary>
        public void CastSkill()
        {
            m_SM.ToDoing();
        }

        protected virtual void Update()
        {
            m_SM.Update();
        }

        /// <summary>
        /// 技能启动
        /// </summary>
        public virtual void OnStartSkill()
        {
            if(GameDataManager.Ins.AutoSkill)
                EventManager.Call(LogicEvent.SkillAutoPlay, m_SkillId);
        }
        
        /// <summary>
        /// 技能持续时间结束, 技能重置的时候 这个方法不会被主动调用
        /// </summary>
        public virtual void OnStopSkill()
        {
            
        }
        
        /// <summary>
        /// 切换战斗得时候 需要重置技能
        /// </summary>
        void OnSkillReset(int arg1, object arg2)
        {
            OnSkillReset();
        }
        
        public virtual void OnSkillReset()
        {
            
        }
        
        /// <summary>
        /// 游戏中 技能上阵后
        /// </summary>
        public virtual void OnSkillOn()
        {
            //默认先走一次CD
            m_SM.ToDisable();
        }
        
        /// <summary>
        /// Doing 状态机是否需要搜敌
        /// </summary>
        public virtual bool NeedSearchTarget()
        {
            return false;
        }
        
        /// <summary>
        /// 持续过程中锁定目标
        /// </summary>
        /// <param name="pTarget">目标</param>
        public virtual void OnFindTarget(Enemy pTarget)
        {
            
        }

        public virtual bool CanOperation()
        {
            if (m_SM == null)
                return true;
            if(m_SM.GetState().m_Type == SkillState.Doing)
                return false;
            return true;
        }
        
        public BigDouble GetSkillBaseDamage()
        {
            return ((m_SkillData.DamageBase + (m_GameSkillData.m_Level - 1) * m_SkillData.DamageGrow )  )  ;
        }
    }
}
