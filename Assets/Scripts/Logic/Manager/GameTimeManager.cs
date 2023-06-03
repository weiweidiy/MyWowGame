using System;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Networks;
using UnityTimer;

namespace Logic.Manager
{
    /// <summary>
    /// 时间管理
    /// </summary>
    public class GameTimeManager : Singleton<GameTimeManager>
    {
        private Timer m_DayTimer; //跨天计时器
        private long m_ServerTimer; //服务器当前时间
        private int m_NextDaySeconds; //距离下一天的秒数

        /// <summary>
        /// 初始化一些数据
        /// </summary>
        public void Init(S2C_Login pMsg)
        {
            m_DayTimer = null;
            m_ServerTimer = pMsg.ServerTimer;
            m_NextDaySeconds = (int)pMsg.NextDaySeconds;
        }

        /// <summary>
        /// 启动游戏定时器
        /// </summary>
        public void StartGameTimer()
        {
            DayTimerStart();
        }

        /// <summary>
        /// 关闭游戏定时器
        /// </summary>
        public void StopGameTimer()
        {
            DayTimerStop();
        }

        #region 跨天定时器模块

        /// <summary>
        /// 启动跨天计时器
        /// </summary>
        private void DayTimerStart()
        {
            var now = DateTime.Now;
            var midnight = DateTime.Today.AddDays(1);
            var timeLeft = midnight - now;
            var secondsLeft = (int)timeLeft.TotalSeconds;
            m_NextDaySeconds = secondsLeft;
            m_DayTimer = Timer.Register(1f, () =>
            {
                m_NextDaySeconds--;
                // 通知跨天UI更新
                EventManager.Call(LogicEvent.TimeNextDaySecondsChanged, m_NextDaySeconds);
                // 跨天
                if (m_NextDaySeconds <= 0)
                {
                    // 通知跨天事件
                    EventManager.Call(LogicEvent.TimeDayChanged, m_NextDaySeconds);
                    // 重置每日剩余时间
                    m_NextDaySeconds = 24 * 60 * 60;
                }
            }, isLooped: true, useRealTime: true);
        }

        /// <summary>
        /// 关闭跨天计时器
        /// </summary>
        private void DayTimerStop()
        {
            m_DayTimer?.Cancel();
            m_DayTimer = null;
        }

        #endregion

        #region 其他定时器模块

        #endregion
    }
}