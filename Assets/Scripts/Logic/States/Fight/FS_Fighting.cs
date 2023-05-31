
using System;
using Framework.EventKit;
using Framework.GameFSM;
using Logic.Common;
using Logic.Fight;
using Logic.Fight.Common;
using Logic.Manager;
using UnityEngine;
using UnityTimer;

namespace Logic.States.Fight
{
    /// <summary>
    /// 战斗中 / 处理战斗成功 失败 等逻辑
    /// </summary>
    public class FS_Fighting : IState<FightState, FightStateData>
    {
        #region 战斗定时器

        private Timer m_Timer;
        private bool m_IsTimeUp = false;

        #endregion

        private bool m_InSwitch = false;
        private FightStateData m_StateData;
        public FS_Fighting(FightState pType) : base(pType)
        {
        }

        public override void Enter(FightStateData pContext)
        {
            //Debug.LogWarning("FS - Fighting ENTER");

            m_InSwitch = false;
            m_IsTimeUp = false;
            m_StateData = pContext;

            // 战斗中 当前战斗被终止事件
            m_EventGroup.Register(LogicEvent.Fight_Switch, OnFightSwitch);
            // 副本定时器
            m_EventGroup.Register(LogicEvent.Fight_CopyTimeUp, OnCopyTimeUp);

            switch (m_StateData.m_LevelType)
            {
                case LevelType.NormalLevel:
                    {
                        FightEnemyManager.Ins.StartLevelSpawn();
                        //是否是普通BOSS关卡
                        if (FightManager.Ins.IsBossLevel())
                        {
                            //通知UI显示BOSS战斗倒计时
                            EventManager.Call(LogicEvent.Fight_ShowNormalBossTime, true);
                            StartFightTimer(GameDefine.BOSSFightTime);
                        }
                    }
                    break;
                case LevelType.DiamondCopy:
                    {
                        FightEnemyManager.Ins.StartSpawnDiamondCopyBoss();

                        //通知UI显示BOSS战斗倒计时
                        if (CopyManager.Ins.m_DiamondCopyCount == 1)
                        {
                            CopyManager.Ins.StartCopyTimer();
                        }
                    }
                    break;
                case LevelType.CoinCopy:
                    {
                        FightEnemyManager.Ins.StartSpawnCoinCopyBoss();

                        //通知UI显示BOSS战斗倒计时
                        CopyManager.Ins.StartCopyTimer();
                    }
                    break;
                case LevelType.OilCopy:
                    {
                        FightEnemyManager.Ins.StartSpawnOilCopyBoss();

                        CopyManager.Ins.StartCopyTimer();
                    }
                    break;
                case LevelType.TrophyCopy:
                    {
                        FightEnemyManager.Ins.StartSpawnTrophyCopyEnemy();

                        //通知UI显示BOSS战斗倒计时
                        if (CopyManager.Ins.m_TrophyCopyCount == 1)
                        {
                            CopyManager.Ins.StartCopyTimer();
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            //通知各单位进入战斗状态(GJJ 普通攻击 伙伴 技能 等)
            EventManager.Call(LogicEvent.Fight_Fighting);
        }

        public override void Update(FightStateData pContext)
        {
            if (m_InSwitch)
                return;

            //TODO BOSS/特殊副本 定时器

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

            if (m_IsTimeUp)
            {
                pContext.m_IsWin = false;
                pContext.m_SM.ToFightOver();
                return;
            }

            //TODO 战斗中的其他逻辑
            //throw new Exception("---------------------");
        }

        public override void Release(FightStateData pContext)
        {
            base.Release(pContext);

            m_IsTimeUp = false;
            m_Timer?.Cancel();
            EventManager.Call(LogicEvent.Fight_ShowNormalBossTime, false);
        }

        private void OnFightSwitch(int arg1, object arg2)
        {
            var _Para = (FightSwitchTo)arg2;
            _Para.m_CanSwitchToNextNode = true;

            m_InSwitch = true;
            //清理战场 通知切换到待机状态
            EventManager.Call(LogicEvent.Fight_Over);
            m_StateData.m_SM.ToSwitch(_Para.m_SwitchToType);
        }

        /// <summary>
        /// 部分战斗需要定时器
        /// </summary>
        /// <param name="pCD">时间</param>
        private void StartFightTimer(float pCD)
        {
            m_Timer = Timer.Register(pCD, () =>
            {
                m_IsTimeUp = true;
                m_Timer = null;
            }, f =>
            {
                EventManager.Call(LogicEvent.Fight_NormalBossTimerChanged, f);
            });
        }

        private void OnCopyTimeUp(int i, object o)
        {
            m_IsTimeUp = true;
        }
    }
}