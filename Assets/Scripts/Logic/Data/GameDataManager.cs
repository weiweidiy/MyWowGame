using System;
using BreakInfinity;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Logic.Common;
using Logic.Common.RedDot;
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
            m_Data.m_LastGameDate = pData.LastGameDate;
            m_Data.m_ServerTimes = pData.ST;

            m_Data.m_Coin = BigDouble.Parse(pData.Coin);
            m_Data.m_Diamond = pData.Diamond;
            m_Data.m_Oil = pData.Oil;
            m_Data.m_Trophy = BigDouble.Parse(pData.Trophy);
            m_Data.m_MushRoom = pData.MushRoom;
            m_Data.m_BreakOre = pData.BreakOre;
            m_Data.m_BreakTP = pData.BreakTP;
            m_Data.m_TecPoint = pData.TecPoint;

            m_Data.m_AutoSkill = pData.AutoSkill;
            m_Data.m_IsMusicOn = pData.IsMusicOn;
            m_Data.m_IsSoundOn = pData.IsSoundOn;

            m_Data.m_GJJAtkLevel = pData.GJJAtkLevel;
            m_Data.m_GJJHPLevel = pData.GJJHPLevel;
            m_Data.m_GJJHPRecoverLevel = pData.GJJHPRecoverLevel;
            m_Data.m_GJJCriticalLevel = pData.GJJCriticalLevel;
            m_Data.m_GJJCriticalDamageLevel = pData.GJJCriticalDamageLevel;
            m_Data.m_GJJAtkSpeedLevel = pData.GJJAtkSpeedLevel;
            m_Data.m_GJJDoubleHitLevel = pData.GJJDoubleHitLevel;
            m_Data.m_GJJTripletHitLevel = pData.GJJTripletHitLevel;

            m_Data.m_CurLevelID = pData.CurLevelID;
            m_Data.m_CurLevelNode = pData.CurLevelNode;
            m_Data.m_LevelState = (LevelState)pData.CurLevelState;

            m_Data.m_BtnPlaceRewardClickTime = pData.PlaceRewardClickTime;
            m_Data.m_BtnPlaceRewardShowTime = pData.PlaceRewardShowTime;
            m_Data.m_PopPlaceRewardTime = pData.PlaceRewardPopTime;

            try
            {
                //装备
                EquipManager.Ins.Init(pData.WeaponList, pData.ArmorList, pData.WeaponOnID, pData.ArmorOnID);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //伙伴
                PartnerManager.Ins.Init(pData.PartnerList, pData.PartnerOnList);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //技能
                SkillManager.Ins.Init(pData.SkillList, pData.SkillOnList);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //抽卡
                ShopManager.Ins.Init(pData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //任务
                TaskManager.Ins.Init(pData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //副本
                CopyManager.Ins.Init(pData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //考古
                MiningManager.Ins.Init(pData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //开放剧情
                LockStoryManager.Ins.Init(pData.LockStoryList);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //考古研究
                ResearchManager.Ins.Init(pData.ResearchList, pData.ResearchEffectData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //淬炼
                QuenchingManager.Ins.Init(pData.QuenchingList);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //战利品
                SpoilManager.Ins.Init(pData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //舱室
                RoomManager.Ins.Init(pData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //英雄
                RoleManager.Ins.Init(pData.RoleList, pData.RoleOnId);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //英雄突破天赋树
                RoleBreakTreeManager.Ins.Init(pData.RoleBreakTree);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //引擎
                EngineManager.Ins.Init(pData.EngineData, pData.EngineParts);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            try
            {
                //红点系统
                //需要再最后进行初始化
                BigBoomRedDotManager.Ins.Init();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            //通知其他逻辑 成功获取玩家基础数据 登录成功
            EventManager.Call(LogicEvent.LoginSuccess);

            Debug.LogError("上次登录时间 : " + TimeHelper.GetDateTime(LastGameDate));
        }

        #region 数据接口

        //上次游戏时间
        public long LastGameDate => m_Data.m_LastGameDate;

        // 服务器当前时间
        public ServerTimes ServerTimes => m_Data.m_ServerTimes;

        //游戏币
        public BigDouble Coin
        {
            get => m_Data.m_Coin;
            set
            {
                m_Data.m_Coin = value;
                EventManager.Call(LogicEvent.CoinChanged);
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.CoinData);
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

        public BigDouble Trophy
        {
            get => m_Data.m_Trophy;
            set
            {
                m_Data.m_Trophy = value;
                EventManager.Call(LogicEvent.TropyChanged);
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.TrophyData);
            }
        }

        //英雄升级蘑菇
        public int MushRoom
        {
            get => m_Data.m_MushRoom;
            set
            {
                m_Data.m_MushRoom = value;
                EventManager.Call(LogicEvent.RoleMushRoomChanged);
            }
        }

        //英雄突破矿石
        public int BreakOre
        {
            get => m_Data.m_BreakOre;
            set
            {
                m_Data.m_BreakOre = value;
                EventManager.Call(LogicEvent.RoleBreakOreChanged);
            }
        }

        //英雄突破天赋点
        public int BreakTP
        {
            get => m_Data.m_BreakTP;
            set
            {
                m_Data.m_BreakTP = value;
                EventManager.Call(LogicEvent.RoleBreakTPChanged);
            }
        }

        public int TecPoint
        {
            get => m_Data.m_TecPoint;
            set
            {
                m_Data.m_TecPoint = value;
                EventManager.Call(LogicEvent.TecPointChanged);
            }
        }

        //自动攻击
        public bool AutoSkill
        {
            get => m_Data.m_AutoSkill;
            set => m_Data.m_AutoSkill = value;
        }

        //音乐
        public bool Music
        {
            get => m_Data.m_IsMusicOn;
            set => m_Data.m_IsMusicOn = value;
        }

        //音效
        public bool Sound
        {
            get => m_Data.m_IsSoundOn;
            set => m_Data.m_IsSoundOn = value;
        }

        //GJJ攻击力等级
        public long GJJAtkLevel
        {
            get => m_Data.m_GJJAtkLevel;
            set
            {
                m_Data.m_GJJAtkLevel = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.RoomData);
            }
        }

        //GJJ血量等级
        public long GJJHPLevel
        {
            get => m_Data.m_GJJHPLevel;
            set
            {
                m_Data.m_GJJHPLevel = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.RoomData);
            }
        }

        //GJJ血量恢复速度等级
        public long GJJHPRecoverLevel
        {
            get => m_Data.m_GJJHPRecoverLevel;
            set
            {
                m_Data.m_GJJHPRecoverLevel = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.RoomData);
            }
        }

        //GJJ暴击等级
        public long GJJCriticalLevel
        {
            get => m_Data.m_GJJCriticalLevel;
            set
            {
                m_Data.m_GJJCriticalLevel = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.RoomData);
            }
        }

        //GJJ暴击伤害等级
        public long GJJCriticalDamageLevel
        {
            get => m_Data.m_GJJCriticalDamageLevel;
            set
            {
                m_Data.m_GJJCriticalDamageLevel = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.RoomData);
            }
        }

        //GJJ攻击速度等级
        public long GJJAtkSpeedLevel
        {
            get => m_Data.m_GJJAtkSpeedLevel;
            set
            {
                m_Data.m_GJJAtkSpeedLevel = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.RoomData);
            }
        }

        //GJJ双倍攻击等级
        public long GJJDoubleHitLevel
        {
            get => m_Data.m_GJJDoubleHitLevel;
            set
            {
                m_Data.m_GJJDoubleHitLevel = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.RoomData);
            }
        }

        //GJJ三倍攻击等级
        public long GJJTripletHitLevel
        {
            get => m_Data.m_GJJTripletHitLevel;
            set
            {
                m_Data.m_GJJTripletHitLevel = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.RoomData);
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
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.LevelData);
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
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.LevelData);
            }
        }

        //当前关卡状态
        public LevelState LevelState
        {
            get => m_Data.m_LevelState;
            set
            {
                m_Data.m_LevelState = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.LevelData);
            }
        }

        //当前放置奖励按钮点击时间戳
        public long BtnPlaceRewardClickTime
        {
            get => m_Data.m_BtnPlaceRewardClickTime;
            set
            {
                m_Data.m_BtnPlaceRewardClickTime = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.PlaceReward);
            }
        }

        // 放置奖励按钮显示时间
        public float BtnPlaceRewardShowTime
        {
            get => m_Data.m_BtnPlaceRewardShowTime;
            set
            {
                m_Data.m_BtnPlaceRewardShowTime = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.PlaceReward);
            }
        }

        // 放置奖励页面每日主动弹出时间
        public int PopPlaceRewardTime
        {
            get => m_Data.m_PopPlaceRewardTime;
            set
            {
                m_Data.m_PopPlaceRewardTime = value;
                EventManager.Call(LogicEvent.SyncUserData, SyncDataType.PlaceReward);
            }
        }

        #endregion
    }
}