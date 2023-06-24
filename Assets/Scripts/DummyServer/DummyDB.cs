using System;
using System.Collections.Generic;
using Framework.Helper;
using LitJson;
using Networks;
using UnityEngine;

namespace DummyServer
{
    /// <summary>
    /// 模拟服务器数据存储
    /// 本地存储在PlayerPrefs内
    /// </summary>
    [Serializable]
    public class DummyDB
    {
        #region 公用逻辑数据

        public bool m_IsFirstLogin = true; //是否是第一次登录
        public long m_LastGameDate; //上次登录时间

        #endregion

        #region 货币

        public string m_Coin; //游戏币
        public long m_Diamond; //钻石
        public int m_Oil; //原油
        public string m_Trophy; //战利品
        public int m_MushRoom; //角色升级蘑菇
        public int m_BreakOre; //角色突破矿石
        public int BreakTP; //英雄突破天赋点
        public int BreakTPTotal; //返还的英雄突破天赋点
        public long m_TechnologyPoint; //改造科技点

        #endregion

        #region GJJ数据

        public long m_GJJAtkLevel; //GJJ攻击力等级
        public long m_GJJHPLevel; //GJJ血量等级
        public long m_GJJHPRecoverLevel; //GJJ血量恢复速度等级
        public long m_GJJCriticalLevel; //GJJ暴击等级
        public long m_GJJCriticalDamageLevel; //GJJ暴击伤害等级
        public long m_GJJAtkSpeedLevel; //GJJ攻击速度等级
        public long m_GJJDoubleHitLevel; //GJJ两连攻击等级
        public long m_GJJTripletHitLevel; //GJJ三连攻击等级

        #endregion

        #region 通用设置等

        public bool m_AutoSkill; //自动技能
        public bool m_IsMusicOn; //音乐开关
        public bool m_IsSoundOn; //音效开关

        #endregion

        #region 关卡数据

        public long m_CurLevelID; //当前关卡ID
        public int m_CurLevelNode; //当前关卡节点(1-5 5 BOSS)
        public int m_CurLevelState; //关卡状态

        #endregion

        #region 放置奖励数据

        public long m_BtnPlaceRewardClickTime; // 放置奖励按钮点击时间戳
        public float m_BtnPlaceRewardShowTime; // 放置奖励按钮显示时间
        public int m_PopPlaceRewardTime; // 放置奖励页面每日主动弹出时间

        #endregion

        #region 伙伴, 装备, 技能数据

        public List<int> m_PartnerOnList; //伙伴上阵列表
        public List<GamePartnerData> m_PartnerList;

        public int m_WeaponOnID; //武器上阵ID
        public int m_ArmorOnID; //衣服上阵ID
        public List<GameEquipData> m_WeaponList;
        public List<GameEquipData> m_ArmorList;

        public List<int> m_SkillOnList; //技能上阵列表
        public List<GameSkillData> m_SkillList;

        #endregion

        #region 任务

        public int m_MainTaskCount; //主线任务完成次数
        public GameTaskData m_CurMainTask; //当前主线任务
        public List<GameTaskData> m_DailyTaskList; //每日任务列表

        #endregion

        #region 抽卡数据

        public GameShopCardData m_ShopSkillData;
        public GameShopCardData m_ShopPartnerData;
        public GameShopCardData m_ShopEquipData;

        #endregion

        #region 考古相关数据

        public GameMiningData m_MiningData; // 考古数据

        #endregion

        #region 副本数据

        public GameCopyData m_DiamondCopyData; //钻石副本数据
        public GameCopyData m_CoinCopyData; //金币副本数据
        public GameCopyOilData m_OilCopyData; //原油副本
        public GameCopyData m_TropyCopyData; //战利品副本
        public GameCopyData m_ReformCopyData; //改造副本

        #endregion

        #region 开放剧情数据

        public List<GameLockStoryData> m_LockStoryList; // 开放剧情数据

        #endregion

        #region 考古研究数据

        public List<GameResearchData> m_ResearchList; //考古研究数据
        public GameResearchEffectData m_ResearchEffectData; //考古属性加成数据

        #endregion

        #region 淬炼数据

        public List<GameQuenchingData> m_QuenchingList;

