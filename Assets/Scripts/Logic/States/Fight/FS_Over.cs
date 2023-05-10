using System;
using System.Collections;
using Framework.EventKit;
using Framework.GameFSM;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Fight;
using Logic.Manager;
using Logic.UI.UICopy;
using Logic.UI.UIFight;
using Logic.UI.UIMain;
using Networks;
using UnityEngine;

namespace Logic.States.Fight
{
    /// <summary>
    /// 战斗结束 逻辑处理
    /// 此状态不能被主动切换到其他特殊状态(eg 特殊副本等)
    /// </summary>
    public class FS_Over : IState<FightState, FightStateData>
    {
        public FS_Over(FightState pType) : base(pType)
        {
        }

        private FightStateData m_FSMData;

        public override void Enter(FightStateData pContext)
        {
            Debug.LogWarning("FS - FS_Over ENTER");

            m_FSMData = pContext;
            EventManager.Call(LogicEvent.Fight_Over);
            FightManager.Ins.StartCoroutine(OnFightOver());
        }

        IEnumerator OnFightOver()
        {
            //非挂机状态 这里要等待一下
            if (m_FSMData.m_LevelType == LevelType.NormalLevel &&
                (GameDataManager.Ins.LevelState == LevelState.HandUp
                 || !FightManager.Ins.IsBossLevel()
                 && !FightManager.Ins.NextIsBossLevel()))
            {
                yield return null;
            }
            else
            {
                if (m_FSMData.m_IsWin)
                {
                    EventManager.Call(LogicEvent.Fight_Win);
                }
                else
                {
                    // 副本超时失败界面没有弹出
                    EventManager.Call(LogicEvent.ShowFightSwitch, FightSwitchEvent.FallBack);
                }

                yield return new WaitForSeconds(2f);
            }

            //TODO 打开战斗失败的提示UI
            if (!m_FSMData.m_IsWin)
            {
            }

            //清理战场 通知切换到待机状态
            //FightEnemyManager.Ins.ClearBattleground();

            FightFinished(m_FSMData.m_IsWin);
        }

        #region 战斗结束后处理

        //战斗结束处理
        public void FightFinished(bool pWasWin)
        {
            switch (m_FSMData.m_LevelType)
            {
                case LevelType.NormalLevel:
                {
                    if (GameDataManager.Ins.LevelState == LevelState.Normal)
                        OnNormalLevelFinished(pWasWin);
                    else
                        OnHandUpFinished(pWasWin);
                }
                    break;
                case LevelType.DiamondCopy:
                {
                    OnDiamondCopyFinished(pWasWin);
                }
                    break;
                case LevelType.CoinCopy:
                {
                    OnCoinCopyFinished(pWasWin);
                }
                    break;
                case LevelType.OilCopy:
                {
                    OnOilCopyFinished(pWasWin);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //普通关卡结束处理, 切换到下一关或下一节点的关卡数据
        private void OnNormalLevelFinished(bool pWasWin)
        {
            if (pWasWin)
            {
                if (FightManager.Ins.IsBossLevel()) //通关当前关卡BOSS
                {
                    //切换到下一关
                    GameDataManager.Ins.CurLevelID++;
                    GameDataManager.Ins.CurLevelNode = 1;
                    m_FSMData.m_SM.ToSwitch(SwitchToType.ToNextLevel);
                }
                else
                {
                    GameDataManager.Ins.CurLevelNode++;
                    if (GameDataManager.Ins.CurLevelNode == 5)
                    {
                        //一下节点是BOSS 切换到BOSS
                        m_FSMData.m_SM.ToSwitch(SwitchToType.ToBoss);
                    }
                    else
                    {
                        //切换到下一节点
                        //SwitchTo = SwitchToType.ToNextNode;
                        m_FSMData.m_SM.ToStandby();
                    }
                }
            }
            else
            {
                //如果不是BOSS关卡, 则退回到第一个节点
                if (!FightManager.Ins.IsBossLevel())
                    GameDataManager.Ins.CurLevelNode = 1;
                //退回到第一个节点
                m_FSMData.m_SM.ToSwitch(SwitchToType.ToFallBack);
            }
        }

        //挂机一波结束
        private void OnHandUpFinished(bool pWasWin)
        {
            if (pWasWin)
            {
                m_FSMData.m_SM.ToStandby();
            }
            else
            {
                m_FSMData.m_SM.ToSwitch(SwitchToType.ToFallBack);
            }
        }

        //钻石副本结束处理
        private void OnDiamondCopyFinished(bool pWasWin)
        {
            const LevelType levelType = LevelType.DiamondCopy;
            if (pWasWin)
            {
                CopyManager.Ins.m_DiamondCopyCount++;
                if (CopyManager.Ins.m_DiamondCopyCount > GameDefine.CopyDiamondCount) //副本结束
                {
                    CopyManager.Ins.StopCopyTimer();
                    NetworkManager.Ins.SendMsg(new C2S_ExitCopy
                    {
                        m_LevelType = (int)levelType,
                    });
                    OnCopyExit(true, levelType);
                }
                else
                {
                    m_FSMData.m_SM.ToStandby();
                }
            }
            else
            {
                CopyManager.Ins.StopCopyTimer();
                OnCopyExit(false, levelType);
            }
        }

        //金币副本结束处理
        private void OnCoinCopyFinished(bool pWasWin)
        {
            const LevelType levelType = LevelType.CoinCopy;
            CopyManager.Ins.StopCopyTimer();
            if (pWasWin)
                NetworkManager.Ins.SendMsg(new C2S_ExitCopy
                {
                    m_LevelType = (int)levelType,
                });

            OnCopyExit(pWasWin, levelType);
        }

        //引擎副本结束处理
        private void OnOilCopyFinished(bool pWasWin)
        {
            const LevelType levelType = LevelType.OilCopy;
            CopyManager.Ins.StopCopyTimer();
            NetworkManager.Ins.SendMsg(new C2S_ExitCopy
            {
                m_LevelType = (int)levelType,
                m_CurBossLevel = CopyManager.Ins.CurBossLevel,
                m_CurTotalDamage = CopyManager.Ins.CurTotalDamage.ToString()
            });

            OnCopyExit(pWasWin, levelType);
        }

        private void OnCopyExit(bool pWasWin, LevelType levelType)
        {
            UIManager.Ins.Hide<UICopyFighting>();
            UIManager.Ins.Show<UIMainLeft>();
            UIManager.Ins.Show<UIMainRight>();

            // 副本挑战失败返回到副本选择界面，再次打开进入副本界面
            if (!pWasWin)
            {
                UIManager.Ins.Show<UICopy>();

                switch(levelType)
                {
                    case LevelType.OilCopy:
                        //UIManager.Ins.Show<UIOilCopyEnter>();
                        break;
                    default:
                        UIManager.Ins.Show<UICopyEnter>(levelType);
                        break;
                          
                }
                
            }

            m_FSMData.m_SM.ToSwitch(SwitchToType.ToNormalLevel);
        }

        #endregion
    }
}