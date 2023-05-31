using Framework.Extension;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Logic.Manager.RedDotManager;

namespace Logic.Manager
{

    /// <summary>
    /// 红点管理:
    /// 负责监视数据的状态变化
    /// 负责监视事件的通知
    /// 条件变更：有可装备的
    /// </summary>
    public class RedDotManager : Singleton<RedDotManager>
    {
        public enum RedDotType
        {
            /// <summary>
            /// 主线任务完成未领取
            /// </summary>
            MainTaskRedDotChanged,

            /// <summary>
            /// 每日任务完成未领取
            /// </summary>
            DailyTaskRedDotChanged,

        }


        Dictionary<RedDotType, List<IObserver<RedDotInfo>>> mapObservers = new Dictionary<RedDotType, List<IObserver<RedDotInfo>>>();

        Dictionary<RedDotType, IObservable<RedDotInfo>> mapObservables = new Dictionary<RedDotType, IObservable<RedDotInfo>>();


        public void Init()
        {
            mapObservables = new Dictionary<RedDotType, IObservable<RedDotInfo>>();
            mapObservables.Add(RedDotType.MainTaskRedDotChanged, new MainTaskRedDot());
            mapObservables.Add(RedDotType.DailyTaskRedDotChanged, new DailyTaskRedDot());
        }


        public void Regist(RedDotType type, Action<RedDotInfo> action)
        {
            var observer = new RedDotObserver(action);

            if (!mapObservers.ContainsKey(type))
            {
                var observers = new List<IObserver<RedDotInfo>>();
                observers.Add(observer);
                mapObservers.Add(type, observers);
            }
            else
            {
                var observers = mapObservers[type];
                Debug.Assert(!observers.Contains(observer), "包含重复的观察者！" + observer);
                observers.Add(observer);
            }

            
            //订阅指定类型的红点事件
            mapObservables[type].Subscribe(observer);
        }


        public void RegistComposeNode(RedDotType[] types, Action<RedDotInfo> action)
        {

        }
    }

}