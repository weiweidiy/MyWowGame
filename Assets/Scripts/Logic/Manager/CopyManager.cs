using System;
using BreakInfinity;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.UI.UICopy;
using Logic.UI.UIMain;
using Networks;
using UnityTimer;

namespace Logic.Manager
{
    /// <summary>
    /// 副本管理器
    /// </summary>
    public class CopyManager : Singleton<CopyManager>
    {
        public GameCopyData m_DiamondCopyData;
        public GameCopyData m_CoinCopyData;
        public GameCopyOilData m_OilCopyData;

        public int CurSelectedLevel { get; private set; }
        public int CurCDTime { get; set; }

        /// <summary>
        /// 原油副本BOSS当前等级
        /// </summary>
        int m_curBossLevel;
        public int CurBossLevel { get => m_curBossLevel;
            set {
                if( m_curBossLevel != value)
                {
                    m_curBossLevel = value;
                    EventManager.Call(LogicEvent.Fight_OilBossLevelChanged, value);
                }
                
            } 
        } 

        /// <summary>
        /// 原油副本Boss当前的攻击次数
        /// </summary>
        public int CurBossAttackCount { get; set; } //记录当前副本boss攻击次数

        /// <summary>
        /// 原油副本玩家输入的总伤害
        /// </summary>
        public BigDouble CurTotalDamage { get; set; } //记录当前总伤害

        public void Init(S2C_Login pMsg)
        {
            m_DiamondCopyData = pMsg.m_DiamondCopyData;
            m_CoinCopyData = pMsg.m_CoinCopyData;
            m_OilCopyData = pMsg.m_OilCopyData;

            //已经跨天 重置
            //TODO 临时在客户端实现
            if (GameDataManager.Ins.LastGameDate.Day != DateTime.Now.Day)
            {
                SendC2SUpdateCopyKeyCount();
            }
        }

