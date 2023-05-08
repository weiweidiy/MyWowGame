using System.Collections.Generic;
using Logic.Common;
using Logic.Data;

namespace Networks
{
    /*
     * 网络协议定义
     */

    /// <summary>
    /// 消息头
    /// </summary>
    public class MessageHead
    {
        public NetWorkMsgType m_MsgType; //消息类型
    }

    /// <summary>
    /// 请求登录
    /// </summary>
    public class C2S_Login : MessageHead
    {
        public C2S_Login()
        {
            m_MsgType = NetWorkMsgType.C2S_Login;
        }

        //TODO 账号ID
    }

    //元宝更新
    public class C2S_GMAccount : MessageHead
    {
        public C2S_GMAccount()
        {
            m_MsgType = NetWorkMsgType.C2S_GMAccount;
        }
    }

    public class S2C_DiamondUpdate : MessageHead
    {
        public S2C_DiamondUpdate()
        {
            m_MsgType = NetWorkMsgType.S2C_DiamondUpdate;
        }

        public long m_Diamond;
    }

    /// <summary>
    /// 返回玩家完整数据
    /// </summary>
    public class S2C_Login : MessageHead
    {
        public S2C_Login()
        {
            m_MsgType = NetWorkMsgType.S2C_Login;
        }

        #region 公用逻辑数据

        public bool m_IsFirstLogin = true; //是否是第一次登录
        public string m_LastGameDate; //上次登录时间

        #endregion

        #region 货币

        public string m_Coin; //游戏币
        public long m_Diamond; //金币
        public int m_Iron; //钢铁

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
        public int m_CurLevelNode; //当前关卡节点
        public int m_CurLevelState; //当前关卡状态

        #endregion

        #region 放置奖励数据

        public long m_BtnPlaceRewardClickTime; //放置奖励按钮点击时间戳
        public float m_BtnPlaceRewardShowTime; // 放置奖励按钮显示时间
        public string m_PopPlaceRewardTime; // 放置奖励页面每日主动弹出时间

        #endregion

        #region 考古相关数据

        public GameMiningData m_MiningData;

        #endregion

        #region 伙伴 装备 技能 引擎

        public List<int> m_PartnerOnList = new List<int>(5); //伙伴上阵列表
        public List<GamePartnerData> m_PartnerList = new List<GamePartnerData>(64);

        public int m_WeaponOnID; //武器上阵ID
        public int m_ClothesOnID; //衣服上阵ID
        public List<GameEquipData> m_WeaponList = new List<GameEquipData>(64);
        public List<GameEquipData> m_ArmorList = new List<GameEquipData>(64);

        public List<int> m_SkillOnList = new List<int>(); //技能上阵列表
        public List<GameSkillData> m_SkillList = new List<GameSkillData>(64);

        public int m_EngineOnId; // 引擎上阵ID
        public int m_EngineGetId; // 引擎获取ID
        public List<GameEngineData> m_EngineList = new List<GameEngineData>(64);

        #endregion

        #region 任务数据

        public int m_MainTaskCount; //主线任务完成次数
        public GameTaskData m_CurMainTask; //当前主线任务
        public List<GameTaskData> m_DailyTaskList = new List<GameTaskData>(32); //每日任务列表

        #endregion

        #region 抽卡数据

        public GameShopSkillData m_ShopSkillData;
        public GameShopPartnerData m_ShopPartnerData;
        public GameShopEquipData m_ShopEquipData;

        #endregion

        #region 副本数据

        public GameCopyData m_DiamondCopyData; //钻石副本数据
        public GameCopyData m_CoinCopyData; //金币副本数据

        #endregion

        #region 开放剧情数据

        public List<GameLockStoryData> m_LockStoryList = new List<GameLockStoryData>(64); // 开放剧情数据

        #endregion
    }

    /// <summary>
    /// 向服务器同步玩家部分数据 - 每个一段时间 自动同步一下
    /// 有单独Manager控制的数据已经同步过了 不需要在这里同步
    /// </summary>
    public class C2S_SyncPlayerData : MessageHead
    {
        public C2S_SyncPlayerData()
        {
            m_MsgType = NetWorkMsgType.C2S_SyncPlayerData;
        }

        public string m_Coin; //游戏币

