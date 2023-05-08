using System;
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
    /// 战斗前准备 移动 刷怪 等
    /// </summary>
    public class FS_Standby : IState<FightState, FightStateData>
    {
        private FightStateData m_StateData;
        public FS_Standby(FightState pType) : base(pType)
        {
        }
        
        public override void Enter(FightStateData pData)
        {
            Debug.LogWarning("FS - FS_Standby ENTER");
            
            m_StateData = pData;
            //Dump();
            
            m_EventGroup.Register(LogicEvent.Fight_Start, OnFightStart);
            m_EventGroup.Register(LogicEvent.Fight_Switch, OnFightSwitch);
            // 副本定时器
            m_EventGroup.Register(LogicEvent.Fight_CopyTimeUp, OnCopyTimeUp);
            
            //通知播放战斗前准备动画
            EventManager.Call(LogicEvent.Fight_Standby);
            EventManager.Call(LogicEvent.Fight_LevelTypeChanged, m_StateData.m_LevelType);
            
            //战斗状态改变 通知UI (正常/挂机)
            EventManager.Call(LogicEvent.Fight_LevelStateChanged);
        }

        /// <summary>
        /// 战前动画结束 进入战斗
        /// </summary>
        private void OnFightStart(int arg1, object arg2)
        {
            switch (m_StateData.m_LevelType)
            {
                case LevelType.NormalLevel:
                {
                    if (GameDataManager.Ins.LevelState == LevelState.HandUp)
                        m_StateData.m_SM.ToHandUp();
                    else
                        m_StateData.m_SM.ToFighting();
                }
                    break;
                case LevelType.DiamondCopy:
                {
                    m_StateData.m_SM.ToFighting();
                    EventManager.Call(LogicEvent.Fight_CopyDiamondCountChanged);
                }
                    break;
                case LevelType.CoinCopy:
                    m_StateData.m_SM.ToFighting();
                    break;
                case LevelType.EngineCopy:
                    m_StateData.m_SM.ToFighting();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnFightSwitch(int arg1, object arg2)
        {
            var _Para = (FightSwitchTo)arg2;
            _Para.m_CanSwitchToNextNode = true;
            
            //清理战场 通知切换到待机状态
            EventManager.Call(LogicEvent.Fight_Over);
            m_StateData.m_SM.ToSwitch(_Para.m_SwitchToType);
        }
        
        private void OnCopyTimeUp(int i, object o)
        {
            m_StateData.m_IsWin = false;
            m_StateData.m_SM.ToFightOver();
        }
        
        private void Dump()
        {
            Debug.LogError("----------------------------");
            Debug.LogError($"当前关卡类型: {m_StateData.m_LevelType}");
            Debug.LogError($"当前关卡状态: {GameDataManager.Ins.LevelState}");
            Debug.LogError($"当前关卡ID : {GameDataManager.Ins.CurLevelID}");
            Debug.LogError($"当前关卡节点: {GameDataManager.Ins.CurLevelNode}");
            Debug.LogError($"当前是否BOSS: {FightManager.Ins.IsBossLevel()}");
            Debug.LogError("----------------------------");
        }
    }
}