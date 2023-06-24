using Framework.EventKit;
using Framework.GameFSM;
using Logic.Common;
using System;

namespace Logic.States.Fight
{
    public class FS_Idle : IState<FightState, FightStateData>
    {
        private FightStateData m_StateData;

        public FS_Idle(FightState pType) : base(pType)
        {
        }

        public override void Enter(FightStateData pContext)
        {
            base.Enter(pContext);

            m_StateData = pContext;
            m_EventGroup.Register(LogicEvent.Fight_Switch, OnFightSwitch);
        }

        private void OnFightSwitch(int arg1, object arg2)
        {
            var _Para = (FightSwitchTo)arg2;
            _Para.m_CanSwitchToNextNode = true;

            //清理战场 通知切换到待机状态
            EventManager.Call(LogicEvent.Fight_Over);
            m_StateData.m_SM.ToSwitch(_Para.m_SwitchToType);
        }

        public override void Release(FightStateData pContext)
        {
            base.Release(pContext);
        }
    }
}