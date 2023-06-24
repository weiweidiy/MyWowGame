using Configs;

namespace Logic.Common
{
    /// <summary>
    /// 从GameDefineCfg表中读取的数据 用于游戏内的常量定义
    /// </summary>
    public static class GameDefine
    {
        #region 主角属性配置相关

        //主角基础攻击力
        public static int GJJBaseAtk;

        //主角攻击成长系数
        public static int GJJAtkGrow;

        //主角基础体力
        public static int GJJBaseHP;

        //主角体力成长系数
        public static int GJJHPGrow;

        //主角攻击速度
        public static float GJJAtkSpeed;

        //GJJ攻击速度成长系数
        public static float GJJAtkSpeedGrow;

        //GJJ基础暴击倍数值
        public static float GJJBaseCriticalDamage;

        //GJJ暴击倍数值成长系数
        public static float GJJCriticalDamageGrow;

        //GJJ基础暴击率(万分比)
        public static int GJJBaseCritical;

        //GJJ暴击率成长系数(万分比)
        public static int GJJCriticalGrow;

        //GJJ基础生命回复值
        public static float GJJBaseHPRecover;

        //GJJ生命回复成长系数
        public static float GJJHPRecoverGrow;

        //GJJ基础二连击概率(万分比)
        public static int GJJBaseDoubleHit;

        //GJJ二连击成长系数(万分比)
        public static int GJJDoubleHitGrow;

        //GJJ基础三连击概率(万分比)
        public static int GJJBaseTripletHit;

        //GJJ三连击成长系数(万分比)
        public static int GJJTripletHitGrow;

        #endregion

        #region 关卡基础配置相关

        public static int LevelDefaultWave;

        //章节默认关卡数
        public static int ChapterDefaultLevel;

        //关卡默认小节点数量
        public static int LevelDefaultNodeNum;

        //普通怪刷新间隔
        public static int NormalSpawnInterval;

        //BOSS战斗时限
        public static float BOSSFightTime;

        //放置奖励倍率
        public static float PlaceRewardMultiplier;

        //放置奖励显示时长(min)
        public static int PlaceRewardBtnShowTime;

        //放置奖励最长时长(min)
        public static int PlaceRewardMaxTime;

        //放置奖励抽卡系数
        public static float PlaceRewardFactor;

        #endregion

        #region 挂机相关配置

        public static int HangUpSpawnInterval;
        public static float HangUpATKWight;
        public static float HangUpHPWight;
        public static float HangUpDropWight;

        #endregion

        #region 道具技能等配置相关

        public static int SkillMaxID;
        public static int PartnerMaxID;
        public static int WeaponMaxID;
        public static int ArmorMaxID;
        public static int SkillMinID;
        public static int PartnerMinID;
        public static int WeaponMinID;
        public static int ArmorMinID;
        public static int CommonItemMaxLevel;

        #endregion

        #region 其他通用配置

        public static int Draw11CardCost;
        public static int Draw35CardCost;

        #endregion

        #region 副本通用高配置

        public static int CopyDiamondTime;
        public static int CopyDiamondCount;
        public static float CopyDiamondMoveSpeedMult;
        public static int CopyCoinTime;
        public static int CopyDiamondSpawnInterval;
        public static int CopyOilConstValue; //公式常量
        public static float CopyOilBossAtkConstValue; //Boss攻击常量
        public static int CopyTrophyTime;
        public static float CopyTrophyMoveSpeedMult;
        public static int CopyTrophySpawnInterval; //刷怪间隔
        public static int CopyTrophyCount;

        public static float CopyReformMoveSpeedMult;
        public static int CopyReformSpawnInterval; //刷怪间隔
        public static int CopyReformCount;
        public static int CopyReformCylinderRate; //气缸权重
        public static int CopyReformAttrRate; //属性权重

        #endregion

        #region 考古相关配置

