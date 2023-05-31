using Framework.EventKit;
using Logic.Common;
using Networks;
using System;
using System.Collections.Generic;

namespace Logic.Manager
{


    public class DailyTaskRedDot : IObservable<RedDotInfo>
    {
        private List<IObserver<RedDotInfo>> observers;
        private List<RedDotInfo> tasks;

        protected readonly EventGroup m_EventGroup = new();

        public DailyTaskRedDot()
        {
            observers = new List<IObserver<RedDotInfo>>();
            tasks = GetTasks(TaskManager.Ins.m_DailyTaskList);

        }



        private List<RedDotInfo> GetTasks(List<GameTaskData> m_DailyTaskList)
        {
            var result = new List<RedDotInfo>();

            foreach(var data in m_DailyTaskList)
            {
                result.Add(GetInfo(data)); 
            }

            return result;
        }

        RedDotInfo GetInfo(GameTaskData data)
        {
            var uid = data.TaskID.ToString();
            var isOn = data.TaskState == (int)TaskState.Complete;
            return new RedDotInfo(uid, isOn);
        }


        public IDisposable Subscribe(IObserver<RedDotInfo> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                foreach(var task in tasks)
                {
                    observer.OnNext(task);
                }
                
            }

            return new Unsubscriber<RedDotInfo>(observers, observer);
        }

    }









    public class DailyTaskComposeRedDot : IObservable<RedDotInfo>
    {
        private List<IObserver<RedDotInfo>> observers;
        private List<RedDotInfo> tasks;

        protected readonly EventGroup m_EventGroup = new();

        public DailyTaskComposeRedDot()
        {
            observers = new List<IObserver<RedDotInfo>>();
            tasks = GetTasks(TaskManager.Ins.m_DailyTaskList);


            //m_EventGroup.Register(LogicEvent.TaskStateChanged, (i, o) =>
            //{

            //});
        }

        private List<RedDotInfo> GetTasks(List<GameTaskData> m_DailyTaskList)
        {
            var result = new List<RedDotInfo>();

            foreach (var data in m_DailyTaskList)
            {
                result.Add(GetInfo(data));
            }

            return result;
        }

        RedDotInfo GetInfo(GameTaskData data)
        {
            var uid = data.TaskID.ToString();
            var isOn = data.TaskState == (int)TaskState.Complete;
            return new RedDotInfo(uid, isOn);
        }

        RedDotInfo GetInfo(string uid, bool isOn)
        {
            return new RedDotInfo(uid, isOn);
        }


        public IDisposable Subscribe(IObserver<RedDotInfo> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

                var isOn = false;
                foreach (var task in tasks)
                {
                    if(task.isOn)
                    {
                        isOn = true;
                        break;
                    }
                    
                }

                observer.OnNext(GetInfo("", isOn));

            }

            return new Unsubscriber<RedDotInfo>(observers, observer);
        }
    }
}