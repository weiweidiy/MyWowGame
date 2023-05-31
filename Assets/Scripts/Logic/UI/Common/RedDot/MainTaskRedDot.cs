using Framework.EventKit;
using Logic.Common;
using Networks;
using System;
using System.Collections.Generic;

namespace Logic.Manager
{
    public class MainTaskRedDot : IObservable<RedDotInfo>
    {
        private List<IObserver<RedDotInfo>> observers;
        private RedDotInfo mainTask;

        protected readonly EventGroup m_EventGroup = new();

        public MainTaskRedDot()
        {
            observers = new List<IObserver<RedDotInfo>>();
            mainTask = GetInfo(TaskManager.Ins.m_MainTaskData);


            m_EventGroup.Register(LogicEvent.MainTaskChanged, (i, o) => UpdateMainTask(o as GameTaskData));
            m_EventGroup.Register(LogicEvent.TaskStateChanged, (i, o) =>
            {
                var _TaskID = (int)o;
                if (_TaskID.ToString().Equals(mainTask.uid))
                {
                    UpdateMainTask(TaskManager.Ins.m_MainTaskData);
                }
            });
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

                //通知新增加的观察者缓存数据

                observer.OnNext(mainTask);
            }

            return new Unsubscriber<RedDotInfo>(observers, observer);
        }


        void Notify(IObserver<RedDotInfo> observer, RedDotInfo info)
        {

        }



        private void UpdateMainTask(GameTaskData data)
        {
            mainTask = GetInfo(data);
            foreach (var observer in observers)
            {
                observer.OnNext(mainTask);
            }
        }
    }

}