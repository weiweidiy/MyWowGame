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
        EngineCopy = 3, //引擎副本
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
        ToEngineCopy = 12, //引擎副本
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
        [LabelText("引擎")] Engine = 5,
        [LabelText("玩具")] Toy = 6,
        [LabelText("钻石/元宝")] Diamond = 101,
        [LabelText("金币")] Coin = 102,
        [LabelText("零件")] Gear = 103,
        [LabelText("统领材料")] Honor = 104,
        [LabelText("古代金属")] Metal = 105,
        [LabelText("钢铁")] Iron = 106,
        [LabelText("宝箱")] Treasure = 1000,
        [LabelText("道具")] Prop = 2000,
    }

    /// <summary>
    /// 房间类型
    /// </summary>RoomType
    public enum RoomType
    {
        ATK, //攻击
        HP, //血量
        HPRecover, //血量恢复
        Critical, //暴击
        CriticalDamage, //暴击伤害
        Speed, //攻击速度
        DoubleHit, //双连击
        TripletHit, //三连击
    }

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

    #region 引擎相关

    /// <summary>
    /// 随机属性
    /// </summary>
    public enum EngineAttrType
    {
        None,
        ATK = 11, //攻击力
        HP = 12, //体力
        HPRecover = 13, //体力恢复
        ASPD = 14, //攻击速度
        CriticalHitChance = 15, //暴击率
        CriticalHitDamage = 16, //暴击伤害
        DoubleHit = 17, //二连击率
        TripleHit = 18, //三连击率
        EvasionRate = 19, //闪避率
        CompanionDamage = 20, //同伴攻击伤害
        CompanionASPD = 21, //同伴攻击速度
        SkillDamage = 22, //技能伤害
        SkillCooldown = 23, //技能冷却减少
        GoldObtain = 24, //获得金币量
        BossDamageAmount = 25, //对Boss伤害量
        BattleDuration = 26, //战斗时长
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
        LT_2300 = 2300, //村庄副本
        LT_2400 = 2400, //侏儒副本
        LT_2500 = 2500, //突击副本
        LT_2600 = 2600, //考古玩法
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
    }

    /// <summary>
    /// 开放解锁状态
    /// </summary>
    public enum LockState
    {
        Lock = 0, //未解锁
        Unlock = 1, //解锁
    }

    #endregion

    #region 考古研究相关

    public enum ResearchType
    {
        None,
        [LabelText("攻击力增加")] IncreaseATK,
        [LabelText("矿锤补充速度增加")] IncreaseHammerRecoverSpeed,
        [LabelText("矿锤拥有上限增加")] IncreaseHammerLimit,
        [LabelText("矿石获得量增加")] IncreaseMineObtainAmount,
        [LabelText("体力增加")] IncreaseHP,
        [LabelText("研究速度增加")] IncreaseResearchSpeed,
    }

    #endregion
}