        public long m_GJJAtkLevel; //GJJ攻击力等级
        public long m_GJJHPLevel; //GJJ血量等级
        public long m_GJJHPRecoverLevel; //GJJ血量恢复速度等级
        public long m_GJJCriticalLevel; //GJJ暴击等级
        public long m_GJJCriticalDamageLevel; //GJJ暴击伤害等级
        public long m_GJJAtkSpeedLevel; //GJJ攻击速度等级
        public long m_GJJDoubleHitLevel; //GJJ两连攻击等级
        public long m_GJJTripletHitLevel; //GJJ三连攻击等级

        public bool m_AutoSkill; //自动技能

        public long m_CurLevelID; //当前关卡ID
        public int m_CurLevelNode; //当前关卡节点
        public int m_CurLevelState; //当前关卡状态

        public long m_BtnPlaceRewardClickTime; //放置奖励按钮点击时间戳
        public float m_BtnPlaceRewardShowTime; // 放置奖励按钮显示时间
        public string m_PopPlaceRewardTime; // 放置奖励页面每日主动弹出时间

        public void Init()
        {
            m_Coin = GameDataManager.Ins.Coin.ToString();

            m_GJJAtkLevel = GameDataManager.Ins.GJJAtkLevel;
            m_GJJHPLevel = GameDataManager.Ins.GJJHPLevel;
            m_GJJHPRecoverLevel = GameDataManager.Ins.GJJHPRecoverLevel;
            m_GJJCriticalLevel = GameDataManager.Ins.GJJCriticalLevel;
            m_GJJCriticalDamageLevel = GameDataManager.Ins.GJJCriticalDamageLevel;
            m_GJJAtkSpeedLevel = GameDataManager.Ins.GJJAtkSpeedLevel;
            m_GJJDoubleHitLevel = GameDataManager.Ins.GJJDoubleHitLevel;
            m_GJJTripletHitLevel = GameDataManager.Ins.GJJTripletHitLevel;

            m_AutoSkill = GameDataManager.Ins.AutoSkill;

            m_CurLevelID = GameDataManager.Ins.CurLevelID;
            m_CurLevelNode = GameDataManager.Ins.CurLevelNode;
            m_CurLevelState = (int)GameDataManager.Ins.LevelState;

            m_BtnPlaceRewardClickTime = GameDataManager.Ins.BtnPlaceRewardClickTime;
            m_BtnPlaceRewardShowTime = GameDataManager.Ins.BtnPlaceRewardShowTime;
            m_PopPlaceRewardTime = GameDataManager.Ins.PopPlaceRewardTime;
        }
    }

    //装备-装备上
    public class C2S_EquipOn : MessageHead
    {
        public C2S_EquipOn()
        {
            m_MsgType = NetWorkMsgType.C2S_EquipOn;
        }

        public int m_EquipID;
        public ItemType m_Type;
    }

    public class S2C_EquipOn : MessageHead
    {
        public S2C_EquipOn()
        {
            m_MsgType = NetWorkMsgType.S2C_EquipOn;
        }

        public int m_EquipID;
        public ItemType m_Type;
    }

    //装备-装备下
    public class S2C_EquipOff : MessageHead
    {
        public S2C_EquipOff()
        {
            m_MsgType = NetWorkMsgType.S2C_EquipOff;
        }

        public int m_EquipID;
        public ItemType m_Type;
    }

    //装备-强化
    public class C2S_EquipIntensify : MessageHead
    {
        public C2S_EquipIntensify()
        {
            m_MsgType = NetWorkMsgType.C2S_EquipIntensify;
        }

        public bool m_IsAuto;
        public int m_EquipID;
        public ItemType m_Type;
    }

    public class S2C_EquipIntensify : MessageHead
    {
        public S2C_EquipIntensify()
        {
            m_MsgType = NetWorkMsgType.S2C_EquipIntensify;
        }

        public bool m_IsAuto;
        public ItemType m_Type;
        public List<GameEquipUpgradeData> m_EquipList;
    }

    //装备列表更新
    public class S2C_EquipListUpdate : MessageHead
    {
        public S2C_EquipListUpdate()
        {
            m_MsgType = NetWorkMsgType.S2C_EquipListUpdate;
        }

        public List<GameEquipData> m_WeaponList;
        public List<GameEquipData> m_ArmorList;
    }

