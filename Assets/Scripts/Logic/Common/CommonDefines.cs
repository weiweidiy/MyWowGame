//通用定义

using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Common
{
    /// <summary>
    /// 关卡类型
    /// </summary>
    public enum LevelType
    {
        NormalLevel = 0, //普通关卡
        DiamondCopy = 1, //钻石副本
        CoinCopy = 2, //金币副本
        OilCopy = 3, //原油副本
        TrophyCopy = 4, //功勋副本
        ReformCopy = 5, //改造副本
    }

    /// <summary>
    /// 关卡状态
    /// </summary>
    public enum LevelState
    {
        Normal, //正常推进
        HandUp, //挂机
    }

    /// <summary>
    /// 战斗切换到何处
    /// </summary>
    public enum SwitchToType
    {
        ToNextNode, //切换到下一节点
        ToFallBack, //普通关卡死亡回退
        ToBoss, //切换到BOSS关卡
        ToNextLevel, //切换到下一关卡
        ToNormalLevel, //切换到普通关卡 从其他特殊玩法

        ToDiamondCopy = 10, //钻石副本
        ToCoinCopy = 11, //金币副本
        ToOilCopy = 12, //原油副本
        ToTrophyCopy = 13, //战利品副本
        ToReformCopy = 14, //改造副本
    }

    /// <summary>
    /// 普通关卡刷怪类型
    /// </summary>
    public enum SpawnType
    {
        Normal = 1,
        Elite = 2,
        Both = 3,
        Boss = 4,
        BossNormal = 5,
        All = 6,
    }

    /// <summary>
    /// Item品质
    /// </summary>
    public enum ItemQuality
    {
        White = 0, //白
        Green, //绿
        Blue, //蓝
        Purple, //紫
        Yellow, //黄
        Cyan, //青
        Red //红
    }

    /// <summary>
    /// 通用Item类型
    /// </summary>
    public enum ItemType
    {
        None,
        [LabelText("武器")] Weapon = 1,
        [LabelText("防具")] Armor = 2,
        [LabelText("技能")] Skill = 3,
        [LabelText("伙伴")] Partner = 4,
        [LabelText("火花塞")] Spark = 5,
        [LabelText("气缸")] Cylinder = 6,
        [LabelText("战利品")] Spoil = 7,
        [LabelText("礼包")] ShopGift = 8,
        [LabelText("英雄技能")] HeroSkill = 10,
        [LabelText("钻石/元宝")] Diamond = 101,
        [LabelText("金币")] Coin = 102,
        [LabelText("零件")] Gear = 103,
        [LabelText("统领材料")] Honor = 104,
        [LabelText("古代金属")] Metal = 105,
        [LabelText("钢铁")] Iron = 106,
        [LabelText("音符")] Oil = 107,
        [LabelText("鸡腿")] Trophy = 108,
        [LabelText("蘑菇")] MushRoom = 109,
        [LabelText("氪金矿石")] BreakOre = 110,
        [LabelText("宝箱")] Treasure = 1000,
        [LabelText("道具")] Prop = 2000,
        [LabelText("钻石钥匙")] DiamondKey = 2101,
        [LabelText("金币钥匙")] CoinKey = 2102,
        [LabelText("功勋钥匙")] TrophyKey = 2103,
        [LabelText("音乐钥匙")] OilKey = 2104,
        [LabelText("改造钥匙")] ReformKey = 2105,
    }

    /// <summary>
    /// 房间类型
    /// </summary>RoomType
    public enum AttributeType
    {
        [LabelText("攻击")] ATK = 11,
        [LabelText("血量")] HP = 12,
        [LabelText("血量恢复")] HPRecover = 13,
        [LabelText("攻击速度")] Speed = 14,
        [LabelText("暴击")] Critical = 15,
        [LabelText("暴击伤害")] CriticalDamage = 16,
        [LabelText("双连击")] DoubleHit = 17,
        [LabelText("三连击")] TripletHit = 18,
        [LabelText("闪避率")] EvasionRate = 19,
        [LabelText("同伴伤害")] CompanionDamage = 20,
        [LabelText("同伴攻击速度")] CompanionASPD = 21,
        [LabelText("技能伤害")] SkillDamage = 22,
        [LabelText("技能冷却减少")] SkillCooldown = 23,
        [LabelText("获得金币量")] GoldObtain = 24,
        [LabelText("对Boss伤害量")] BossDamageAmount = 25,
        [LabelText("战斗时长")] BattleDuration = 26,
        [LabelText("体力恢复上限")] HPRecoverEverySecond = 28,
        [LabelText("多连射")] MultipleShot = 29,
        [LabelText("矿锤拥有上限增加")] HammerLimit = 30,
        [LabelText("矿石获得量增加")] MineObtainAmount = 31,
        [LabelText("矿锤补充速度增加")] HammerRecoverSpeed = 32,
        [LabelText("研究速度增加")] ResearchSpeed = 33,

        [LabelText("武器攻击力提升")] WeaponATK = 34,
        [LabelText("防具血量提升")] ArmorHP = 35,
        [LabelText("导弹类技能伤害提升")] MissileSkillDamage = 36,
        [LabelText("炮弹类技能伤害提升")] CannonBallSkillDamage = 37,
        [LabelText("机枪类技能伤害提升")] MachineGunSkillDamage = 38,
        [LabelText("特战小队伤害提升")] MachineGun = 40,
        [LabelText("工程小队伤害提升")] SpecialWarfareTeam = 41,
        [LabelText("后勤小队伤害提升")] LogisticsTeam = 42,
        [LabelText("摇滚乐队伤害提升")] RockTeam = 43,
        [LabelText("坏人小队伤害提升")] BadGuyTeam = 44,
        [LabelText("运动员队伤害提升")] AthleteTeam = 45,
        [LabelText("英雄经验获取")] RoleExpObtain = 50,
    }

    #region 商店相关

    //商店类型
    public enum ShopType
    {
        Diamond = 0, //充值
        Gift = 1, //礼包
        //季票
    }

    //商店礼包类型
    public enum ShopGiftType
    {
        Once = 1, //一次性 购买后就没了
        Day = 2, //每日重置 - 自然日凌晨5点起算
        Week = 3, //每周重置
        Month = 4, //每月重置
    }

    //商店礼包时间类型
    public enum ShopGiftTimeType
    {
        None = 0, //无时间
        Fixed = 1, //固定时间间隔 读 起始时间 终止时间
        Start = 2, //无时间限制 读 起始时间
        UnLockTime = 3, //解锁条件 持续时间后终止 读 持续时间
        UnLock = 4, //解锁条件 无时间限制
    }

    #endregion

    /// <summary>
    /// 定义游戏里用到的颜色数值(主要是字体)
    /// </summary>
    public static class GameColors
    {
        //正常白色
        public static Color Normal = Color.white;

        //材料不够
        public static Color NotEnough = Color.red;
    }

    /// <summary>
    /// 动画状态机Trigger值
    /// </summary>
    public static class AniTrigger
    {
        public static readonly int ToIdle = Animator.StringToHash("ToIdle");
        public static readonly int ToMove = Animator.StringToHash("ToMove");
        public static readonly int ToAtk = Animator.StringToHash("ToAtk");
        public static readonly int ToDead = Animator.StringToHash("ToDead");
    }

    /// <summary>
    /// 主界面底部按钮
    /// </summary>
    public enum BottomBtnType
    {
        User = 0, //角色
        Copy = 1, //副本
        Home = 2, //主界面
        Special = 3, //特殊玩法
        Shop = 4 //商店
    }

    /// <summary>
    /// 战斗状态切换
    /// </summary>
    public class FightSwitchTo
    {
        public SwitchToType m_SwitchToType { get; set; }
        public bool m_CanSwitchToNextNode { get; set; }
    }

    #region 抽卡相关

    /// <summary>
    /// 抽卡类型
    /// </summary>
    public enum DrawCardType
    {
        Skill = 1,
        Partner = 2,
        Equip = 3,
    }

    // 抽卡消耗
    public enum DrawCardCostType
    {
        Draw11CardCost,
        Draw35CardCost,
    }

    #endregion

    #region 任务相关

    /// <summary>
    /// 任务大类型
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        ///完成关卡进度-累计
        /// </summary>
        TT_1001 = 1001,

        /// <summary>
        ///完成钻石副本进度-累计
        /// </summary>
        TT_2001 = 2001,

        /// <summary>
        ///完成金币副本进度-累计
        /// </summary>
        TT_2002 = 2002,

        /// <summary>
        ///升级攻击力进度-累计
        /// </summary>
        TT_3001 = 3001,

        /// <summary>
        ///升级血量进度-累计
        /// </summary>
        TT_3002 = 3002,

        /// <summary>
        ///升级恢复进度-累计
        /// </summary>
        TT_3003 = 3003,

        /// <summary>
        ///升级暴击进度-累计
        /// </summary>
        TT_3004 = 3004,

        /// <summary>
        ///升级暴击率进度-累计
        /// </summary>
        TT_3005 = 3005,

        /// <summary>
        ///升级攻速进度-累计
        /// </summary>
        TT_3006 = 3006,

        /// <summary>
        ///升级二连击进度-累计
        /// </summary>
        TT_3007 = 3007,

        /// <summary>
        ///升级三连击进度-累计
        /// </summary>
        TT_3008 = 3008,

        /// <summary>
        ///召唤装备进度-累计
        /// </summary>
        TT_4001 = 4001,

        /// <summary>
        ///召唤技能进度-累计
        /// </summary>
        TT_4002 = 4002,

        /// <summary>
        ///召唤伙伴进度-累计
        /// </summary>
        TT_4003 = 4003,

        /// <summary>
        ///考古层-累计
        /// </summary>
        TT_5001 = 5001,

        /// <summary>
        ///研究级-累计
        /// </summary>
        TT_5002 = 5002,

        /// <summary>
        ///击败敌人-活跃
        /// </summary>
        TT_7001 = 7001,

        /// <summary>
        ///召唤-活跃
        /// </summary>
        TT_8001 = 8001,

        /// <summary>
        ///召唤装备-活跃
        /// </summary>
        TT_8002 = 8002,

        /// <summary>
        ///召唤技能-活跃
        /// </summary>
        TT_8003 = 8003,

        /// <summary>
        ///召唤伙伴-活跃
        /// </summary>
        TT_8004 = 8004,

        /// <summary>
        /// 强化武器-活跃
        /// </summary>
        TT_9001 = 9001,

        /// <summary>
        /// 强化防具-活跃
        /// </summary>
        TT_9002 = 9002,

        /// <summary>
        /// 强化技能-活跃
        /// </summary>
        TT_9003 = 9003,

        /// <summary>
        /// 强化伙伴-活跃
        /// </summary>
        TT_9004 = 9004,

        /// <summary>
        ///完成钻石副本次数-活跃
        /// </summary>
        TT_9012 = 9012,

        /// <summary>
        ///完成金币副本次数-活跃
        /// </summary>
        TT_9013 = 9013,
    }

    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskState
    {
        //进行中
        Process = 0,

        //完成
        Complete = 1,

        //已领取
        Done = 2,
    }

    #endregion

    #region 考古相关

    /// <summary>
    /// 考古道具
    /// </summary>
    public enum MiningType
    {
        Door = 99, //门和层数
        WeaponTreasure = 100001, //武器宝箱
        ArmorTreasure = 100002, //防具宝箱
        EquipTreasure = 100005, //装备宝箱
        RandomTreasure = 100006, //随机宝箱
        Coin = 100007, //金币堆
        BigCoin = 100008, //大金币
        Honor = 100009, //统领材料包
        Gear = 100010, //零件材料包
        CopperMine = 100011, //远古铜矿
        SilverMine = 100012, //远古银矿
        GoldMine = 100013, //远古金矿
        Diamond = 100015, //钻石包
        SpecialProp = 100016, // 考古道具包
        Hammer = 100017, //铲子包
        Bomb = 100018, //炸药包
        Scope = 100019, //透视镜包
    }

    /// <summary>
    /// 考古道具数据
    /// </summary>
    public enum MiningUpdateType
    {
        Reduce, //减少
        Increase, //增加
    }

    #endregion

    #region 开放功能相关

    /// <summary>
    /// 开放解锁类型
    /// </summary>
    public enum LockType
    {
        None,
        LT_100 = 100, //解锁舱室1（攻击）
        LT_200 = 200, //解锁舱室2（生命）
        LT_300 = 300, //解锁舱室3（生命恢复）
        LT_400 = 400, //解锁舱室4（攻速）
        LT_500 = 500, //解锁舱室5（暴击率）
        LT_600 = 600, //解锁舱室6（暴击伤害）
        LT_700 = 700, //技能1(技能系统开启)
        LT_800 = 800, //技能2
        LT_900 = 900, //技能3
        LT_1000 = 1000, //技能4
        LT_1100 = 1100, //技能5
        LT_1200 = 1200, //技能6
        LT_1300 = 1300, //兽人1(兽人系统开启)
        LT_1400 = 1400, //兽人2
        LT_1500 = 1500, //兽人3
        LT_1600 = 1600, //兽人4
        LT_1700 = 1700, //兽人5
        LT_1800 = 1800, //装备攻击(装备系统开启)
        LT_1900 = 1900, //装备血量
        LT_2000 = 2000, //装备引擎(引擎系统开启)
        LT_2100 = 2100, //钻石副本
        LT_2200 = 2200, //金币副本
        LT_2300 = 2300, //战利品副本
        LT_2400 = 2400, //原油副本
        LT_2500 = 2500, //改造副本
        LT_2600 = 2600, //考古玩法
        LT_2601 = 2601, //考古研究
        LT_2611 = 2611, //司机系统
        LT_2700 = 2700, //召唤装备
        LT_2800 = 2800, //召唤技能
        LT_2900 = 2900, //召唤兽人
        LT_3000 = 3000, //探索页签
        LT_3100 = 3100, //副本页签
        LT_3200 = 3200, //商店页签
        LT_3300 = 3300, //成长页签
        LT_3400 = 3400, //统领页签
        LT_3500 = 3500, //技能页签
        LT_3600 = 3600, //伙伴页签
        LT_3700 = 3700, //战利品页签
        LT_3800 = 3800, //淬炼页签
    }

    #endregion

    #region 引导相关

    public enum GuidanceType
    {
        Forced = 0, //强制引导
        Soft = 1, //软引导
    }

    #endregion

    #region 考古研究相关

    public enum ResearchState
    {
        [LabelText("未研究")] UnLock,
        [LabelText("可研究")] Min,
        [LabelText("研究中")] Middle,
        [LabelText("研究完成")] Max,
    }

    public enum ResearchCompleteType
    {
        TimeComplete,
        DiamondComplete,
    }

    #endregion

    #region 淬炼相关

    public enum QuenchingType
    {
        Do = 1,
        Re = 2,
        Mi = 3,
        Fa = 4,
        Sol = 5,
    }

    #endregion
}