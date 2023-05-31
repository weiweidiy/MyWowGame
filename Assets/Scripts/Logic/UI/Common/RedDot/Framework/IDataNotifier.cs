using System;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{
    /// <summary>
    /// 数据变更通知器
    /// </summary>
    public interface IDataNotifier<T>
    {
        event Action<T,object> onDataChanged;

        void Init();

        List<T> GetDataList();

    }






    //bool CheckNeedNotify(RedDotObserver observer, RedDotInfo data)
    //{
    //    if (observer.EqualUid(""))
    //        return true;

    //    if (observer.EqualUid(data.uid))
    //        return true;

    //    return false;
    //}

    //void Notify(RedDotObserver observer, RedDotInfo data)
    //{
    //    if (observer.EqualUid(""))
    //    {
    //        //不关心id
    //        observer.OnNext(data);
    //    }
    //    else
    //    {
    //        if (observer.EqualUid(data.uid))
    //        {
    //            observer.OnNext(data);
    //        }
    //    }
    //}



    //public class DailyTaskComposeRedDot : IObservable<RedDotInfo>
    //{
    //    private List<IObserver<RedDotInfo>> observers;
    //    private List<RedDotInfo> tasks;

    //    protected readonly EventGroup m_EventGroup = new();

    //    public DailyTaskComposeRedDot()
    //    {
    //        observers = new List<IObserver<RedDotInfo>>();
    //        tasks = GetTasks(TaskManager.Ins.m_DailyTaskList);


    //        //m_EventGroup.Register(LogicEvent.TaskStateChanged, (i, o) =>
    //        //{

    //        //});
    //    }

    //    private List<RedDotInfo> GetTasks(List<GameTaskData> m_DailyTaskList)
    //    {
    //        var result = new List<RedDotInfo>();

    //        foreach (var data in m_DailyTaskList)
    //        {
    //            result.Add(GetInfo(data));
    //        }

    //        return result;
    //    }

    //    RedDotInfo GetInfo(GameTaskData data)
    //    {
    //        var uid = data.TaskID.ToString();
    //        var isOn = data.TaskState == (int)TaskState.Complete;
    //        return new RedDotInfo(uid, isOn,0);
    //    }

    //    RedDotInfo GetInfo(string uid, bool isOn)
    //    {
    //        return new RedDotInfo(uid, isOn,0);
    //    }


    //    public IDisposable Subscribe(IObserver<RedDotInfo> observer)
    //    {
    //        if (!observers.Contains(observer))
    //        {
    //            observers.Add(observer);

    //            var isOn = false;
    //            foreach (var task in tasks)
    //            {
    //                if(task.isOn)
    //                {
    //                    isOn = true;
    //                    break;
    //                }

    //            }

    //            observer.OnNext(GetInfo("", isOn));

    //        }

    //        return new Unsubscriber<RedDotInfo>(observers, observer);
    //    }
    //}
}