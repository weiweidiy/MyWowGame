using Framework.EventKit;
using Logic.Common;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{
    

    /// <summary>
    /// 每日任务红点逻辑
    /// </summary>
    public class DailyTaskComplete: RedDotLogic<GameTaskData>
    {

        public DailyTaskComplete() : base()
        { }

        /// <summary>
        /// 创建一个数据变更通知器
        /// </summary>
        /// <returns></returns>
        protected override IRedDotDataNotifier<GameTaskData> GetDataNotifier()
        {
            return new DailyTaskDataNotifier();
        }

        protected override RedDotStatus GetStatus(GameTaskData data)
        {
            return data.TaskState == (int)TaskState.Complete ? RedDotStatus.Normal : RedDotStatus.Null;
        }


        /// <summary>
        /// 获取UID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetUID(GameTaskData data)
        {
            return data.TaskID.ToString();
        }

        /// <summary>
        /// 日常任务数据，判断数据是否是日常任务，可以通过arg等方式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected override bool IsValidData(GameTaskData data, object arg)
        {
            if (data == null)
                return false;

            foreach (var info in cachInfos)
            {
                if (info.uid.Equals(GetUID(data)))
                    return true;
            }
            return false;
        }

    }



    public class DailyTaskDataNotifier : IRedDotDataNotifier<GameTaskData>
    {
        protected readonly EventGroup m_EventGroup = new();

        public event Action<GameTaskData, object> onDataChanged;

        public List<GameTaskData> GetDataList()
        {
            return TaskManager.Ins.m_DailyTaskList;
        }

        public void Init()
        {
            m_EventGroup.Register(LogicEvent.TaskStateChanged, (i, o) =>
            {
                var _TaskID = (int)o;
                onDataChanged?.Invoke(TaskManager.Ins.GetDailyTask(_TaskID), null);
            });

            m_EventGroup.Register(LogicEvent.DailyTaskChanged, (i, o) =>
            {
                var _TaskID = (int)o;
                onDataChanged?.Invoke(TaskManager.Ins.GetDailyTask(_TaskID), null);
            });
        }
    }
}
