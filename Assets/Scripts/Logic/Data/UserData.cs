﻿using BreakInfinity;
using Logic.Common;
using Networks;

namespace Logic.Data
{
    /// <summary>
    /// 玩家全部游戏数据
    /// 运行时全局数据
    /// </summary>
    public class UserData
    {
        #region 公用逻辑数据

        public long m_LastGameDate; //上次登录时间
        public ServerTimes m_ServerTimes; // 服务器当前时间

        #endregion

        #region 货币

        public BigDouble m_Coin; //游戏币
        public long m_Diamond; //金币
        public int m_Oil; //原油
        public BigDouble m_Trophy; //战利品
        public int m_MushRoom; //英雄升级蘑菇
        public int m_BreakOre; //英雄突破矿石
        public int m_BreakTP; //英雄突破天赋点
        public int m_TecPoint; //引擎升级科技点

        #endregion

        #region 通用设置等

        public bool m_AutoSkill; //自动技能
        public bool m_IsMusicOn; //音乐开关
        public bool m_IsSoundOn; //音效开关

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

        #region 关卡数据

        public long m_CurLevelID; //当前关卡ID
        public int m_CurLevelNode; //当前关卡节点(1-5 5 BOSS)
        public LevelState m_LevelState; //关卡状态

        #endregion

        #region 放置奖励数据

        public long m_BtnPlaceRewardClickTime; //放置奖励按钮点击时间戳
        public float m_BtnPlaceRewardShowTime; // 放置奖励按钮显示时间
        public int m_PopPlaceRewardTime; // 放置奖励页面每日主动弹出时间

        #endregion
    }
}