        public static int MaxHammerCount; // 矿锤最大数量
        public static int AddHammerTime; // 矿锤回复刷新时间
        public static int MiningLoopStartFloor; // 考古随机开始矿层
        public static int MiningLoopEndFloor; // 考古随机终止矿层
        public static int ResearchDiamondCost; // 研究每分钟钻石消耗数

        #endregion

        #region 淬炼相关配置

        public static int DJProbabilityF;
        public static int DJProbabilityE;
        public static int DJProbabilityD;
        public static int DJProbabilityC;
        public static int DJProbabilityB;
        public static int DJProbabilityA;
        public static int DJProbabilityS;
        public static int DJProbabilitySS;

        #endregion

        #region 英雄系统相关

        public static int RoleAttrMaxLevel;
        public static float RoleMushRoomExp;
        public static int RoleBreakBreakTPGet;
        public static int RoleBreakEveryLevel;
        public static int RoleBreakTreeDiamondCost;
        public static int RoleBreakTreeDefaultId;
        public static int RoleUpgradeCost;

        #endregion

        #region 引擎

        public static float EngineUpgradeExp;
        public static int EngineMaxLevel;
        public static int SparkComposeCost;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            GJJBaseAtk = GameDefineCfg.GetData(1).IntValue;
            GJJAtkGrow = GameDefineCfg.GetData(2).IntValue;
            GJJBaseHP = GameDefineCfg.GetData(3).IntValue;
            GJJHPGrow = GameDefineCfg.GetData(4).IntValue;
            GJJAtkSpeed = GameDefineCfg.GetData(5).FloatValue;
            GJJAtkSpeedGrow = GameDefineCfg.GetData(6).FloatValue;
            GJJBaseCriticalDamage = GameDefineCfg.GetData(7).FloatValue;
            GJJCriticalDamageGrow = GameDefineCfg.GetData(8).FloatValue;
            GJJBaseCritical = GameDefineCfg.GetData(9).IntValue;
            GJJCriticalGrow = GameDefineCfg.GetData(10).IntValue;
            GJJBaseHPRecover = GameDefineCfg.GetData(11).FloatValue;
            GJJHPRecoverGrow = GameDefineCfg.GetData(12).FloatValue;
            GJJBaseDoubleHit = GameDefineCfg.GetData(13).IntValue;
            GJJDoubleHitGrow = GameDefineCfg.GetData(14).IntValue;
            GJJBaseTripletHit = GameDefineCfg.GetData(15).IntValue;
            GJJTripletHitGrow = GameDefineCfg.GetData(16).IntValue;

            LevelDefaultWave = GameDefineCfg.GetData(111).IntValue;
            ChapterDefaultLevel = GameDefineCfg.GetData(112).IntValue;
            LevelDefaultNodeNum = GameDefineCfg.GetData(113).IntValue;
            NormalSpawnInterval = GameDefineCfg.GetData(114).IntValue;
            BOSSFightTime = GameDefineCfg.GetData(115).FloatValue;
            PlaceRewardMultiplier = GameDefineCfg.GetData(116).FloatValue;
            PlaceRewardBtnShowTime = GameDefineCfg.GetData(117).IntValue;
            PlaceRewardMaxTime = GameDefineCfg.GetData(118).IntValue;
            PlaceRewardFactor = GameDefineCfg.GetData(119).FloatValue;

            SkillMaxID = GameDefineCfg.GetData(300).IntValue;
            PartnerMaxID = GameDefineCfg.GetData(301).IntValue;
            WeaponMaxID = GameDefineCfg.GetData(302).IntValue;
            ArmorMaxID = GameDefineCfg.GetData(303).IntValue;
            SkillMinID = GameDefineCfg.GetData(310).IntValue;
            PartnerMinID = GameDefineCfg.GetData(311).IntValue;
            WeaponMinID = GameDefineCfg.GetData(312).IntValue;
            ArmorMinID = GameDefineCfg.GetData(313).IntValue;
            CommonItemMaxLevel = GameDefineCfg.GetData(320).IntValue;