        public async void OnEnterCopy(S2C_EnterCopy pMsg)
        {
            switch ((LevelType)pMsg.m_LevelType)
            {
                case LevelType.DiamondCopy:
                    {
                        m_DiamondCopyCount = 1;
                        CurCDTime = GameDefine.CopyDiamondTime;
                        var _Para = new FightSwitchTo { m_SwitchToType = SwitchToType.ToDiamondCopy };
                        EventManager.Call(LogicEvent.Fight_Switch, _Para);
                        if (_Para.m_CanSwitchToNextNode == false)
                            return;
                    }
                    break;
                case LevelType.CoinCopy:
                    {
                        CurCDTime = GameDefine.CopyCoinTime;
                        var _Para = new FightSwitchTo { m_SwitchToType = SwitchToType.ToCoinCopy };
                        EventManager.Call(LogicEvent.Fight_Switch, _Para);
                        if (_Para.m_CanSwitchToNextNode == false)
                            return;
                        break;
                    }
                case LevelType.OilCopy:
                    {
                        CurBossLevel = 1;
                        CurBossAttackCount = 1;
                        CurTotalDamage = 0;
                        CurCDTime = GameDefine.CopyCoinTime;
                        var _Para = new FightSwitchTo { m_SwitchToType = SwitchToType.ToOilCopy };
                        EventManager.Call(LogicEvent.Fight_Switch, _Para);
                        if (_Para.m_CanSwitchToNextNode == false)
                            return;
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }

            UIManager.Ins.Hide<UIMainLeft>();
            UIManager.Ins.Hide<UIMainRight>();
            UIManager.Ins.Hide<UICopy>();
            await UIManager.Ins.OpenUI<UICopyFighting>();
        }

        /// <summary>
        /// 副本成功通关
        /// </summary>
        public async void OnExitCopy(S2C_ExitCopy pMsg)
        {
            if (pMsg.m_LevelType == (int)LevelType.DiamondCopy)
            {
                m_DiamondCopyData.m_Level = pMsg.m_Level;
                m_DiamondCopyData.m_KeyCount = pMsg.m_KeyCount;

                TaskManager.Ins.DoTaskUpdate(TaskType.TT_2001, pMsg.m_Level);
            }
            else if (pMsg.m_LevelType == (int)LevelType.CoinCopy)
            {
                m_CoinCopyData.m_Level = pMsg.m_Level;
                m_CoinCopyData.m_KeyCount = pMsg.m_KeyCount;
                GameDataManager.Ins.Coin += GetCopyCoinReward(pMsg.m_Level - 1);

                TaskManager.Ins.DoTaskUpdate(TaskType.TT_2002, pMsg.m_Level);
            }
            else if(pMsg.m_LevelType == (int)LevelType.OilCopy)
            {
                m_OilCopyData.m_Level = pMsg.m_Level;
                m_OilCopyData.m_KeyCount = pMsg.m_KeyCount;
                m_OilCopyData.m_BestDamageRecord = pMsg.m_CurTotalDamage;
                m_OilCopyData.m_BestLevelRecord = pMsg.m_CurBossLevel;

            }

            // 副本挑战成功返回到副本选择界面，再次打开进入副本界面
            UIManager.Ins.Show<UICopy>();

            if(pMsg.m_LevelType != (int)LevelType.OilCopy)
            {
                UIManager.Ins.Show<UICopyEnter>(pMsg.m_LevelType);
                await UIManager.Ins.OpenUI<UICopyExit>(pMsg);
            }
        }

        public void OnUpdateCopyKeyCount(S2C_UpdateCopyKeyCount pMsg)
        {
            m_DiamondCopyData.m_KeyCount = pMsg.m_DiamondKeyCount;
            m_CoinCopyData.m_KeyCount = pMsg.m_CoinKeyCount;
            m_OilCopyData.m_KeyCount = pMsg.m_OilKeyCount;
            EventManager.Call(LogicEvent.CopyKeyChanged);
        }

        #region 发送消息

        public void SendEnterCopy(LevelType pLevelType, int pCurSelectLevel)
        {
            CurSelectedLevel = pCurSelectLevel;
            NetworkManager.Ins.SendMsg(new C2S_EnterCopy { m_LevelType = (int)pLevelType });
        }

        /// <summary>
        /// 已经跨天
        /// 向服务器发送更新副本钥匙协议
        /// </summary>
        public void SendC2SUpdateCopyKeyCount()
        {
            NetworkManager.Ins.SendMsg(new C2S_UpdateCopyKeyCount());
        }

        #endregion

        #region 战斗副本逻辑

        public int m_DiamondCopyCount = 0; //刷怪

        private Timer m_Timer;

        public void StartCopyTimer()
        {
            EventManager.Call(LogicEvent.Fight_ShowCopyBossTime, true);
            EventManager.Call(LogicEvent.Fight_CopyBossTimerChanged);
            m_Timer = Timer.Register(1, () =>
            {
                CurCDTime--;
                EventManager.Call(LogicEvent.Fight_CopyBossTimerChanged);
                if (CurCDTime <= 0)
                {
                    EventManager.Call(LogicEvent.Fight_ShowCopyBossTime, false);
                    EventManager.Call(LogicEvent.Fight_CopyTimeUp);
                    m_Timer.Cancel();
                    m_Timer = null;
                }
            }, null, true);
        }

        public void StopCopyTimer()
        {
            EventManager.Call(LogicEvent.Fight_ShowCopyBossTime, false);
            if (m_Timer != null)
            {
                m_Timer.Cancel();
                m_Timer = null;
            }
        }

        #endregion

        #region 副本相关数值公式处理

        public BigDouble GetCopyCoinReward(int pLevel)
        {
            var _data = CopyCoinCfg.GetData(pLevel);
            if (_data == null)
                _data = CopyCoinCfg.GetData(0);
            return (BigDouble)_data.RewardBase + (_data.RewardGrow + _data.RewardExp) * (BigDouble)pLevel;
        }

        public int GetCopyCoinBossID()
        {
            var _data = CopyCoinCfg.GetData(m_CoinCopyData.m_Level);
            if (_data == null)
                _data = CopyCoinCfg.GetData(0);
            return _data.ResGroupId;
        }

        public BigDouble GetCopyCoinBossHp()
        {
            var _data = CopyCoinCfg.GetData(m_CoinCopyData.m_Level);
            if (_data == null)
                _data = CopyCoinCfg.GetData(0);
            return (BigDouble)_data.BOSSHPBase +
                   (_data.BOSSHPGrow + _data.BOSSHPExp) * (BigDouble)m_CoinCopyData.m_Level;
        }

        public long GetCopyDiamondReward(int pLevel)
        {
            var _data = CopyDiamondCfg.GetData(pLevel);
            if (_data == null)
                _data = CopyDiamondCfg.GetData(0);
            return ((long)_data.RewardBase + (_data.RewardGrow + _data.RewardExp) * (long)pLevel);
        }

        public int GetCopyDiamondBossID()
        {
            var _data = CopyDiamondCfg.GetData(m_DiamondCopyData.m_Level);
            if (_data == null)
                _data = CopyDiamondCfg.GetData(0);
            return _data.ResGroupId;
        }

        public BigDouble GetCopyDiamondBossHp()
        {
            var _data = CopyDiamondCfg.GetData(m_DiamondCopyData.m_Level);
            if (_data == null)
                _data = CopyDiamondCfg.GetData(0);
            return (BigDouble)_data.BOSSHPBase +
                   (_data.BOSSHPGrow + _data.BOSSHPExp) * (BigDouble)m_CoinCopyData.m_Level;
        }

        public BigDouble GetCopyDiamondBossATK()
        {
            var _data = CopyDiamondCfg.GetData(m_DiamondCopyData.m_Level);
            if (_data == null)
                _data = CopyDiamondCfg.GetData(0);
            return (BigDouble)_data.BOSSAtkBase +
                   (_data.BOSSAtkGrow + _data.BOSSAtkExp) * (BigDouble)m_CoinCopyData.m_Level;
        }

        public int GetCopyDiamondBossCount()
        {
            var _data = CopyDiamondCfg.GetData(m_DiamondCopyData.m_Level);
            if (_data == null)
                _data = CopyDiamondCfg.GetData(0);
            return RandomHelper.Range(_data.MonsterCountMin, _data.MonsterCountMax);
        }

        public int GetOilCopyBossCount()
        {
            return 1;
        }

        public BigDouble GetCopyOilBossATK()
        {
            var _data = CopyOilCfg.GetData(CurBossLevel);
            if (_data == null)
                _data = CopyOilCfg.GetData(0);

            return 5 * CurBossAttackCount;// MathF.Pow(GameDefine.CopyOilBossAtkConstValue, CurBossAttackCount);
        }

        public BigDouble GetCopyOilBossHp()
        {
            var _data = CopyOilCfg.GetData(CurBossLevel);
            if (_data == null)
                _data = CopyOilCfg.GetData(0);

            int multip = CurBossLevel - GameDefine.CopyOilConstValue;

            multip = multip <= 0 ? 0 : multip;

            return 1000;// (BigDouble)(_data.BOSSHPBase + (_data.BOSSHPGrow * CurBossLevel) * MathF.Pow(_data.BOSSHPGrowMultiple , multip));
        }

        public int GetCopyOilBossID()
        {
            var _data = CopyOilCfg.GetData(CurBossLevel);
            if (_data == null)
                _data = CopyOilCfg.GetData(0);
            return _data.ResGroupId;
        }

        #endregion
    }
}