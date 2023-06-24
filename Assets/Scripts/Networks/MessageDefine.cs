using System.Collections.Generic;
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
        public NetWorkMsgType MsgType; //消息类型
        public long MsgID; //消息ID
    }

    /// <summary>
    /// 服务器推送的错误 通常是比较严重的问题
    /// </summary>
    public class S2C_CommonError : MessageHead
    {
        public S2C_CommonError()
        {
            MsgType = NetWorkMsgType.S2C_CommonError;
        }

        public int ErrorCode;
    }

    /// <summary>
    /// 请求登录
    /// </summary>
    public class C2S_Login : MessageHead
    {
        public C2S_Login()
        {
            MsgType = NetWorkMsgType.C2S_Login;
        }

        //账号ID
        public string AccountID;
    }

    public class C2S_GMAccount : MessageHead
    {
        public C2S_GMAccount()
        {
            MsgType = NetWorkMsgType.C2S_GMCommand;
        }
    }

    //游戏币变化
    public class C2S_SyncCoin : MessageHead
    {
        public C2S_SyncCoin()
        {
            MsgType = NetWorkMsgType.C2S_SyncCoin;
        }

        public string Coin;
    }

    //元宝更新
    public class S2C_DiamondUpdate : MessageHead
    {
        public S2C_DiamondUpdate()
        {
            MsgType = NetWorkMsgType.S2C_DiamondUpdate;
        }

        public long Diamond;
    }

    public class S2C_OilUpdate : MessageHead
    {
        public S2C_OilUpdate()
        {
            MsgType = NetWorkMsgType.S2C_OilUpdate;
        }

        public int Oil;
    }

    //public class S2C_TechnologyPointUpdate : MessageHead
    //{
    //    public S2C_TechnologyPointUpdate()
    //    {
    //        MsgType = NetWorkMsgType.S2C_TechnologyPointUpdate;
    //    }

    //    public long TechnologyPoint;
    //}


    public class C2S_SyncTrophy : MessageHead
    {
        public C2S_SyncTrophy()
        {
            MsgType = NetWorkMsgType.C2S_SyncTrophy;
        }

        public string Trophy;
    }
    
    public class S2C_TecPointUpdate : MessageHead
    {
        public S2C_TecPointUpdate()
        {
            MsgType = NetWorkMsgType.S2C_TecPointUpdate;
        }

        public int TecPoint;
    }

    /// <summary>
    /// 返回玩家完整数据
    /// </summary>
    public class S2C_Login : MessageHead
    {
        public S2C_Login()
        {
            MsgType = NetWorkMsgType.S2C_Login;
        }

        #region 公用逻辑数据

        public ServerTimes ST; //服务器时间
        public long LastGameDate; //上次下线时间

        #endregion

        #region 货币

        public string Coin; //游戏币
        public long Diamond; //金币
        public int Oil; //原油
        public string Trophy; //战利品
        public int MushRoom; //英雄升级蘑菇
        public int BreakOre; //英雄突破矿石
        public int BreakTP; //英雄突破天赋点
        public int TecPoint; //引擎升级科技点

        #endregion

        #region GJJ数据

        public long GJJAtkLevel; //GJJ攻击力等级
        public long GJJHPLevel; //GJJ血量等级
        public long GJJHPRecoverLevel; //GJJ血量恢复速度等级
        public long GJJCriticalLevel; //GJJ暴击等级
        public long GJJCriticalDamageLevel; //GJJ暴击伤害等级
        public long GJJAtkSpeedLevel; //GJJ攻击速度等级
        public long GJJDoubleHitLevel; //GJJ两连攻击等级
        public long GJJTripletHitLevel; //GJJ三连攻击等级

        #endregion

        #region 通用设置等

        public bool AutoSkill; //自动技能
        public bool IsMusicOn; //音乐开关
        public bool IsSoundOn; //音效开关

        #endregion

        #region 关卡数据

        public long CurLevelID; //当前关卡ID
        public int CurLevelNode; //当前关卡节点
        public int CurLevelState; //当前关卡状态

        #endregion

        #region 放置奖励数据

        public long PlaceRewardClickTime; //放置奖励按钮点击时间戳
        public float PlaceRewardShowTime; // 放置奖励按钮显示时间
        public int PlaceRewardPopTime; // 放置奖励页面每日主动弹出时间

        #endregion

        #region 考古相关数据

        public GameMiningData MiningData;

        #endregion

        #region 伙伴 装备 技能

        public List<int> PartnerOnList = new List<int>(5); //伙伴上阵列表
        public List<GamePartnerData> PartnerList = new List<GamePartnerData>(64);

        public int WeaponOnID; //武器上阵ID
        public int ArmorOnID; //衣服上阵ID
        public List<GameEquipData> WeaponList = new List<GameEquipData>(64);
        public List<GameEquipData> ArmorList = new List<GameEquipData>(64);

        public List<int> SkillOnList = new List<int>(); //技能上阵列表
        public List<GameSkillData> SkillList = new List<GameSkillData>(64);

        #endregion

        #region 任务数据

        public int MainTaskCount; //主线任务完成次数
        public GameTaskData CurMainTask; //当前主线任务
        public List<GameTaskData> DailyTaskList = new List<GameTaskData>(32); //每日任务列表

        #endregion

        #region 商城数据

        public GameShopCardData ShopSkillData;
        public GameShopCardData ShopPartnerData;
        public GameShopCardData ShopEquipData;
        public GameShopData ShopData;

        #endregion

        #region 副本数据

        public GameCopyData DiamondCopyData; //钻石副本数据
        public GameCopyData CoinCopyData; //金币副本数据
        public GameCopyOilData OilCopyData;
        public GameCopyData TrophyCopyData; //功勋副本
        public GameCopyData ReformCopyData; //改造副本

        #endregion

        #region 开放剧情数据

        public List<GameLockStoryData> LockStoryList = new List<GameLockStoryData>(64); // 开放剧情数据

        #endregion

        #region 考古研究数据

        public List<GameResearchData> ResearchList = new List<GameResearchData>(64); //考古研究数据
        public GameResearchEffectData ResearchEffectData; //考古属性加成数据

        #endregion

        #region 淬炼数据

        public List<GameQuenchingData> QuenchingList = new List<GameQuenchingData>(64);

        #endregion
        
        #region 战利品数据

        public List<int> SpoilSlotsUnlockData = new List<int>(8); //战利品槽位解锁数据
        public List<SpoilSlotData> SpoilSlotsData = new List<SpoilSlotData>(8); //战利品槽位数据
        public List<SpoilData> SpoilsData = new List<SpoilData>(32); //战利品数据
        public int SpoilDrawProgress; //战利品抽卡池当前进度
        public List<SpoilBreakthroughData> SpoilBreakthroughData = new List<SpoilBreakthroughData>(32); //战利品突破数据

        #endregion
        
        #region 英雄数据

        public int RoleOnId; //英雄上阵ID
        public List<GameRoleData> RoleList = new List<GameRoleData>(64); //英雄上阵列表
        public List<GameBreakTreeData> RoleBreakTree = new List<GameBreakTreeData>(64); //英雄突破天赋树列表数据

        #endregion

        #region 引擎

        public GameEngineData EngineData;       //引擎数据
        public List<GameEnginePartData> EngineParts = new List<GameEnginePartData>(); //引擎部件列表

        #endregion
    }

    public class C2S_ST : MessageHead
    {
        public C2S_ST()
        {
            MsgType = NetWorkMsgType.C2S_ST;
        }
    }

    public class S2C_ST : MessageHead
    {
        public ServerTimes ST; //服务器时间
    }

    /// <summary>
    /// 向服务器同步玩家部分数据 - 每个一段时间 自动同步一下
    /// 有单独Manager控制的数据已经同步过了 不需要在这里同步
    /// </summary>
    public class C2S_SyncRoomData : MessageHead
    {
        public C2S_SyncRoomData()
        {
            MsgType = NetWorkMsgType.C2S_SyncRoomData;
        }

        public long GJJAtkLevel; //GJJ攻击力等级
        public long GJJHPLevel; //GJJ血量等级
        public long GJJHPRecoverLevel; //GJJ血量恢复速度等级
        public long GJJCriticalLevel; //GJJ暴击等级
        public long GJJCriticalDamageLevel; //GJJ暴击伤害等级
        public long GJJAtkSpeedLevel; //GJJ攻击速度等级
        public long GJJDoubleHitLevel; //GJJ两连攻击等级
        public long GJJTripletHitLevel; //GJJ三连攻击等级

        public void Init()
        {
            GJJAtkLevel = GameDataManager.Ins.GJJAtkLevel;
            GJJHPLevel = GameDataManager.Ins.GJJHPLevel;
            GJJHPRecoverLevel = GameDataManager.Ins.GJJHPRecoverLevel;
            GJJCriticalLevel = GameDataManager.Ins.GJJCriticalLevel;
            GJJCriticalDamageLevel = GameDataManager.Ins.GJJCriticalDamageLevel;
            GJJAtkSpeedLevel = GameDataManager.Ins.GJJAtkSpeedLevel;
            GJJDoubleHitLevel = GameDataManager.Ins.GJJDoubleHitLevel;
            GJJTripletHitLevel = GameDataManager.Ins.GJJTripletHitLevel;
        }
    }

    public class C2S_SyncSettingData : MessageHead
    {
        public C2S_SyncSettingData()
        {
            MsgType = NetWorkMsgType.C2S_SyncSettingData;
        }

        public bool AutoSkill; //自动技能
        public bool IsMusicOn; //音乐开关
        public bool IsSoundOn; //音效开关

        public void Init()
        {
            AutoSkill = GameDataManager.Ins.AutoSkill;
            IsMusicOn = GameDataManager.Ins.Music;
            IsSoundOn = GameDataManager.Ins.Sound;
        }
    }

    public class C2S_SyncLevelData : MessageHead
    {
        public C2S_SyncLevelData()
        {
            MsgType = NetWorkMsgType.C2S_SyncLevelData;
        }

        public long CurLevelID; //当前关卡ID
        public int CurLevelNode; //当前关卡节点
        public int CurLevelState; //当前关卡状态

        public void Init()
        {
            CurLevelID = GameDataManager.Ins.CurLevelID;
            CurLevelNode = GameDataManager.Ins.CurLevelNode;
            CurLevelState = (int)GameDataManager.Ins.LevelState;
        }
    }

    public class C2S_SyncPlaceRewardData : MessageHead
    {
        public C2S_SyncPlaceRewardData()
        {
            MsgType = NetWorkMsgType.C2S_SyncPlaceRewardData;
        }

        public long PlaceRewardClickTime; //放置奖励按钮点击时间戳
        public float PlaceRewardShowTime; // 放置奖励按钮显示时间
        public int PlaceRewardPopTime; // 放置奖励页面每日主动弹出时间

        public void Init()
        {
            PlaceRewardClickTime = GameDataManager.Ins.BtnPlaceRewardClickTime;
            PlaceRewardShowTime = GameDataManager.Ins.BtnPlaceRewardShowTime;
            PlaceRewardPopTime = GameDataManager.Ins.PopPlaceRewardTime;
        }
    }

    //装备-装备上
    public class C2S_EquipOn : MessageHead
    {
        public C2S_EquipOn()
        {
            MsgType = NetWorkMsgType.C2S_EquipOn;
        }

        public int EquipID;
        public int Type;
    }

    public class S2C_EquipOn : MessageHead
    {
        public S2C_EquipOn()
        {
            MsgType = NetWorkMsgType.S2C_EquipOn;
        }

        public int EquipID;
        public int Type;
    }

    //装备-装备下
    public class S2C_EquipOff : MessageHead
    {
        public S2C_EquipOff()
        {
            MsgType = NetWorkMsgType.S2C_EquipOff;
        }

        public int EquipID;
        public int Type;
    }

    //装备-强化
    public class C2S_EquipIntensify : MessageHead
    {
        public C2S_EquipIntensify()
        {
            MsgType = NetWorkMsgType.C2S_EquipIntensify;
        }

        public bool IsAuto;
        public int EquipID;
        public int Type;
    }

    public class S2C_EquipIntensify : MessageHead
    {
        public S2C_EquipIntensify()
        {
            MsgType = NetWorkMsgType.S2C_EquipIntensify;
        }

        public bool IsAuto;
        public int Type;
        public List<GameEquipUpgradeData> EquipList;
        public List<GameComposeData> ComposeList;
    }

    //装备列表更新
    public class S2C_EquipListUpdate : MessageHead
    {
        public S2C_EquipListUpdate()
        {
            MsgType = NetWorkMsgType.S2C_EquipListUpdate;
        }

        public List<GameEquipData> WeaponList;
        public List<GameEquipData> ArmorList;
    }

    //单次合成
    public class C2S_EquipCompose : MessageHead
    {
        public C2S_EquipCompose()
        {
            MsgType = NetWorkMsgType.C2S_EquipCompose;
        }

        public int EquipID;
        public int Type;
    }

    public class S2S_EquipCompose : MessageHead
    {
        public int Type;
        public int FromID; //合成者ID
        public int FromCount; //合成后数量
        public int ToID; //合成后ID
        public int ToCount; //合成后数量
    }

    //---------------------------------------------------------------------------
    //伙伴-上阵
    public class C2S_PartnerOn : MessageHead
    {
        public C2S_PartnerOn()
        {
            MsgType = NetWorkMsgType.C2S_PartnerOn;
        }

        public int PartnerID;
    }

    public class S2C_PartnerOn : MessageHead
    {
        public S2C_PartnerOn()
        {
            MsgType = NetWorkMsgType.S2C_PartnerOn;
        }

        public int PartnerID;
        public int Index;
    }

    //伙伴-下阵
    public class C2S_PartnerOff : MessageHead
    {
        public C2S_PartnerOff()
        {
            MsgType = NetWorkMsgType.C2S_PartnerOff;
        }

        public int PartnerID;
    }

    public class S2C_PartnerOff : MessageHead
    {
        public S2C_PartnerOff()
        {
            MsgType = NetWorkMsgType.S2C_PartnerOff;
        }

        public int PartnerID;
        public int Index;
    }

    public class C2S_PartnerIntensify : MessageHead
    {
        public C2S_PartnerIntensify()
        {
            MsgType = NetWorkMsgType.C2S_PartnerIntensify;
        }

        public bool IsAuto;
        public int PartnerID;
    }

    public class S2C_PartnerIntensify : MessageHead
    {
        public S2C_PartnerIntensify()
        {
            MsgType = NetWorkMsgType.S2C_PartnerIntensify;
        }

        public bool IsAuto;
        public List<GamePartnerUpgradeData> PartnerList;
        public List<GameComposeData> ComposeList;
    }

    //伙伴列表更新
    public class S2C_PartnerListUpdate : MessageHead
    {
        public S2C_PartnerListUpdate()
        {
            MsgType = NetWorkMsgType.S2C_PartnerListUpdate;
        }

        public List<GamePartnerData> PartnerList;
    }

    //单次合成
    public class C2S_PartnerCompose : MessageHead
    {
        public C2S_PartnerCompose()
        {
            MsgType = NetWorkMsgType.C2S_PartnerCompose;
        }

        public int PartnerID;
    }

    public class S2S_PartnerCompose : MessageHead
    {
        public int FromID; //合成者ID
        public int FromCount; //合成后数量
        public int ToID; //合成后ID
        public int ToCount; //合成后数量
    }

    //---------------------------------------------------------------------------
    //技能-上
    public class C2S_SkillOn : MessageHead
    {
        public C2S_SkillOn()
        {
            MsgType = NetWorkMsgType.C2S_SkillOn;
        }

        public int SkillID;
    }

    public class S2C_SkillOn : MessageHead
    {
        public S2C_SkillOn()
        {
            MsgType = NetWorkMsgType.S2C_SkillOn;
        }

        public int SkillID;
        public int Index;
    }

    //技能-下
    public class C2S_SkillOff : MessageHead
    {
        public C2S_SkillOff()
        {
            MsgType = NetWorkMsgType.C2S_SkillOff;
        }

        public int SkillID;
    }

    public class S2C_SkillOff : MessageHead
    {
        public S2C_SkillOff()
        {
            MsgType = NetWorkMsgType.S2C_SkillOff;
        }

        public int SkillID;
        public int Index;
    }

    //技能-强化
    public class C2S_SkillIntensify : MessageHead
    {
        public C2S_SkillIntensify()
        {
            MsgType = NetWorkMsgType.C2S_SkillIntensify;
        }

        public bool IsAuto;
        public int SkillID;
    }

    public class S2C_SkillIntensify : MessageHead
    {
        public S2C_SkillIntensify()
        {
            MsgType = NetWorkMsgType.S2C_SkillIntensify;
        }

        public bool IsAuto;
        public List<GameSkillUpgradeData> SkillList;
        public List<GameComposeData> ComposeList;
    }

    //技能列表更新
    public class S2C_SkillListUpdate : MessageHead
    {
        public S2C_SkillListUpdate()
        {
            MsgType = NetWorkMsgType.S2C_SkillListUpdate;
        }

        public List<GameSkillData> SkillList;
    }

    //单次合成
    public class C2S_SkillCompose : MessageHead
    {
        public C2S_SkillCompose()
        {
            MsgType = NetWorkMsgType.C2S_SkillCompose;
        }

        public int SkillID;
    }

    public class S2S_SkillCompose : MessageHead
    {
        public int FromID; //合成者ID
        public int FromCount; //合成后数量
        public int ToID; //合成后ID
        public int ToCount; //合成后数量
    }

    //抽卡
    public class C2S_DrawCard : MessageHead
    {
        public C2S_DrawCard()
        {
            MsgType = NetWorkMsgType.C2S_DrawCard;
        }

        public int DrawCardType;
        public int DrawCardCostType;
    }

    public class S2C_DrawCard : MessageHead
    {
        public S2C_DrawCard()
        {
            MsgType = NetWorkMsgType.S2C_DrawCard;
        }

        public int DrawCardType;
        public List<int> List;
    }

    public class C2S_UpdateDrawCardData : MessageHead
    {
        public C2S_UpdateDrawCardData()
        {
            MsgType = NetWorkMsgType.C2S_UpdateDrawCardData;
        }

        public int DrawCardType;
    }

    public class S2C_UpdateDrawCardData : MessageHead
    {
        public S2C_UpdateDrawCardData()
        {
            MsgType = NetWorkMsgType.S2C_UpdateDrawCardData;
        }

        public int DrawCardType;
        public int DrawCardId;
        public int DrawCardLevel;
        public int DrawCardExp;
        public int DrawCardTotalExp;
    }

    public class C2S_ShopBuy : MessageHead //商店购买
    {
        public C2S_ShopBuy()
        {
            MsgType = NetWorkMsgType.C2S_ShopBuy;
        }

        public int ID;
        public int Type; //ShopType
    }

    public class S2C_ShopBuyOrder : MessageHead
    {
        public string OrderId;
    }

    public class S2C_ShopBuy : MessageHead
    {
        public bool FirstBuy; //首充
        public GameShopBuyData Data; //本次数据
    }

    // 奖励
    public class C2S_MiningReward : MessageHead
    {
        public C2S_MiningReward()
        {
            MsgType = NetWorkMsgType.C2S_MiningReward;
        }

        public int TreasureType;
    }

    public class S2C_MiningReward : MessageHead
    {
        public S2C_MiningReward()
        {
            MsgType = NetWorkMsgType.S2C_MiningReward;
        }

        public int TreasureType;
        public int RewardId;
        public int RewardCount;
    }

    public class C2S_PlaceReward : MessageHead
    {
        public C2S_PlaceReward()
        {
            MsgType = NetWorkMsgType.C2S_PlaceReward;
        }

        public int PlaceRewardCount;
    }

    public class S2C_PlaceReward : MessageHead
    {
        public S2C_PlaceReward()
        {
            MsgType = NetWorkMsgType.S2C_PlaceReward;
        }

        public List<int> PlaceRewardId;
        public List<int> PlaceRewardCount;
    }

    public class C2S_GetPlaceReward : MessageHead
    {
        public C2S_GetPlaceReward()
        {
            MsgType = NetWorkMsgType.C2S_GetPlaceReward;
        }
    }

    /// <summary>
    /// 通知客户端获得了原油副本奖励
    /// </summary>
    public class S2C_OilCopyReward : MessageHead
    {
        public S2C_OilCopyReward()
        {
            MsgType = NetWorkMsgType.S2C_OilCopyReward;
        }

        public int Oil;
        public List<int> LstRewardId;
        public List<int> LstRewardCount;
    }

    public class S2C_ReformCopyReward : MessageHead
    {
        public S2C_ReformCopyReward()
        {
            MsgType = NetWorkMsgType.S2C_ReformCopyReward;
        }

        public long TechnologyPoint;
        public List<GameEnginePartData> LstCylinders;
    }

    public class C2S_CommonReward : MessageHead
    {
        public C2S_CommonReward()
        {
            MsgType = NetWorkMsgType.C2S_CommonReward;
        }
    }

    public class S2C_CommonReward : MessageHead
    {
        public S2C_CommonReward()
        {
            MsgType = NetWorkMsgType.S2C_CommonReward;
        }

        public List<int> m_CommonRewardId;
        public List<int> m_CommonRewardCount;
    }

    // 考古
    public class C2S_UpdateMiningData : MessageHead
    {
        public C2S_UpdateMiningData()
        {
            MsgType = NetWorkMsgType.C2S_UpdateMiningData;
        }

        public int MiningDataType;
        public int UpdateType;
    }

    public class S2C_UpdateMiningData : MessageHead
    {
        public S2C_UpdateMiningData()
        {
            MsgType = NetWorkMsgType.S2C_UpdateMiningData;
        }

        public int MiningDataType;
        public GameMiningData MiningData;
    }

    //任务
    public class C2S_UpdateTaskProcess : MessageHead
    {
        public C2S_UpdateTaskProcess()
        {
            MsgType = NetWorkMsgType.C2S_UpdateTaskProcess;
        }

        public int TaskID;
        public long Process;
        public bool IsMain;
    }

    public class S2C_UpdateTaskState : MessageHead
    {
        public S2C_UpdateTaskState()
        {
            MsgType = NetWorkMsgType.S2C_UpdateTaskState;
        }

        public int TaskID;
        public int TaskState;
        public bool IsMain;
    }

    public class S2C_UpdateDailyTask : MessageHead
    {
        public S2C_UpdateDailyTask()
        {
            MsgType = NetWorkMsgType.S2C_UpdateDailyTask;
        }

        public int TaskID;
    }

    public class C2S_TaskGetReward : MessageHead
    {
        public C2S_TaskGetReward()
        {
            MsgType = NetWorkMsgType.C2S_TaskGetReward;
        }

        public int TaskID;
        public bool IsMain;
    }

    public class S2C_UpdateMainTask : MessageHead
    {
        public S2C_UpdateMainTask()
        {
            MsgType = NetWorkMsgType.S2C_UpdateMainTask;
        }

        public GameTaskData TaskData;
        public int MainTaskCount;
    }

    public class C2S_RequestDailyTaskList : MessageHead    {
        public C2S_RequestDailyTaskList()
        {
            MsgType = NetWorkMsgType.C2S_RequestDailyTaskList;
        }
    }

    public class S2C_RequestDailyTaskList : MessageHead
    {
        public S2C_RequestDailyTaskList()
        {
            MsgType = NetWorkMsgType.S2C_RequestDailyTaskList;
        }

        public List<GameTaskData> DailyTaskList;
    }

    //副本
    public class C2S_EnterCopy : MessageHead
    {
        public C2S_EnterCopy()
        {
            MsgType = NetWorkMsgType.C2S_EnterCopy;
        }

        public int LevelType;
    }

    public class S2C_EnterCopy : MessageHead
    {
        public S2C_EnterCopy()
        {
            MsgType = NetWorkMsgType.S2C_EnterCopy;
        }

        public int LevelType;
    }

    public class C2S_ExitCopy : MessageHead
    {
        public C2S_ExitCopy()
        {
            MsgType = NetWorkMsgType.C2S_ExitCopy;
        }

        public int LevelType;
        public string CurTotalDamage;
        public int CurBossLevel;
        public bool IsWin;
        public List<GameEnginePartData> LstEnginePartData;
    }

    public class S2C_ExitCopy : MessageHead
    {
        public S2C_ExitCopy()
        {
            MsgType = NetWorkMsgType.S2C_ExitCopy;
        }

        public int LevelType;
        public int Level;
        public long RewardCount;
        public int KeyCount;
        public string CurTotalDamage;
        public int CurBossLevel;
    }

    public class C2S_UpdateCopyKeyCount : MessageHead
    {
        public C2S_UpdateCopyKeyCount()
        {
            MsgType = NetWorkMsgType.C2S_UpdateCopyKeyCount;
        }
    }

    public class S2C_UpdateCopyKeyCount : MessageHead
    {
        public S2C_UpdateCopyKeyCount()
        {
            MsgType = NetWorkMsgType.S2C_UpdateCopyKeyCount;
        }

        public int DiamondKeyCount;
        public int CoinKeyCount;
        public int OilKeyCount;
        public int TrophyKeyCount;
        public int ReformKeyCount;
    }

    //开放剧情系统
    public class C2S_UpdateLockStoryData : MessageHead
    {
        public C2S_UpdateLockStoryData()
        {
            MsgType = NetWorkMsgType.C2S_UpdateLockStoryData;
        }

        public GameLockStoryData LockStoryData;
    }

    // 考古研究

    public class C2S_UpdateResearchTime : MessageHead
    {
        public C2S_UpdateResearchTime()
        {
            MsgType = NetWorkMsgType.C2S_UpdateResearchTime;
        }

        public int ResearchId;
    }

    public class S2C_UpdateResearchTime : MessageHead
    {
        public S2C_UpdateResearchTime()
        {
            MsgType = NetWorkMsgType.S2C_UpdateResearchTime;
        }

        public int ResearchId;
        public long ResearchTimeStamp;
    }

    public class C2S_Researching : MessageHead
    {
        public C2S_Researching()
        {
            MsgType = NetWorkMsgType.C2S_Researching;
        }

        public int ResearchId;
        public int ResearchCompleteType;
    }

    public class S2C_Researching : MessageHead
    {
        public S2C_Researching()
        {
            MsgType = NetWorkMsgType.S2C_Researching;
        }

        public int ResearchId;
        public int ResearchLevel;
        public List<GameResearchData> ResearchList;
        public GameResearchEffectData ResearchEffectData;
    }

    //淬炼
    public class C2S_QuenchingLock : MessageHead
    {
        public C2S_QuenchingLock()
        {
            MsgType = NetWorkMsgType.C2S_QuenchingLock;
        }

        public int QuenchingId;
        public int QuenchingUnLockType;
    }

    public class C2S_Quenching : MessageHead
    {
        public C2S_Quenching()
        {
            MsgType = NetWorkMsgType.C2S_Quenching;
        }

        public List<GameQuenchingData> QuenchingList;
    }

    #region 战利品相关协议

    public class C2S_SpoilDraw : MessageHead
    {
        public C2S_SpoilDraw()
        {
            MsgType = NetWorkMsgType.C2S_SpoilDraw;
        }
    }

    public class S2C_SpoilDraw : MessageHead
    {
        public S2C_SpoilDraw()
        {
            MsgType = NetWorkMsgType.S2C_SpoilDraw;
        }

        public SpoilData Spoil;
    }

    public class S2C_SpoilSlotUnlock : MessageHead
    {
        public S2C_SpoilSlotUnlock()
        {
            MsgType = NetWorkMsgType.S2C_SpoilSlotUnlock;
        }

        public int SlotId;
    }

    public class C2S_SpoilEquip : MessageHead
    {
        public C2S_SpoilEquip()
        {
            MsgType = NetWorkMsgType.C2S_SpoilEquip;
        }

        public int SpoilId;
    }

    public class S2C_SpoilEquip : MessageHead
    {
        public S2C_SpoilEquip()
        {
            MsgType = NetWorkMsgType.S2C_SpoilEquip;
        }

        public SpoilSlotData CurSlotData;
        public SpoilSlotData OriginSlotData;
    }

    public class C2S_SpoilUpgrade : MessageHead
    {
        public C2S_SpoilUpgrade()
        {
            MsgType = NetWorkMsgType.C2S_SpoilUpgrade;
        }

        public SpoilData Spoil;
    }

    public class S2C_SpoilUpgrade : MessageHead
    {
        public S2C_SpoilUpgrade()
        {
            MsgType = NetWorkMsgType.S2C_SpoilUpgrade;
        }

        public SpoilData Spoil;
    }

    public class C2S_SpoilBreakthrough : MessageHead
    {
        public C2S_SpoilBreakthrough()
        {
            MsgType = NetWorkMsgType.C2S_SpoilBreakthrough;
        }

        public int SpoilId;
    }

    public class S2C_SpoilBreakthrough : MessageHead
    {
        public S2C_SpoilBreakthrough()
        {
            MsgType = NetWorkMsgType.S2C_SpoilBreakthrough;
        }

        public SpoilBreakthroughData SpoilBreakthrough;
    }

    #endregion


    //英雄
    public class C2S_RoleOn : MessageHead
    {
        public C2S_RoleOn()
        {
            MsgType = NetWorkMsgType.C2S_RoleOn;
        }

        public int RoleId;
    }

    public class S2C_RoleOn : MessageHead
    {
        public S2C_RoleOn()
        {
            MsgType = NetWorkMsgType.S2C_RoleOn;
        }

        public int RoleId;
    }

    public class S2C_RoleOff : MessageHead
    {
        public S2C_RoleOff()
        {
            MsgType = NetWorkMsgType.S2C_RoleOff;
        }

        public int RoleId;
    }

    public class C2S_RoleIntensify : MessageHead
    {
        public C2S_RoleIntensify()
        {
            MsgType = NetWorkMsgType.C2S_RoleIntensify;
        }

        public int RoleId;
    }

    public class S2C_RoleIntensify : MessageHead
    {
        public S2C_RoleIntensify()
        {
            MsgType = NetWorkMsgType.S2C_RoleIntensify;
        }

        public int RoleId;
        public int RoleLevel;
        public int RoleExp;
        public bool RoleBreakState;
    }

    public class C2S_RoleBreak : MessageHead
    {
        public C2S_RoleBreak()
        {
            MsgType = NetWorkMsgType.C2S_RoleBreak;
        }

        public int RoleId;
    }

    public class S2C_RoleBreak : MessageHead
    {
        public S2C_RoleBreak()
        {
            MsgType = NetWorkMsgType.S2C_RoleBreak;
        }

        public int RoleId;
        public int RoleBreakLevel;
        public bool RoleBreakState;
    }

    public class S2C_RoleListUpdate : MessageHead
    {
        public S2C_RoleListUpdate()
        {
            MsgType = NetWorkMsgType.S2C_RoleListUpdate;
        }

        public List<GameRoleData> RoleList;
    }

    //英雄升级材料更新
    public class S2C_MushRoomUpdate : MessageHead
    {
        public S2C_MushRoomUpdate()
        {
            MsgType = NetWorkMsgType.S2C_MushRoomUpdate;
        }

        public int MushRoom;
    }

    //英雄突破材料更新
    public class S2C_BreakOreUpdate : MessageHead
    {
        public S2C_BreakOreUpdate()
        {
            MsgType = NetWorkMsgType.S2C_BreakOreUpdate;
        }

        public int BreakOre;
    }

    //英雄突破天赋树重置
    public class C2S_BreakTreeReset : MessageHead
    {
        public C2S_BreakTreeReset()
        {
            MsgType = NetWorkMsgType.C2S_BreakTreeReset;
        }
    }

    //英雄突破天赋树强化
    public class C2S_BreakTreeIntensify : MessageHead
    {
        public C2S_BreakTreeIntensify()
        {
            MsgType = NetWorkMsgType.C2S_BreakTreeIntensify;
        }

        public int Id; //突破天赋树当前项Id
    }

    public class S2C_BreakTreeIntensify : MessageHead
    {
        public GameBreakTreeData CurBreakData; //突破天赋树当前项数据

        public S2C_BreakTreeIntensify()
        {
            MsgType = NetWorkMsgType.S2C_BreakTreeIntensify;
        }
    }

    public class S2C_BreakTPUpdate : MessageHead
    {
        public S2C_BreakTPUpdate()
        {
            MsgType = NetWorkMsgType.S2C_BreakTPUpdate;
        }

        public int BreakTP;
    }
    
    // 引擎系统
    public class C2S_EngUpgrade : MessageHead // 引擎升级
    {
        public C2S_EngUpgrade()
        {
            MsgType = NetWorkMsgType.C2S_EngUpgrade;
        }
    }

    public class S2C_EngUpgrade : MessageHead
    {
        public S2C_EngUpgrade()
        {
            MsgType = NetWorkMsgType.S2C_EngUpgrade;
        }
        public int Level;
        public int Exp;
    }

    public class C2S_EngPartOn : MessageHead // 引擎装备装配
    {
        public C2S_EngPartOn()
        {
            MsgType = NetWorkMsgType.C2S_EngPartOn;
        }
        public int InsID;
        public int SlotID; // 1 - 6
    }

    public class S2C_EngPartOn : MessageHead
    {
        public S2C_EngPartOn()
        {
            MsgType = NetWorkMsgType.S2C_EngPartOn;
        }
        public int InsID;
        public int SlotID; // 1 - 6
    }

    public class C2S_EngPartOff : MessageHead // 引擎装备卸下
    {
        public C2S_EngPartOff()
        {
            MsgType = NetWorkMsgType.C2S_EngPartOff;
        }
        public int InsID;
    }

    public class S2C_EngPartOff : MessageHead
    {
        public S2C_EngPartOff()
        {
            MsgType = NetWorkMsgType.S2C_EngPartOff;
        }
        public int InsID;
        public int SlotID; // 1 - 6
    }

    public class C2S_EngResolve : MessageHead // 分解
    {
        public C2S_EngResolve()
        {
            MsgType = NetWorkMsgType.C2S_EngResolve;
        }
        public List<int> InsID;
    }

    public class S2C_EngResolve : MessageHead
    {
        public S2C_EngResolve()
        {
            MsgType = NetWorkMsgType.S2C_EngResolve;
        }
        public List<int> InsID;
    }

    public class S2C_UpdateEngParts : MessageHead
    {
        public List<GameEnginePartData> Parts;
    }
}