            HangUpATKWight = GameDefineCfg.GetData(400).FloatValue;
            HangUpHPWight = GameDefineCfg.GetData(401).FloatValue;
            HangUpDropWight = GameDefineCfg.GetData(402).FloatValue;
            HangUpSpawnInterval = GameDefineCfg.GetData(420).IntValue;

            Draw11CardCost = GameDefineCfg.GetData(500).IntValue;
            Draw35CardCost = GameDefineCfg.GetData(501).IntValue;

            CopyDiamondTime = GameDefineCfg.GetData(601).IntValue;
            CopyDiamondCount = GameDefineCfg.GetData(602).IntValue;
            CopyDiamondMoveSpeedMult = GameDefineCfg.GetData(605).FloatValue;
            CopyDiamondSpawnInterval = GameDefineCfg.GetData(606).IntValue;
            CopyCoinTime = GameDefineCfg.GetData(611).IntValue;

            CopyTrophyTime = GameDefineCfg.GetData(631).IntValue; //战利品副本战斗时限
            CopyTrophyCount = GameDefineCfg.GetData(632).IntValue;
            CopyTrophyMoveSpeedMult = GameDefineCfg.GetData(635).FloatValue;
            CopyTrophySpawnInterval = GameDefineCfg.GetData(636).IntValue;

            CopyReformCount = GameDefineCfg.GetData(1001).IntValue;
            CopyReformMoveSpeedMult = GameDefineCfg.GetData(1004).FloatValue;
            CopyReformSpawnInterval = GameDefineCfg.GetData(1005).IntValue;

            CopyReformCylinderRate = GameDefineCfg.GetData(1010).IntValue;
            CopyReformAttrRate = GameDefineCfg.GetData(1011).IntValue;


            CopyOilConstValue = GameDefineCfg.GetData(641).IntValue;
            CopyOilBossAtkConstValue = GameDefineCfg.GetData(642).FloatValue;

            DJProbabilityF = GameDefineCfg.GetData(650).IntValue;
            DJProbabilityE = GameDefineCfg.GetData(651).IntValue;
            DJProbabilityD = GameDefineCfg.GetData(652).IntValue;
            DJProbabilityC = GameDefineCfg.GetData(653).IntValue;
            DJProbabilityB = GameDefineCfg.GetData(654).IntValue;
            DJProbabilityA = GameDefineCfg.GetData(655).IntValue;
            DJProbabilityS = GameDefineCfg.GetData(656).IntValue;
            DJProbabilitySS = GameDefineCfg.GetData(657).IntValue;

            MaxHammerCount = GameDefineCfg.GetData(701).IntValue;
            AddHammerTime = GameDefineCfg.GetData(703).IntValue;
            MiningLoopStartFloor = GameDefineCfg.GetData(704).IntValue;
            MiningLoopEndFloor = GameDefineCfg.GetData(705).IntValue;
            ResearchDiamondCost = GameDefineCfg.GetData(706).IntValue;

            RoleAttrMaxLevel = GameDefineCfg.GetData(901).IntValue;
            RoleMushRoomExp = GameDefineCfg.GetData(902).FloatValue;
            RoleBreakBreakTPGet = GameDefineCfg.GetData(903).IntValue;
            RoleBreakEveryLevel = GameDefineCfg.GetData(904).IntValue;
            RoleBreakTreeDiamondCost = GameDefineCfg.GetData(905).IntValue;
            RoleBreakTreeDefaultId = GameDefineCfg.GetData(906).IntValue;
            RoleUpgradeCost = GameDefineCfg.GetData(907).IntValue;

            EngineUpgradeExp = GameDefineCfg.GetData(1100).FloatValue;
            EngineMaxLevel = GameDefineCfg.GetData(1102).IntValue;
            SparkComposeCost = GameDefineCfg.GetData(1103).IntValue;
        }
    }
}