    //---------------------------------------------------------------------------
    //伙伴-上阵
    public class C2S_PartnerOn : MessageHead
    {
        public C2S_PartnerOn()
        {
            m_MsgType = NetWorkMsgType.C2S_PartnerOn;
        }

        public int m_PartnerID;
    }

    public class S2C_PartnerOn : MessageHead
    {
        public S2C_PartnerOn()
        {
            m_MsgType = NetWorkMsgType.S2C_PartnerOn;
        }

        public int m_PartnerID;
        public int m_Index;
    }

    //伙伴-下阵
    public class C2S_PartnerOff : MessageHead
    {
        public C2S_PartnerOff()
        {
            m_MsgType = NetWorkMsgType.C2S_PartnerOff;
        }

        public int m_PartnerID;
    }

    public class S2C_PartnerOff : MessageHead
    {
        public S2C_PartnerOff()
        {
            m_MsgType = NetWorkMsgType.S2C_PartnerOff;
        }

        public int m_PartnerID;
        public int m_Index;
    }

    public class C2S_PartnerIntensify : MessageHead
    {
        public C2S_PartnerIntensify()
        {
            m_MsgType = NetWorkMsgType.C2S_PartnerIntensify;
        }

        public bool m_IsAuto;
        public int m_PartnerID;
    }

    public class S2C_PartnerIntensify : MessageHead
    {
        public S2C_PartnerIntensify()
        {
            m_MsgType = NetWorkMsgType.S2C_PartnerIntensify;
        }

        public bool m_IsAuto;
        public List<GamePartnerUpgradeData> m_PartnerList;
    }

    //伙伴列表更新
    public class S2C_PartnerListUpdate : MessageHead
    {
        public S2C_PartnerListUpdate()
        {
            m_MsgType = NetWorkMsgType.S2C_PartnerListUpdate;
        }

        public List<GamePartnerData> m_PartnerList;
    }

    //---------------------------------------------------------------------------
    //技能-装备上
    public class C2S_SkillOn : MessageHead
    {
        public C2S_SkillOn()
        {
            m_MsgType = NetWorkMsgType.C2S_SkillOn;
        }

        public int m_SkillID;
    }

    public class S2C_SkillOn : MessageHead
    {
        public S2C_SkillOn()
        {
            m_MsgType = NetWorkMsgType.S2C_SkillOn;
        }

        public int m_SkillID;
        public int m_Index;
    }

    //技能-装备下
    public class C2S_SkillOff : MessageHead
    {
        public C2S_SkillOff()
        {
            m_MsgType = NetWorkMsgType.C2S_SkillOff;
        }

        public int m_SkillID;
    }

    public class S2C_SkillOff : MessageHead
    {
        public S2C_SkillOff()
        {
            m_MsgType = NetWorkMsgType.S2C_SkillOff;
        }

        public int m_SkillID;
        public int m_Index;
    }

    //技能-强化
    public class C2S_SkillIntensify : MessageHead
    {
        public C2S_SkillIntensify()
        {
            m_MsgType = NetWorkMsgType.C2S_SkillIntensify;
        }

        public bool m_IsAuto;
        public int m_SkillID;
    }

    public class S2C_SkillIntensify : MessageHead
    {
        public S2C_SkillIntensify()
        {
            m_MsgType = NetWorkMsgType.S2C_SkillIntensify;
        }

        public bool m_IsAuto;
        public List<GameSkillUpgradeData> m_SkillList;
    }

    //技能列表更新
    public class S2C_SkillListUpdate : MessageHead
    {
        public S2C_SkillListUpdate()
        {
            m_MsgType = NetWorkMsgType.S2C_SkillListUpdate;
        }

        public List<GameSkillData> m_SkillList;
    }

    //引擎获取
    public class S2C_EngineGet : MessageHead
    {
        public S2C_EngineGet()
        {
            m_MsgType = NetWorkMsgType.S2C_EngineGet;
        }

        public List<GameEngineData> m_EngineList;
        public int m_LastEngineGetId;
    }

    //引擎强化
    public class C2S_EngineIntensify : MessageHead
    {
        public C2S_EngineIntensify()
        {
            m_MsgType = NetWorkMsgType.C2S_EngineIntensify;
        }

        public int m_EngineIntensifyId;
    }

    public class S2C_EngineIntensify : MessageHead
    {
        public S2C_EngineIntensify()
        {
            m_MsgType = NetWorkMsgType.S2C_EngineIntensify;
        }

