using Framework.GameFSM;
using Logic.Fight.Common;
using System;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    /// <summary>
    /// 攻击者状态机，攻击者可能是技能创建出来的，也可以是其他创建出来的可复用对象
    /// </summary>
    public class ATKERSM : FSMachine<IState<AttackerState, AttackerStateData>, AttackerState, AttackerStateData>
    {

        public void ToReady()
        {
            NextState(m_ContextData.m_Ready);
        }

        public void ToIdle()
        {
            NextState(m_ContextData.m_Idle);
        }

        public void ToSearch()
        {
            NextState(m_ContextData.m_Searching);
        }

        public void ToEmit()
        {
            NextState(m_ContextData.m_Emitting);
        }

        public void ToEnd()
        {
            NextState(m_ContextData.m_Ending);
        }
    }

    public enum AttackerState
    {
        //准备攻击状态
        Ready,
        //待机状态
        Idle,
        //搜敌状态
        Search,
        //发射状态
        Emit,
        //结束状态
        End,
    }

    /// <summary>
    /// 状态机上下文
    /// </summary>
    public class AttackerStateData
    {
        public ATKERSM m_SM;
        public IAttacker m_Attacker;
        public IDamagable m_CurrentTarget;

        //状态
        public readonly ATKERSS_Ready m_Ready = new(AttackerState.Ready);
        public readonly ATKERSS_IDLE m_Idle = new(AttackerState.Idle);
        public readonly ATKERSS_Searching m_Searching = new(AttackerState.Search);
        public readonly ATKERSS_Emitting m_Emitting = new(AttackerState.Emit);
        public readonly ATKERSS_Ending m_Ending = new(AttackerState.End);
    }
}