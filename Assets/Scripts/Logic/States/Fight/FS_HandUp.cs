
using Framework.EventKit;
using Framework.GameFSM;
using Logic.Common;
using Logic.Data;
using Logic.Fight;
using Logic.Fight.Common;
using UnityEngine;

namespace Logic.States.Fight
{
    /// <summary>
    /// 挂机状态 - 普通关卡死亡后的状态
    /// </summary>
    public class FS_HandUp : IState<FightState, FightStateData>
    {
        private FightStateData m_StateData;
        public FS_HandUp(FightState pType) : base(pType)
        {
        }
        
        public override void Enter(FightStateData pContext)
        {
            Debug.LogWarning("FS - FS_HandUp ENTER");
            
            m_StateData = pContext;
            
            // 战斗中 当前战斗被终止事件
            m_EventGroup.Register(LogicEvent.Fight_Switch, OnFightSwitch);
            // 放置刷挂
            FightEnemyManager.Ins.StartSpawnHandUp();
            
            EventManager.Call(LogicEvent.Fight_Fighting);
        }

        public override void Update(FightStateData pContext)
        {
            //GJJ死亡 战斗失败
            if (FightManager.Ins.m_CurGJJ.IsDead())
            {
                pContext.m_IsWin = false;
                pContext.m_SM.ToFightOver();
                return;
            }
            
            //怪物死亡 战斗成功
            if (FightEnemyManager.Ins.IsClear())
            {
                pContext.m_IsWin = true;
                pContext.m_SM.ToFightOver();
                return;
            }
        }

        public override void Release(FightStateData pContext)
        {
            base.Release(pContext);
        }

        private void OnFightSwitch(int arg1, object arg2)
        {
            var _Para = (FightSwitchTo)arg2;
            _Para.m_CanSwitchToNextNode = true;
            
            //清理战场 通知切换到待机状态
            EventManager.Call(LogicEvent.Fight_Over);
            m_StateData.m_SM.ToSwitch(_Para.m_SwitchToType);
        }
    }
}