        #endregion


        #region 战利品数据

        public List<int> m_SpoilSlotsUnlockData; //战利品槽位解锁数据
        public List<SpoilSlotData> m_SpoilSlotsData; //战利品槽位数据
        public List<SpoilData> m_SpoilsData; //战利品数据
        public int m_SpoilDrawProgress; //战利品抽卡池当前进度
        public List<SpoilBreakthroughData> m_SpoilBreakthrough;

        #endregion


        #region 英雄数据

        public int m_RoleOnId; //英雄上阵ID
        public List<GameRoleData> m_RoleList; //英雄上阵列表
        public List<GameBreakTreeData> BreakTreeList; //英雄突破天赋树列表数据

        #endregion


        /// <summary>
        /// 初始化 玩家第一次进游戏的默认数据
        /// </summary>
        public void Init()
        {
            m_IsFirstLogin = true;
            m_LastGameDate = TimeHelper.GetUnixTimeStamp(); //首次进入默认当前日期

            m_Coin = "0";
            m_Diamond = 0;
            m_MushRoom = 0;
            m_BreakOre = 0;
            BreakTP = 0;
            BreakTPTotal = 0;

            m_GJJAtkLevel = 1;
            m_GJJHPLevel = 1;
            m_GJJHPRecoverLevel = 1;
            m_GJJCriticalLevel = 1;
            m_GJJCriticalDamageLevel = 1;
            m_GJJAtkSpeedLevel = 1;
            m_GJJDoubleHitLevel = 1;
            m_GJJTripletHitLevel = 1;

            m_CurLevelID = 1;
            m_CurLevelNode = 1;

            m_PartnerOnList = new List<int> { 0, 0, 0, 0, 0 };
            m_PartnerList = new List<GamePartnerData>(64);

            m_WeaponOnID = 0;
            m_ArmorOnID = 0;
            m_WeaponList = new List<GameEquipData>(64);
            m_ArmorList = new List<GameEquipData>(64);

            m_SkillOnList = new List<int> { 0, 0, 0, 0, 0, 0 };
            m_SkillList = new List<GameSkillData>(64);

            m_BtnPlaceRewardClickTime = TimeHelper.GetUnixTimeStamp();
            m_BtnPlaceRewardShowTime = 0;
            m_PopPlaceRewardTime = 1;

            DummyServerMgr.Ins.InitShop(this);
            DummyServerMgr.Ins.InitMining(this);

            DummyServerMgr.Ins.InitTask(this);
            DummyServerMgr.Ins.InitCopy(this);

            m_LockStoryList = new List<GameLockStoryData>(64);
            m_ResearchList = new List<GameResearchData>(64);
            DummyServerMgr.Ins.InitResearch(this);

            m_QuenchingList = new List<GameQuenchingData>(64);

            //战利品系统
            m_SpoilSlotsUnlockData = new List<int>(8);
            m_SpoilSlotsData = new List<SpoilSlotData>(8);
            m_SpoilsData = new List<SpoilData>(32);
            m_SpoilDrawProgress = 1;
            m_SpoilBreakthrough = new List<SpoilBreakthroughData>(32);

            m_RoleOnId = 7101;
            m_RoleList = new List<GameRoleData>(64);
            DummyServerMgr.Ins.InitRole(this);

            //英雄突破天赋树
            BreakTreeList = new List<GameBreakTreeData>(64);
            DummyServerMgr.Ins.InitRoleBreakTree(this);
        }

        #region 静态操作接口

        public static string GetSaveKey()
        {
            return "DummyDBSave";
        }

        public static void Save(DummyDB pData)
        {
            try
            {
                var _S = JsonMapper.ToJson(pData);
                PlayerPrefs.SetString(GetSaveKey(), _S);
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static DummyDB Load()
        {
            if (PlayerPrefs.HasKey(GetSaveKey()))
            {
                var _S = PlayerPrefs.GetString(GetSaveKey());
                var _D = JsonMapper.ToObject<DummyDB>(_S);
                return _D;
            }
            else
            {
                //没有存档, 创建一个新的
                var _Data = new DummyDB();
                return _Data;
            }
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteKey(GetSaveKey());
        }

        #endregion
    }
}