using System;
using System.Collections.Generic;
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
        public GameCopyData m_TrophyCopyData;
        public GameCopyData m_ReformCopyData;

        public int CurSelectedLevel { get; private set; }
        public int CurCDTime { get; set; }

        /// <summary>
        /// 原油副本BOSS当前等级
        /// </summary>
        int m_curBossLevel;

        public int CurBossLevel
        {
            get => m_curBossLevel;
            set
            {
                if (m_curBossLevel != value)
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

        private EventGroup m_EventGroup = new();

        /// <summary>
        /// 具体的每一个副本的逻辑管理器
        /// </summary>
        Dictionary<LevelType, CopyLogicManager> subCopyManagers = new Dictionary<LevelType, CopyLogicManager>();

        public void Init(S2C_Login pMsg)
        {
            m_DiamondCopyData = pMsg.DiamondCopyData;
            m_CoinCopyData = pMsg.CoinCopyData;
            m_OilCopyData = pMsg.OilCopyData;
            m_TrophyCopyData = pMsg.TrophyCopyData;
            m_ReformCopyData =  pMsg.ReformCopyData;

            //已经跨天 更新副本钥匙
            m_EventGroup.Register(LogicEvent.TimeDayChanged, (i, o) => SendC2SUpdateCopyKeyCount());


            subCopyManagers.Add(LevelType.ReformCopy, new ReformCopyManager(this));
        }

        /// <summary>
        /// 获取具体的副本逻辑管理器
        /// </summary>
        /// <param name="levelType"></param>
        /// <returns></returns>
        public CopyLogicManager GetCopyLogicManager(LevelType levelType)
        {
            if (!subCopyManagers.ContainsKey(levelType))
                return null;

            return subCopyManagers[levelType];
        }

        public override void OnSingletonRelease()
        {
            m_EventGroup.Release();
        }

        public async void OnEnterCopy(S2C_EnterCopy pMsg)
        {
            var levelType = (LevelType)pMsg.LevelType;
            var copyLogicManager = GetCopyLogicManager(levelType);
            //目前只重构了这一个副本种类，所以这个类型的走新的副本逻辑
            if(copyLogicManager != null && levelType == LevelType.ReformCopy)
            {
                await copyLogicManager.OnEnter(pMsg);
                return;
            }


            switch ((LevelType)pMsg.LevelType)
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
                case LevelType.TrophyCopy:
                {
                    m_TrophyCopyCount = 1;
                    CurCDTime = GameDefine.CopyDiamondTime;
                    var _Para = new FightSwitchTo { m_SwitchToType = SwitchToType.ToTrophyCopy };
                    EventManager.Call(LogicEvent.Fight_Switch, _Para);
                    if (_Para.m_CanSwitchToNextNode == false)
                        return;

                    break;
                }
                //case LevelType.ReformCopy:
                //    {
                //        m_ReformCopyCount = 1;
                //        var _Para = new FightSwitchTo { m_SwitchToType = SwitchToType.ToReformCopy };
                //        EventManager.Call(LogicEvent.Fight_Switch, _Para);
                //        if (_Para.m_CanSwitchToNextNode == false)
                //            return;

                //        break;
                //    }

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
            var levelType = (LevelType)pMsg.LevelType;
            var copyLogicManager = GetCopyLogicManager(levelType);
            //目前只重构了这一个副本种类，所以这个类型的走新的副本逻辑
            if (copyLogicManager != null && levelType == LevelType.ReformCopy)
            {
                copyLogicManager.OnExit(pMsg);
                return;
            }



            if (pMsg.LevelType == (int)LevelType.DiamondCopy)
            {
                m_DiamondCopyData.Level = pMsg.Level;
                m_DiamondCopyData.KeyCount = pMsg.KeyCount;

                TaskManager.Ins.DoTaskUpdate(TaskType.TT_2001, pMsg.Level);
            }
            else if (pMsg.LevelType == (int)LevelType.CoinCopy)
            {
                m_CoinCopyData.Level = pMsg.Level;
                m_CoinCopyData.KeyCount = pMsg.KeyCount;
                GameDataManager.Ins.Coin += GetCopyCoinReward(pMsg.Level - 1);

                TaskManager.Ins.DoTaskUpdate(TaskType.TT_2002, pMsg.Level);
            }
            else if (pMsg.LevelType == (int)LevelType.OilCopy)
            {
                m_OilCopyData.Level = pMsg.Level;
                m_OilCopyData.KeyCount = pMsg.KeyCount;
                m_OilCopyData.BestDamageRecord = pMsg.CurTotalDamage;
                m_OilCopyData.BestLevelRecord = pMsg.CurBossLevel;
            }
            else if (pMsg.LevelType == (int)LevelType.TrophyCopy)
            {
                m_TrophyCopyData.Level = pMsg.Level;
                m_TrophyCopyData.KeyCount = pMsg.KeyCount;
                GameDataManager.Ins.Trophy += GetCopyTrophyReward(pMsg.Level - 1);
            }
           


            // 副本挑战成功返回到副本选择界面，再次打开进入副本界面
            UIManager.Ins.Show<UICopy>();

            if (pMsg.LevelType != (int)LevelType.OilCopy)
            {
                //UIManager.Ins.Show<UICopyEnter>(pMsg.m_LevelType);
                await UIManager.Ins.OpenUI<UICopyExit>(pMsg);
            }
        }

        public void OnUpdateCopyKeyCount(S2C_UpdateCopyKeyCount pMsg)
        {
            m_DiamondCopyData.KeyCount = pMsg.DiamondKeyCount;
            m_CoinCopyData.KeyCount = pMsg.CoinKeyCount;
            m_OilCopyData.KeyCount = pMsg.OilKeyCount;
            m_TrophyCopyData.KeyCount = pMsg.TrophyKeyCount;
            m_ReformCopyData.KeyCount = pMsg.ReformKeyCount;

            EventManager.Call(LogicEvent.CopyKeyChanged);
        }

        #region 发送消息

        public void SendEnterCopy(LevelType pLevelType, int pCurSelectLevel)
        {
            CurSelectedLevel = pCurSelectLevel;
            NetworkManager.Ins.SendMsg(new C2S_EnterCopy { LevelType = (int)pLevelType });
        }

        /// <summary>
        /// 已经跨天
        /// 向服务器发送更新副本钥匙协议
        /// </summary>
        public void SendC2SUpdateCopyKeyCount()
        {
            NetworkManager.Ins.SendMsg(new C2S_UpdateCopyKeyCount());
        }

        /// <summary>
        /// 请求退出副本
        /// </summary>
        /// <param name="pLevelType"></param>
        /// <param name="isWin"></param>
        public void SendExitCopy(LevelType pLevelType, bool isWin)
        {
            var manager = GetCopyLogicManager(pLevelType);
            manager.RequestExitCopy(isWin);
        }

        #endregion

        #region 战斗副本逻辑

        public int m_DiamondCopyCount = 0; //刷怪波次
        public int m_TrophyCopyCount = 0;
        public int m_ReformCopyCount = 0;

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
            var _data = CopyCoinCfg.GetData(m_CoinCopyData.Level);
            if (_data == null)
                _data = CopyCoinCfg.GetData(0);
            return _data.ResGroupId;
        }

        public BigDouble GetCopyCoinBossHp()
        {
            var _data = CopyCoinCfg.GetData(m_CoinCopyData.Level);
            if (_data == null)
                _data = CopyCoinCfg.GetData(0);
            return (BigDouble)_data.BOSSHPBase +
                   (_data.BOSSHPGrow + _data.BOSSHPExp) * (BigDouble)m_CoinCopyData.Level;
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
            var _data = CopyDiamondCfg.GetData(m_DiamondCopyData.Level);
            if (_data == null)
                _data = CopyDiamondCfg.GetData(0);
            return _data.ResGroupId;
        }

        public BigDouble GetCopyDiamondBossHp()
        {
            var _data = CopyDiamondCfg.GetData(m_DiamondCopyData.Level);
            if (_data == null)
                _data = CopyDiamondCfg.GetData(0);
            return (BigDouble)_data.BOSSHPBase +
                   (_data.BOSSHPGrow + _data.BOSSHPExp) * (BigDouble)m_CoinCopyData.Level;
        }

        public BigDouble GetCopyDiamondBossATK()
        {
            var _data = CopyDiamondCfg.GetData(m_DiamondCopyData.Level);
            if (_data == null)
                _data = CopyDiamondCfg.GetData(0);
            return (BigDouble)_data.BOSSAtkBase +
                   (_data.BOSSAtkGrow + _data.BOSSAtkExp) * (BigDouble)m_CoinCopyData.Level;
        }

        public int GetCopyDiamondBossCount()
        {
            var _data = CopyDiamondCfg.GetData(m_DiamondCopyData.Level);
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

            return 5 * CurBossAttackCount; // MathF.Pow(GameDefine.CopyOilBossAtkConstValue, CurBossAttackCount);
        }

        public BigDouble GetCopyOilBossHp()
        {
            var _data = CopyOilCfg.GetData(CurBossLevel);
            if (_data == null)
                _data = CopyOilCfg.GetData(0);

            int multip = CurBossLevel - GameDefine.CopyOilConstValue;

            multip = multip <= 0 ? 0 : multip;

            return (BigDouble)(_data.BOSSHPBase +
                               (_data.BOSSHPGrow * CurBossLevel) * MathF.Pow(_data.BOSSHPGrowMultiple, multip));
        }

        public int GetCopyOilBossID()
        {
            var _data = CopyOilCfg.GetData(CurBossLevel);
            if (_data == null)
                _data = CopyOilCfg.GetData(0);
            return _data.ResGroupId;
        }


        public BigDouble GetCopyTrophyReward(int pLevel)
        {
            var _data = CopyTrophyCfg.GetData(pLevel);
            if (_data == null)
                _data = CopyTrophyCfg.GetData(0);
            return (BigDouble)_data.RewardBase + (_data.RewardGrow + _data.RewardExp) * (BigDouble)pLevel;
        }

        public int GetCopyTrophyBossCount()
        {
            var _data = CopyTrophyCfg.GetData(m_TrophyCopyData.Level);
            if (_data == null)
                _data = CopyTrophyCfg.GetData(0);
            return RandomHelper.Range(_data.MonsterCountMin, _data.MonsterCountMax);
        }

        public BigDouble GetCopyTrophyBossATK()
        {
            var _data = CopyTrophyCfg.GetData(m_TrophyCopyData.Level);
            if (_data == null)
                _data = CopyTrophyCfg.GetData(0);
            return (BigDouble)_data.BOSSAtkBase +
                   (_data.BOSSAtkGrow + _data.BOSSAtkExp) * (BigDouble)m_CoinCopyData.Level;
        }

        public BigDouble GetCopyTrophyBossHp()
        {
            var _data = CopyTrophyCfg.GetData(m_TrophyCopyData.Level);
            if (_data == null)
                _data = CopyTrophyCfg.GetData(0);
            return (BigDouble)_data.BOSSHPBase +
                   (_data.BOSSHPGrow + _data.BOSSHPExp) * (BigDouble)m_CoinCopyData.Level;
        }

        public int GetCopyTrophyBossID()
        {
            var _data = CopyTrophyCfg.GetData(m_TrophyCopyData.Level);
            if (_data == null)
                _data = CopyTrophyCfg.GetData(0);
            return _data.ResGroupId;
        }


        public long GetCopyReformReward(int pLevel)
        {
            var _data = CopyReformCfg.GetData(pLevel);
            if (_data == null)
                _data = CopyReformCfg.GetData(0);
            return (long)_data.RewardBase + (_data.RewardGrow + _data.RewardExp) * (long)pLevel;
        }


        public int GetCopyReformBossCount()
        {
            var _data = CopyReformCfg.GetData(m_ReformCopyData.Level);
            if (_data == null)
                _data = CopyReformCfg.GetData(0);
            return RandomHelper.Range(_data.MonsterCountMin, _data.MonsterCountMax);
        }

        public BigDouble GetCopyRefromBossATK()
        {
            var _data = CopyReformCfg.GetData(m_ReformCopyData.Level);
            if (_data == null)
                _data = CopyReformCfg.GetData(0);
            return (BigDouble)_data.BOSSAtkBase +
                   (_data.BOSSAtkGrow + _data.BOSSAtkExp) * (BigDouble)m_CoinCopyData.Level;
        }

        public BigDouble GetCopyRefromBossHp()
        {
            var _data = CopyReformCfg.GetData(m_ReformCopyData.Level);
            if (_data == null)
                _data = CopyReformCfg.GetData(0);
            return (BigDouble)_data.BOSSHPBase +
                   (_data.BOSSHPGrow + _data.BOSSHPExp) * (BigDouble)m_CoinCopyData.Level;
        }

        public int GetCopyReformBossID()
        {
            var _data = CopyReformCfg.GetData(m_ReformCopyData.Level);
            if (_data == null)
                _data = CopyReformCfg.GetData(0);
            return _data.ResGroupId;
        }

        #endregion
    }
}