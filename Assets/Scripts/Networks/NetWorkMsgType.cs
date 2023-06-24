namespace Networks
{
    /// <summary>
    /// 网络消息类型定义
    /// C2S_ : 客户端发送给服务器
    /// S2C_ : 服务器发送给客户端
    /// </summary>
    public enum NetWorkMsgType
    {
        None,
        S2C_CommonError, //通用错误提示

        //登录
        C2S_Login,
        S2C_Login,

        //请求服务器时间
        C2S_ST,
        S2C_ST,

        //GM命令数据
        C2S_GMCommand,

        //游戏币变化
        C2S_SyncCoin,

        //元宝更新
        S2C_DiamondUpdate,

        //原油更新
        S2C_OilUpdate,
        
        //战功同步
        C2S_SyncTrophy,
        
        // 科技点更新
        S2C_TecPointUpdate,

        //同步玩家当前数据
        C2S_SyncRoomData,
        C2S_SyncSettingData,
        C2S_SyncLevelData,
        C2S_SyncPlaceRewardData,

        //装备
        C2S_EquipOn, //穿装备
        S2C_EquipOn,
        S2C_EquipOff, //替换下来
        C2S_EquipIntensify, //强化
        S2C_EquipIntensify,
        S2C_EquipListUpdate, //装备列表更新
        C2S_EquipCompose, // 单次合成
        S2S_EquipCompose,

        //伙伴
        C2S_PartnerOn, //上阵
        S2C_PartnerOn,
        C2S_PartnerOff, //下阵
        S2C_PartnerOff,
        C2S_PartnerIntensify, //强化
        S2C_PartnerIntensify,
        S2C_PartnerListUpdate, //伙伴列表更新
        C2S_PartnerCompose, // 单次合成
        S2S_PartnerCompose,

        //技能
        C2S_SkillOn, //上阵
        S2C_SkillOn,
        C2S_SkillOff, //下阵
        S2C_SkillOff,
        C2S_SkillIntensify, //强化
        S2C_SkillIntensify,
        S2C_SkillListUpdate, //伙伴列表更新
        C2S_SkillCompose, // 单次合成
        S2S_SkillCompose,

        //商店
        C2S_DrawCard,
        S2C_DrawCard,
        C2S_UpdateDrawCardData,
        S2C_UpdateDrawCardData,
        C2S_ShopBuy, //商店购买
        S2C_ShopBuyOrder, //购买订单 / 暂未使用
        S2C_ShopBuy, //购买成功发送

        //放置挂机
        C2S_PlaceReward,
        S2C_PlaceReward,

        //奖励
        C2S_GetPlaceReward,
        C2S_CommonReward,
        S2C_CommonReward,
        S2C_OilCopyReward,
        S2C_ReformCopyReward,

        // 更新考古相关数据
        C2S_MiningReward,
        S2C_MiningReward,
        C2S_UpdateMiningData,
        S2C_UpdateMiningData,

        //考古研究
        C2S_UpdateResearchTime,
        S2C_UpdateResearchTime,
        C2S_Researching,
        S2C_Researching,

        //任务
        C2S_UpdateTaskProcess, //更新任务进度
        S2C_UpdateTaskState, //更新任务状态
        C2S_TaskGetReward, //领取任务奖励
        S2C_UpdateMainTask, //新的主线任务
        S2C_UpdateDailyTask, //每日任务领取状态更新
        C2S_RequestDailyTaskList, //跨天以后请求新的每日任务列表
        S2C_RequestDailyTaskList,

        //副本
        C2S_EnterCopy, //进入
        S2C_EnterCopy,
        C2S_ExitCopy, //结束 - 只有成功的时候才会发送
        S2C_ExitCopy,
        C2S_UpdateCopyKeyCount, //更新钥匙数量
        S2C_UpdateCopyKeyCount,

        //开放剧情
        C2S_UpdateLockStoryData,

        //淬炼
        C2S_QuenchingLock,
        C2S_Quenching,

        //战利品
        C2S_SpoilDraw,
        S2C_SpoilDraw,
        S2C_SpoilSlotUnlock,
        C2S_SpoilEquip,
        S2C_SpoilEquip,
        C2S_SpoilUpgrade,
        S2C_SpoilUpgrade,
        C2S_SpoilBreakthrough,
        S2C_SpoilBreakthrough,

        //英雄
        C2S_RoleOn, //装配英雄
        S2C_RoleOn,
        S2C_RoleOff, //英雄替换
        C2S_RoleIntensify, //英雄强化
        S2C_RoleIntensify,
        C2S_RoleBreak, //英雄突破
        S2C_RoleBreak,
        S2C_RoleListUpdate, //英雄列表更新
        S2C_MushRoomUpdate, //英雄升级材料更新
        S2C_BreakOreUpdate, //英雄突破材料更新
        C2S_BreakTreeReset, //英雄突破天赋树重置
        C2S_BreakTreeIntensify, //英雄突破天赋树强化
        S2C_BreakTreeIntensify,
        S2C_BreakTPUpdate, //英雄突破天赋点
        
        // 引擎系统
        C2S_EngUpgrade, // 引擎升级
        S2C_EngUpgrade,

        C2S_EngPartOn, // 引擎装备装配
        S2C_EngPartOn,
        C2S_EngPartOff, // 引擎装备卸下
        S2C_EngPartOff,
        C2S_EngResolve, // 分解
        S2C_EngResolve,
        S2C_UpdateEngParts // 更新引擎装备列表
    }
}