        public int m_EngineIntensifyId;
        public int m_EngineLevel;
    }

    //引擎分解
    public class C2S_EngineRemove : MessageHead
    {
        public C2S_EngineRemove()
        {
            m_MsgType = NetWorkMsgType.C2S_EngineRemove;
        }

        public int m_EngineRemoveId;
    }

    //引擎材料更新
    public class S2C_EngineIronUpdate : MessageHead
    {
        public S2C_EngineIronUpdate()
        {
            m_MsgType = NetWorkMsgType.S2C_EngineIronUpdate;
        }

        public int m_Iron;
    }

    //引擎-装备上
    public class C2S_EngineOn : MessageHead
    {
        public C2S_EngineOn()
        {
            m_MsgType = NetWorkMsgType.C2S_EngineOn;
        }

        public int m_EngineOnId;
    }

    public class S2C_EngineOn : MessageHead
    {
        public S2C_EngineOn()
        {
            m_MsgType = NetWorkMsgType.S2C_EngineOn;
        }

        public int m_EngineOnId;
    }

    //引擎-装备下
    public class C2S_EngineOff : MessageHead
    {
        public C2S_EngineOff()
        {
            m_MsgType = NetWorkMsgType.C2S_EngineOff;
        }

        public int m_EngineOffId;
    }

    public class S2C_EngineOff : MessageHead
    {
        public S2C_EngineOff()
        {
            m_MsgType = NetWorkMsgType.S2C_EngineOff;
        }

        public int m_EngineOffId;
    }

    //抽卡
    public class C2S_DrawCard : MessageHead
    {
        public C2S_DrawCard()
        {
            m_MsgType = NetWorkMsgType.C2S_DrawCard;
        }

        public int m_DrawCardType;
        public int m_DrawCardCostType;
    }

    public class S2C_DrawCard : MessageHead
    {
        public S2C_DrawCard()
        {
            m_MsgType = NetWorkMsgType.S2C_DrawCard;
        }

        public int m_DrawCardType;
        public List<int> m_List;
    }

    public class C2S_UpdateDrawCardData : MessageHead
    {
        public C2S_UpdateDrawCardData()
        {
            m_MsgType = NetWorkMsgType.C2S_UpdateDrawCardData;
        }

        public int m_DrawCardType;
    }

    public class S2C_UpdateDrawCardData : MessageHead
    {
        public S2C_UpdateDrawCardData()
        {
            m_MsgType = NetWorkMsgType.S2C_UpdateDrawCardData;
        }

        public int m_DrawCardType;
        public int m_DrawCardId;
        public int m_DrawCardLevel;
        public int m_DarwCardExp;
        public int m_DrawCardTotalExp;
    }

    // 奖励
    public class C2S_MiningReward : MessageHead
    {
        public C2S_MiningReward()
        {
            m_MsgType = NetWorkMsgType.C2S_MiningReward;
        }

        public MiningType m_TreasureType;
    }

    public class S2C_MiningReward : MessageHead
    {
        public S2C_MiningReward()
        {
            m_MsgType = NetWorkMsgType.S2C_MiningReward;
        }

        public MiningType m_TreasureType;
        public int m_RewardId;
        public int m_RewardCount;
    }

    public class C2S_PlaceReward : MessageHead
    {
        public C2S_PlaceReward()
        {
            m_MsgType = NetWorkMsgType.C2S_PlaceReward;
        }

        public int m_PlaceRewardCount;
    }

    public class S2C_PlaceReward : MessageHead
    {
        public S2C_PlaceReward()
        {
            m_MsgType = NetWorkMsgType.S2C_PlaceReward;
        }

        public List<int> m_PlaceRewardId;
        public List<int> m_PlaceRewardCount;
    }

    public class C2S_GetPlaceReward : MessageHead
    {
        public C2S_GetPlaceReward()
        {
            m_MsgType = NetWorkMsgType.C2S_GetPlaceReward;
        }
    }

    public class C2S_CommonReward : MessageHead
    {
        public C2S_CommonReward()
        {
            m_MsgType = NetWorkMsgType.C2S_CommonReward;
        }
    }

    public class S2C_CommonReward : MessageHead
    {
        public S2C_CommonReward()
        {
            m_MsgType = NetWorkMsgType.S2C_CommonReward;
        }

