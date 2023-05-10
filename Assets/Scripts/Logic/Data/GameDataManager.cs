using System;
using BreakInfinity;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Networks;
using UnityEngine;

namespace Logic.Data
{
    /// <summary>
    /// 游戏全局数据管理
    /// </summary>
    public class GameDataManager : Singleton<GameDataManager>
    {
        private UserData m_Data;

        public override void OnSingletonInit()
        {
            m_Data = new UserData();
        }

        /// <summary>
        /// 登录初始化游戏全局数据
        /// 所有的Manager都在这里初始化
        /// </summary>
        /// <param name="pData">服务器登录数据</param>
        public void OnReceiveLoginData(S2C_Login pData)
        {
            m_Data.m_IsFirstLogin = pData.m_IsFirstLogin;
            m_Data.m_LastGameDate = DateTime.Parse(pData.m_LastGameDate);

            m_Data.m_Coin = BigDouble.Parse(pData.m_Coin);
            m_Data.m_Diamond = pData.m_Diamond;
            m_Data.m_Iron = pData.m_Iron;
            m_Data.m_Oil = pData.m_Oil;

            m_Data.m_AutoSkill = pData.m_AutoSkill;
            m_Data.m_IsMusicOn = pData.m_IsMusicOn;
            m_Data.m_IsSoundOn = pData.m_IsSoundOn;

            m_Data.m_GJJAtkLevel = pData.m_GJJAtkLevel;
            m_Data.m_GJJHPLevel = pData.m_GJJHPLevel;
            m_Data.m_GJJHPRecoverLevel = pData.m_GJJHPRecoverLevel;
            m_Data.m_GJJCriticalLevel = pData.m_GJJCriticalLevel;
            m_Data.m_GJJCriticalDamageLevel = pData.m_GJJCriticalDamageLevel;
            m_Data.m_GJJAtkSpeedLevel = pData.m_GJJAtkSpeedLevel;
            m_Data.m_GJJDoubleHitLevel = pData.m_GJJDoubleHitLevel;
            m_Data.m_GJJTripletHitLevel = pData.m_GJJTripletHitLevel;

            m_Data.m_CurLevelID = pData.m_CurLevelID;
            m_Data.m_CurLevelNode = pData.m_CurLevelNode;
            m_Data.m_LevelState = (LevelState)pData.m_CurLevelState;

            m_Data.m_BtnPlaceRewardClickTime = pData.m_BtnPlaceRewardClickTime;
            m_Data.m_BtnPlaceRewardShowTime = pData.m_BtnPlaceRewardShowTime;
            m_Data.m_PopPlaceRewardTime = pData.m_PopPlaceRewardTime;

            //装备
            EquipManager.Ins.Init(pData.m_WeaponList, pData.m_ArmorList, pData.m_WeaponOnID, pData.m_ClothesOnID);
            //伙伴
            PartnerManager.Ins.Init(pData.m_PartnerList, pData.m_PartnerOnList);
            //技能
            SkillManager.Ins.Init(pData.m_SkillList, pData.m_SkillOnList);
            //引擎
            EngineManager.Ins.Init(pData.m_EngineList, pData.m_EngineOnId, pData.m_EngineGetId);
            //抽卡
            ShopManager.Ins.Init(pData);
            //任务
            TaskManager.Ins.Init(pData);
            //副本
            CopyManager.Ins.Init(pData);
            //考古
            MiningManager.Ins.Init(pData);
            //开放剧情
            LockStoryManager.Ins.Init(pData.m_LockStoryList);
            //考古研究
            ResearchManager.Ins.Init(pData.m_ResearchList, pData.m_ResearchEffectData);

            //通知其他逻辑 成功获取玩家基础数据 登录成功
            EventManager.Call(LogicEvent.LoginSuccess);

            Debug.LogError("上次登录时间 : " + LastGameDate);
        }

        #region 数据接口

        //是否第一次登录
        public bool IsFirstLogin
        {
            get => m_Data.m_IsFirstLogin;
            set => m_Data.m_IsFirstLogin = value;
        }

        //上次游戏时间
        public DateTime LastGameDate
        {
            get => m_Data.m_LastGameDate;
            set => m_Data.m_LastGameDate = value;
        }

