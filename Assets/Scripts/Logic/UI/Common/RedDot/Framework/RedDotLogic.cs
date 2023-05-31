using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Common.RedDot
{
    public abstract class RedDotLogic<TData> : IObservable<RedDotInfo>
    {
        /// <summary>
        /// 指定id的观察者
        /// </summary>
        List<RedDotObserver> observers;

        /// <summary>
        /// 不关心id的观察者
        /// </summary>
        List<RedDotObserver> observersWithoutUID;

        /// <summary>
        /// 红点数据
        /// </summary>
        protected List<RedDotInfo> cachInfos;

        /// <summary>
        /// 数据更新通知器
        /// </summary>
        IRedDotDataNotifier<TData> dataNotifier = null;

        public RedDotLogic()
        {
            observers = new List<RedDotObserver>();
            observersWithoutUID = new List<RedDotObserver>();

            dataNotifier = GetDataNotifier();
            dataNotifier.Init();
            dataNotifier.onDataChanged += OnDataChanged;

            cachInfos = GetInfoList();
        }

        /// <summary>
        /// 获取数据变更通知器
        /// </summary>
        /// <returns></returns>
        protected abstract IRedDotDataNotifier<TData> GetDataNotifier();

        /// <summary>
        /// 获取数据的唯一ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract string GetUID(TData data);

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract RedDotStatus GetStatus(TData data);

        /// <summary>
        /// 数据转换成reddotinfo，核心方法，就是最底层的 红点逻辑
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual RedDotInfo GetCellNodeRedDotInfo(TData data)
        {
            var status = GetStatus(data);

            var number = GetNumber(status);

            RedDotInfo info = new RedDotInfo(GetUID(data), status, number);

            return info;
        }


        /// <summary>
        /// 根据其他红点状态创建一个不关注uid的红点数据，就是各种常见的 父亲页签
        /// </summary>
        /// <param name="lstCacheInfo"></param>
        /// <returns></returns>
        protected virtual RedDotInfo GetParentNodeRedDotInfo(List<RedDotInfo> lstCacheInfo)
        {
            RedDotInfo resultInfo = new RedDotInfo("", RedDotStatus.Null, 0);

            //遍历红点状态，只要有一个是亮的，就通知亮，否则通知不亮
            for (int i = 0; i < lstCacheInfo.Count; i++)
            {
                var cachInfo = lstCacheInfo[i];

                if (cachInfo.status == RedDotStatus.Normal)
                {
                    resultInfo.status = RedDotStatus.Number;
                    resultInfo.number += cachInfo.number;
                }
            }
            return resultInfo;
        }


        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        protected virtual int GetNumber(RedDotStatus status)
        {
            return status == RedDotStatus.Null ? 0 : 1;
        }

        /// <summary>
        /// 数据发生变化了
        /// </summary>
        /// <param name="obj"></param>
        protected void OnDataChanged(TData data, object arg)
        {
            if (IsValidData(data, arg))
            {
                //更新 tasks , 然后发通知
                var info = UpdateInfo(data);
                var observer = GetObserverInterestUID(data);
                NotifyObserver(observer, info);

                //更新不关注UID的观察者
                var infoWithoutUID = GetParentNodeRedDotInfo(cachInfos);
                NotifyObserversUninterestUID(infoWithoutUID);
            }
        }


        /// <summary>
        /// 更新红点数据方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual RedDotInfo UpdateInfo(TData data)
        {
            RedDotInfo result = GetNewInfo(data);

            bool needAdd = true;
            for (int i = 0; i < cachInfos.Count; i++)
            {

                if (cachInfos[i].uid.Equals(GetUID(data)))
                {
                    result = GetCellNodeRedDotInfo(data);
                    cachInfos[i] = result;
                    needAdd = false;

                    break;
                }
            }

            if (needAdd == true)
            {
                cachInfos.Add(result);
            }

            return result;
        }

        /// <summary>
        /// 获得新数据时的红点信息，默认是标记普通红点状态, 子类可以重写
        /// </summary>
        /// <param name="newData"></param>
        /// <returns></returns>
        protected virtual RedDotInfo GetNewInfo(TData newData)
        {
            var status = GetStatus(newData);
            RedDotInfo result = new RedDotInfo(GetUID(newData), status, GetNumber(status));
            return result;
        }

        /// <summary>
        /// 是否有效数据,大部分情况默认即可，像Task消息 maintask和dailytask共用数据的，需要有效数据类型判断区分
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool IsValidData(TData data, object arg)
        {
            if (data == null)
                return false;

            return true;
        }

        /// <summary>
        /// 添加观察者
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<RedDotInfo> observer)
        {
            //转换成实际的红点观察者
            var redDotObserver = observer as RedDotObserver;

            //不关注ID的观察者
            if (redDotObserver.UID.Equals(""))
            {
                //加入列表
                if (!observersWithoutUID.Contains(redDotObserver))
                {
                    observersWithoutUID.Add(redDotObserver);

                    //把缓存数据通知给不关注ID的观察者
                    RedDotInfo info = GetParentNodeRedDotInfo(cachInfos);
                    NotifyObserver(redDotObserver, info);
                }

                return new UnSubScriberRedDot(observersWithoutUID, redDotObserver);
            }

            //关注ID的观察者
            if (!observers.Contains(redDotObserver))
            {
                observers.Add(redDotObserver);

                var info = GetInfoFromCache(redDotObserver.UID);
                NotifyObserver(redDotObserver, info);
            }

            return new UnSubScriberRedDot(observers, redDotObserver);
        }

        /// <summary>
        /// 通知关注UID的观察者
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="info"></param>
        protected void NotifyObserver(RedDotObserver observer, RedDotInfo info)
        {
            if (observer == null)
                return;

            observer.OnNext(info);
        }

        /// <summary>
        /// 通知所有不关注UID的观察者，数据变更
        /// </summary>
        /// <param name="info"></param>
        protected void NotifyObserversUninterestUID(RedDotInfo info)
        {
            foreach (var observer in observersWithoutUID)
            {
                NotifyObserver(observer, info);
            }
        }

        #region get方法
        /// <summary>
        /// 获取数据，将原始数据按规则转换成红点数据
        /// </summary>
        /// <returns></returns>
        protected List<RedDotInfo> GetInfoList()
        {
            var result = new List<RedDotInfo>();

            foreach (var data in dataNotifier.GetDataList())
            {
                result.Add(GetCellNodeRedDotInfo(data));
            }

            return result;
        }

        /// <summary>
        /// 获取关注指定uid的观察者
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        protected RedDotObserver GetObserverInterestUID(TData data)
        {
            return observers.Find((observer) => observer.EqualUid(GetUID(data)));
        }

        /// <summary>
        /// 根据uid返回一个info对象
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        RedDotInfo GetInfoFromCache(string uid)
        {
            for (int i = 0; i < cachInfos.Count; i++)
            {
                var info = cachInfos[i];

                if (info.uid.Equals(uid))
                    return info;
            }

            return new RedDotInfo("", RedDotStatus.Null, 0);
        }
        #endregion
    }



}