        public List<int> m_CommonRewardId;
        public List<int> m_CommonRewardCount;
    }

    // 考古
    public class C2S_UpdateMiningData : MessageHead
    {
        public C2S_UpdateMiningData()
        {
            m_MsgType = NetWorkMsgType.C2S_UpdateMiningData;
        }

        public int m_MiningDataType;
        public int m_UpdateType;
    }

    public class S2C_UpdateMiningData : MessageHead
    {
        public S2C_UpdateMiningData()
        {
            m_MsgType = NetWorkMsgType.S2C_UpdateMiningData;
        }

        public int m_MiningDataType;
        public GameMiningData m_MiningData;
    }

    //任务
    public class C2S_UpdateTaskProcess : MessageHead
    {
        public C2S_UpdateTaskProcess()
        {
            m_MsgType = NetWorkMsgType.C2S_UpdateTaskProcess;
        }

        public int m_TaskID;
        public long m_Process;
        public bool m_IsMain;
    }

    public class S2C_UpdateTaskState : MessageHead
    {
        public S2C_UpdateTaskState()
        {
            m_MsgType = NetWorkMsgType.S2C_UpdateTaskState;
        }

        public int m_TaskID;
        public int m_TaskState;
        public bool m_IsMain;
    }

    public class S2C_UpdateDailyTask : MessageHead
    {
        public S2C_UpdateDailyTask()
        {
            m_MsgType = NetWorkMsgType.S2C_UpdateDailyTask;
        }

        public int m_TaskID;
    }

    public class C2S_TaskGetReward : MessageHead
    {
        public C2S_TaskGetReward()
        {
            m_MsgType = NetWorkMsgType.C2S_TaskGetReward;
        }

        public int m_TaskID;
        public bool m_IsMain;
    }

    public class S2C_UpdateMainTask : MessageHead
    {
        public S2C_UpdateMainTask()
        {
            m_MsgType = NetWorkMsgType.S2C_UpdateMainTask;
        }

        public GameTaskData m_TaskData;
        public int m_MainTaskCount;
    }

    public class C2S_RequestDailyTaskList : MessageHead
    {
        public C2S_RequestDailyTaskList()
        {
            m_MsgType = NetWorkMsgType.C2S_RequestDailyTaskList;
        }
    }

    public class S2C_RequestDailyTaskList : MessageHead
    {
        public S2C_RequestDailyTaskList()
        {
            m_MsgType = NetWorkMsgType.S2C_RequestDailyTaskList;
        }

        public List<GameTaskData> m_DailyTaskList;
    }

    //副本
    public class C2S_EnterCopy : MessageHead
    {
        public C2S_EnterCopy()
        {
            m_MsgType = NetWorkMsgType.C2S_EnterCopy;
        }

        public int m_LevelType;
    }

    public class S2C_EnterCopy : MessageHead
    {
        public S2C_EnterCopy()
        {
            m_MsgType = NetWorkMsgType.S2C_EnterCopy;
        }

        public int m_LevelType;
    }

    public class C2S_ExitCopy : MessageHead
    {
        public C2S_ExitCopy()
        {
            m_MsgType = NetWorkMsgType.C2S_ExitCopy;
        }

        public int m_LevelType;
    }

    public class S2C_ExitCopy : MessageHead
    {
        public S2C_ExitCopy()
        {
            m_MsgType = NetWorkMsgType.S2C_ExitCopy;
        }

        public int m_LevelType;
        public int m_Level;
        public long m_RewardCount;
        public int m_KeyCount;
    }

    public class C2S_UpdateCopyKeyCount : MessageHead
    {
        public C2S_UpdateCopyKeyCount()
        {
            m_MsgType = NetWorkMsgType.C2S_UpdateCopyKeyCount;
        }
    }

    public class S2C_UpdateCopyKeyCount : MessageHead
    {
        public S2C_UpdateCopyKeyCount()
        {
            m_MsgType = NetWorkMsgType.S2C_UpdateCopyKeyCount;
        }

        public int m_DiamondKeyCount;
        public int m_CoinKeyCount;
    }

    //开放剧情系统
    public class C2S_UpdateLockStoryData : MessageHead
    {
        public C2S_UpdateLockStoryData()
        {
            m_MsgType = NetWorkMsgType.C2S_UpdateLockStoryData;
        }

        public GameLockStoryData m_LockStoryData;
    }
}