        //游戏币
        public BigDouble Coin
        {
            get => m_Data.m_Coin;
            set
            {
                m_Data.m_Coin = value;
                EventManager.Call(LogicEvent.CoinChanged);
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //元宝
        public long Diamond
        {
            get => m_Data.m_Diamond;
            set
            {
                m_Data.m_Diamond = value;
                EventManager.Call(LogicEvent.DiamondChanged);
            }
        }

        //原油
        public int Oil
        {
            get => m_Data.m_Oil;
            set
            {
                m_Data.m_Oil = value;
                EventManager.Call(LogicEvent.OilChanged);
            }
        }

        //钢铁
        public int Iron
        {
            get => m_Data.m_Iron;
            set
            {
                m_Data.m_Iron = value;
                EventManager.Call(LogicEvent.EngineIronUpdate);
            }
        }

        public bool AutoSkill
        {
            get => m_Data.m_AutoSkill;
            set => m_Data.m_AutoSkill = value;
        }

        //GJJ攻击力等级
        public long GJJAtkLevel
        {
            get => m_Data.m_GJJAtkLevel;
            set
            {
                m_Data.m_GJJAtkLevel = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //GJJ血量等级
        public long GJJHPLevel
        {
            get => m_Data.m_GJJHPLevel;
            set
            {
                m_Data.m_GJJHPLevel = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //GJJ血量恢复速度等级
        public long GJJHPRecoverLevel
        {
            get => m_Data.m_GJJHPRecoverLevel;
            set
            {
                m_Data.m_GJJHPRecoverLevel = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //GJJ暴击等级
        public long GJJCriticalLevel
        {
            get => m_Data.m_GJJCriticalLevel;
            set
            {
                m_Data.m_GJJCriticalLevel = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //GJJ暴击伤害等级
        public long GJJCriticalDamageLevel
        {
            get => m_Data.m_GJJCriticalDamageLevel;
            set
            {
                m_Data.m_GJJCriticalDamageLevel = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //GJJ攻击速度等级
        public long GJJAtkSpeedLevel
        {
            get => m_Data.m_GJJAtkSpeedLevel;
            set
            {
                m_Data.m_GJJAtkSpeedLevel = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //GJJ双倍攻击等级
        public long GJJDoubleHitLevel
        {
            get => m_Data.m_GJJDoubleHitLevel;
            set
            {
                m_Data.m_GJJDoubleHitLevel = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //GJJ三倍攻击等级
        public long GJJTripletHitLevel
        {
            get => m_Data.m_GJJTripletHitLevel;
            set
            {
                m_Data.m_GJJTripletHitLevel = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //当前关卡ID
        public long CurLevelID
        {
            get => m_Data.m_CurLevelID;
            set
            {
                m_Data.m_CurLevelID = value;
                EventManager.Call(LogicEvent.Fight_LevelChanged);
                EventManager.Call(LogicEvent.SyncUserData);
                TaskManager.Ins.DoTaskUpdate(TaskType.TT_1001, CurLevelID);
            }
        }

        //当前关卡节点
        public int CurLevelNode
        {
            get => m_Data.m_CurLevelNode;
            set
            {
                m_Data.m_CurLevelNode = value;
                EventManager.Call(LogicEvent.Fight_LevelNodeChanged);
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //当前关卡状态
        public LevelState LevelState
        {
            get => m_Data.m_LevelState;
            set
            {
                m_Data.m_LevelState = value;
                EventManager.Call(LogicEvent.SyncUserData);
            }
        }

        //当前放置奖励按钮点击时间戳
        public long BtnPlaceRewardClickTime
        {
            get => m_Data.m_BtnPlaceRewardClickTime;
            set => m_Data.m_BtnPlaceRewardClickTime = value;
        }

        // 放置奖励按钮显示时间
        public float BtnPlaceRewardShowTime
        {
            get => m_Data.m_BtnPlaceRewardShowTime;
            set => m_Data.m_BtnPlaceRewardShowTime = value;
        }

        // 放置奖励页面每日主动弹出时间
        public string PopPlaceRewardTime
        {
            get => m_Data.m_PopPlaceRewardTime;
            set => m_Data.m_PopPlaceRewardTime = value;
        }

        #endregion
    }
}