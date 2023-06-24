using System;
using Framework.Core;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Networks;
using UnityTimer;

namespace Logic.Manager
{
    public class GameTimeManager : Singleton<GameTimeManager>
    {
        //服务器时间
        public ServerTimes m_ServerTimes;

        //计时器
        private Timer m_Timer;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pMsg"></param>
        public void Init()
        {
            m_ServerTimes = GameDataManager.Ins.ServerTimes;
            m_Timer = null;
            //单机模式不启用游戏定时器
            if (GameCore.Ins.UseDummyServer) return;
            TimerStart();
        }

        /// <summary>
        /// 请求服务器时间
        /// </summary>
        public void RequestServerTimes()
        {
            NetworkManager.Ins.SendMsg(new C2S_ST());
        }

        /// <summary>
        /// 接收服务器时间
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnReceiveServerTimes(S2C_ST pMsg)
        {
            //关闭定时器
            TimerStop();
            //刷新服务器时间
            m_ServerTimes = pMsg.ST;
            //启动定时器
            TimerStart();
        }

        /// <summary>
        /// 定时器开启
        /// </summary>
        private void TimerStart(int duration = 1)
        {
            //计算各计时模块剩余时间
            var dayLeftSeconds = (int)(m_ServerTimes.DayET - m_ServerTimes.ServerTimer);
            var weekLeftSeconds = (int)(m_ServerTimes.WeekET - m_ServerTimes.ServerTimer);

            m_Timer = Timer.Register(duration, () =>
            {
                //更新各计时模块剩余时间
                dayLeftSeconds -= duration;
                weekLeftSeconds -= duration;
                //进入各计时模块计时流程
                TimerProcedure(
                    dayLeftSeconds,
                    () => { EventManager.Call(LogicEvent.TimeDaySecondsChanged, dayLeftSeconds); },
                    () =>
                    {
                        RequestServerTimes();
                        EventManager.Call(LogicEvent.TimeDayChanged);
                    });
                TimerProcedure(
                    weekLeftSeconds,
                    () => { EventManager.Call(LogicEvent.TimeWeekSecondsChanged, weekLeftSeconds); },
                    () =>
                    {
                        RequestServerTimes();
                        EventManager.Call(LogicEvent.TimeWeekChanged);
                    });
            }, isLooped: true, useRealTime: true);
        }

        /// <summary>
        /// 定时器流程
        /// </summary>
        /// <param name="leftSeconds"></param>
        /// <param name="action"></param>
        /// <param name="completeAction"></param>
        private void TimerProcedure(int leftSeconds, Action action, Action completeAction)
        {
            //流程中委托
            action?.Invoke();
            if (leftSeconds > 0) return;
            //流程结束后委托
            completeAction?.Invoke();
        }

        /// <summary>
        /// 定时器关闭
        /// </summary>
        private void TimerStop()
        {
            m_Timer?.Cancel();
            m_Timer = null;
        }
    }
}