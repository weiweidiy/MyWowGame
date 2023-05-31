using System;
using Cysharp.Threading.Tasks;
using Framework.EventKit;
using Framework.GameFSM;
using Logic.Common;
using Logic.Data;
using Logic.Fight;
using Logic.Fight.Common;
using Logic.UI.UIFight;
using Main;
using UnityEngine;

namespace Logic.States.Fight
{
    /// <summary>
    /// 显示特殊战斗UI / 清理当前战场 / 恢复血量 / 恢复技能CD
    /// 进入特殊战斗状态(BOSS / 特殊关卡)
    /// </summary>
    public class FS_Switch : IState<FightState, FightStateData>
    {
        public FS_Switch(FightState pType) : base(pType)
        {
        }

        public override async void Enter(FightStateData pContext)
        {
            //Debug.LogWarning("FS - FS_Switch ENTER");

            switch (FightManager.Ins.SwitchToType)
            {
                // case SwitchToType.ToNextNode:
                // {
                //     await UniTask.Delay(1200);
                //     pContext.m_SM.ToStandby();
                // }
                //     break;
                case SwitchToType.ToFallBack:
                    {
                        pContext.m_SM.m_ContextData.m_LevelType = LevelType.NormalLevel;
                        EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.FallBack);
                        //EventManager.Call(LogicEvent.SkillReset);
                        GameDataManager.Ins.LevelState = LevelState.HandUp;

                        await UniTask.Delay(1000);
                        FightEnemyManager.Ins.ClearBattleground();
                        FightManager.Ins.m_CurGJJ.OnSwitching();
                        FightManager.Ins.m_CurGJJ.RecoverMaxHP();
                        EventManager.Call(LogicEvent.Fight_MapChanged, pContext.m_SM.m_ContextData.m_LevelType);
                        await UniTask.Delay(200);
                        pContext.m_SM.ToStandby();
                    }
                    break;
                case SwitchToType.ToBoss:
                    {
                        pContext.m_SM.m_ContextData.m_LevelType = LevelType.NormalLevel;
                        EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.NormalBoss);
                        //EventManager.Call(LogicEvent.SkillReset);

                        await UniTask.Delay(1000);
                        FightEnemyManager.Ins.ClearBattleground();
                        FightManager.Ins.m_CurGJJ.OnSwitching();
                        FightManager.Ins.m_CurGJJ.RecoverMaxHP();
                        EventManager.Call(LogicEvent.Fight_MapChanged, pContext.m_SM.m_ContextData.m_LevelType);
                        await UniTask.Delay(200);
                        pContext.m_SM.ToStandby();
                    }
                    break;
                case SwitchToType.ToNextLevel:
                    {
                        pContext.m_SM.m_ContextData.m_LevelType = LevelType.NormalLevel;
                        EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.NextLevel);
                        //EventManager.Call(LogicEvent.SkillReset);

                        await UniTask.Delay(1000);
                        FightEnemyManager.Ins.ClearBattleground();
                        FightManager.Ins.m_CurGJJ.OnSwitching();
                        FightManager.Ins.m_CurGJJ.RecoverMaxHP();
                        EventManager.Call(LogicEvent.Fight_MapChanged, pContext.m_SM.m_ContextData.m_LevelType);
                        await UniTask.Delay(200);
                        pContext.m_SM.ToStandby();
                    }
                    break;
                case SwitchToType.ToNormalLevel:
                    {
                        pContext.m_SM.m_ContextData.m_LevelType = LevelType.NormalLevel;
                        EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.Normal);
                        //EventManager.Call(LogicEvent.SkillReset);

                        await UniTask.Delay(1000);
                        FightEnemyManager.Ins.ClearBattleground();
                        FightManager.Ins.m_CurGJJ.OnSwitching();
                        FightManager.Ins.m_CurGJJ.RecoverMaxHP();
                        EventManager.Call(LogicEvent.Fight_MapChanged, pContext.m_SM.m_ContextData.m_LevelType);
                        await UniTask.Delay(200);
                        pContext.m_SM.ToStandby();
                    }
                    break;
                case SwitchToType.ToDiamondCopy:
                    {
                        pContext.m_SM.m_ContextData.m_LevelType = LevelType.DiamondCopy;
                        EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.NormalBoss);
                        //EventManager.Call(LogicEvent.SkillReset);

                        await UniTask.Delay(1000);
                        FightEnemyManager.Ins.ClearBattleground();
                        FightManager.Ins.m_CurGJJ.OnSwitching();
                        FightManager.Ins.m_CurGJJ.RecoverMaxHP();
                        EventManager.Call(LogicEvent.Fight_MapChanged, pContext.m_SM.m_ContextData.m_LevelType);
                        await UniTask.Delay(200);
                        pContext.m_SM.ToStandby();
                    }
                    break;
                case SwitchToType.ToCoinCopy:
                    {
                        pContext.m_SM.m_ContextData.m_LevelType = LevelType.CoinCopy;
                        EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.NormalBoss);
                        //EventManager.Call(LogicEvent.SkillReset);

                        await UniTask.Delay(1000);
                        FightEnemyManager.Ins.ClearBattleground();
                        FightManager.Ins.m_CurGJJ.OnSwitching();
                        FightManager.Ins.m_CurGJJ.RecoverMaxHP();
                        EventManager.Call(LogicEvent.Fight_MapChanged, pContext.m_SM.m_ContextData.m_LevelType);
                        await UniTask.Delay(200);
                        pContext.m_SM.ToStandby();
                    }
                    break;
                case SwitchToType.ToOilCopy:
                    {
                        pContext.m_SM.m_ContextData.m_LevelType = LevelType.OilCopy;
                        EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.NormalBoss);
                        //EventManager.Call(LogicEvent.SkillReset);

                        await UniTask.Delay(1000);
                        FightEnemyManager.Ins.ClearBattleground();
                        FightManager.Ins.m_CurGJJ.OnSwitching();
                        FightManager.Ins.m_CurGJJ.RecoverMaxHP();
                        EventManager.Call(LogicEvent.Fight_MapChanged, pContext.m_SM.m_ContextData.m_LevelType);
                        await UniTask.Delay(200);
                        pContext.m_SM.ToStandby();
                        break;
                    }
                case SwitchToType.ToTrophyCopy:
                    {
                        pContext.m_SM.m_ContextData.m_LevelType = LevelType.TrophyCopy;
                        EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.NormalBoss);
                        //EventManager.Call(LogicEvent.SkillReset);

                        await UniTask.Delay(1000);
                        FightEnemyManager.Ins.ClearBattleground();
                        FightManager.Ins.m_CurGJJ.OnSwitching();
                        FightManager.Ins.m_CurGJJ.RecoverMaxHP();
                        EventManager.Call(LogicEvent.Fight_MapChanged, pContext.m_SM.m_ContextData.m_LevelType);
                        await UniTask.Delay(200);
                        pContext.m_SM.ToStandby();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EventManager.Call(LogicEvent.SkillReset);
        }

        public override void Release(FightStateData pContext)
        {
            base.Release(pContext);
            EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.Close);
        }
    }
}