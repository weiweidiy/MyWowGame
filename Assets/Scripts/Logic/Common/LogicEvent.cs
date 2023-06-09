﻿namespace Logic.Common
{
    /// <summary>
    /// 游戏内 逻辑事件Event定义
    /// </summary>
    public enum LogicEvent
    {
        #region 网络状态

        ConnectSuccess = 100, //连接成功
        ConnectFailed, //连接失败
        ConnectLost, //连接丢失
        CloseHoldUI, //关闭UIHold

        LoginSuccess, //登录成功
        LoginFailed, //登录失败

        #endregion

        #region 战场/战斗相关

        Fight_Standby, //战斗准备
        Fight_Start, //开始战斗
        Fight_Fighting, //战斗中
        Fight_Over, //战斗结束
        Fight_Win, //战斗胜利 移动GJJ等表现

        //Fight_Cancel,   //取消当前战斗
        Fight_Switch, //遇到Boss/副本SplashUI 切换战斗
        Fight_MapMove, //背景 移动
        Fight_MapMoveBack,
        Fight_MapStop, //背景 停止

        Fight_LevelTypeChanged,
        Fight_LevelChanged, //关卡变化
        Fight_LevelNodeChanged, //关卡节点变化
        Fight_LevelStateChanged, //关卡状态变化

        Fight_ShowNormalBossTime, //显示普通Boss倒计时
        Fight_ShowCopyBossTime, //显示副本Boss倒计时
        Fight_NormalBossTimerChanged, //战斗倒计时变化
        Fight_CopyBossTimerChanged,
        Fight_CopyTimeUp, //副本时间到
        Fight_CopyDiamondCountChanged, //钻石副本进度变化
        Fight_ShowOilBossHpBar, //显示原油副本Boss血条
        Fight_OilBossHpChanged, //Boss血条变化
        Fight_OilBossLevelChanged, //Boss等级变化
        Fight_CopyTrophyCountChanged, //战利品副本进度变化


        Fight_MapChanged, //地图场景改变
        Fight_SwitchComplete, //切换完成

        Fight_EnemyCountChanged, //怪物数量信息改变

        #endregion

        #region 游戏资源类 变化通知

        CoinChanged, //游戏币变化
        DiamondChanged, //元宝变化
        OilChanged, //原油变化
        TropyChanged, //战利品变化
        CopyKeyChanged, // 副本钥匙变化
        RoleMushRoomChanged, //英雄升级材料变化
        RoleBreakOreChanged, //英雄突破材料变化
        RoleBreakTPChanged, //英雄突破天赋点变化
        HammerChanged, // 考古矿锤变化
        MineChanged, // 考古矿石变化
        TecPointChanged, //机灵点变化

        #endregion

        #region 技能 伙伴 装备 引擎相关

        //技能
        SkillListChanged, //技能变化
        SkillUpgraded, //技能升级成功
        SkillOn, //技能上下阵变化
        SkillOff, //技能上下阵变化
        SkillAllEffectUpdate, //技能总加成战斗力变化
        SkillAutoPlay, //技能自动释放 - 战斗通知
        SkillReset, //技能重置 切换关卡 BOSS 等 需要重置技能状态

        //伙伴
        PartnerListChanged, //伙伴变化
        PartnerUpgraded, //伙伴升级成功
        PartnerOn, //伙伴上下阵变化
        PartnerOff, //伙伴上下阵变化
        PartnerAllEffectUpdate, //伙伴总加成战斗力变化

        //装备
        EquipListChanged, //装备变化
        EquipOn,
        EquipOff,
        EquipWeaponUpgraded, //装备升级成功
        EquipArmorUpgraded,
        EquipAllATKEffectUpdate, //装备总加成攻击力变化
        EquipAllHPEffectUpdate, //装备总加成血量变化

        //引擎
        EngineUpgrade, //引擎升级
        EngineOn, //引擎装配
        EngineOff, //引擎解除
        EngineResolve, //引擎分解
        EnginePartsUpdate, //更新引擎装备列表
        EngineEffectUpdate, //更新引擎加成属性

        #endregion

        #region UI通用消息

        ShowTips, //显示Tips
        ShowFightSwitch, //战斗切换UI
        ShowObtain, //显示通用的获得界面

        MoveCamera, //摄像机操作

        #endregion

        #region 商店 抽卡 相关

        ShowDrawCardResult, //显示抽卡结果
        ShowDrawProbability,
        UpdateDrawCardData,
        ShowOrHideLevelUp,

        OnShopBuyOrder, //商店购买
        OnShopBuy,

        #endregion

        RoomUpgraded, //房间升级成功
        SyncUserData, //同步用户数据

        #region 任务相关

        TaskStateChanged, //任务状态变化
        MainTaskChanged, //主线任务变化 - 更换主线任务
        MainTaskProcessChanged, //主线任务进度变化
        MainTaskDoneChanged, //主线任务已领取变化
        DailyTaskChanged, //日常任务变化 - 领取状态变化
        DailyTaskProcessChanged, //日常任务进度变化
        DailyTaskListUpdate, //日常任务列表更新

        #endregion

        #region 考古相关

        MiningDataChanged, //更新考古数据
        ShowCrossedGrid, // 显示炸弹格子
        HideCrossedGrid, // 隐藏炸弹格子
        ShowNineGrid, // 显示透视镜格子
        HideNineGrid, // 隐藏透视镜格子

        #endregion

        #region 奖励相关

        ShowMiningReward, // 显示考古奖励
        ShowPlaceReward, // 显示放置奖励
        ShowCommonReward, // 显示通用奖励
        ShowOilCopyRewards,
        ShowReformCopyRewards, //显示改造副本奖励
        RefreshReformItems, //刷新改造副本道具

        #endregion

        #region 开放剧情相关

        UpdateLockState, //更新开放解锁状态
        UpdateUnlockAll, //开放所有解锁功能

        #endregion

        #region 舱室解锁

        RoomUnlocked, //舱室解锁了

        #endregion

        #region 研究相关

        OnUpdateResearchTime, //正在研究
        OnResearching, //研究完成
        ResearchLevelChanged, //研究等级变化
        ResearchCompleteEffectUpdate, //研究属性总加成变化
        ResearchMapChanged, //研究数据表变化

        #endregion

        #region 淬炼相关

        OnQuenching, // 正在淬炼
        QuenchingEffectUpdate,

        #endregion

        #region 战利品相关

        OnSpoilDraw,
        OnSpoilSlotUnlock,
        OnSpoilEquipChanged,
        OnSpoilUpgrade,
        OnSpoilBreakthrough,

        #endregion

        #region 英雄相关

        RoleListChanged, //英雄列表变化
        RoleOn, //英雄换上
        RoleOff, //英雄换下
        RoleIntensify, //英雄升级
        RoleBreak,
        RoleEffectUpdate, //英雄属性变化
        RoleBreakTreeReset, //英雄天赋树重置
        RoleBreakTreeIntensify, //英雄天赋树强化
        RoleBreakTreeEffectUpdate, //英雄天赋树属性变化

        #endregion

        #region 时间相关

        TimeDaySecondsChanged, //跨天秒数变化
        TimeDayChanged, //跨天天数变化
        TimeWeekSecondsChanged, //跨周秒数变化
        TimeWeekChanged, //跨周周数变化

        #endregion

        #region 引导相关

        GuidanceStart,
        GuidanceEnd,

        #endregion
    }
}