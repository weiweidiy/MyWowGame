using Framework.EventKit;
using Logic.Common;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{

    public class MainTaskComplete : RedDotLogic<GameTaskData>
    {

        protected override IRedDotDataNotifier<GameTaskData> GetDataNotifier()
        {
            return new MainTaskDataNotifier();
        }

        protected override string GetUID(GameTaskData data)
        {
            return data.TaskID.ToString();
        }


        protected override RedDotStatus GetStatus(GameTaskData data)
        {
            return data.TaskState == (int)TaskState.Complete ? RedDotStatus.Normal : RedDotStatus.Null;
        }


        protected override RedDotInfo GetParentNodeRedDotInfo(List<RedDotInfo> lstCacheInfo)
        {
            var info = lstCacheInfo[0];
            return info;
        }


        /// <summary>
        /// 参数是更新来源传过来的
        /// </summary>
        /// <param name="data"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected override bool IsValidData(GameTaskData data, object arg)
        {
            var isTaskStateChanged = (bool)arg;
            if(isTaskStateChanged)
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
            return true;
        }

        protected override RedDotInfo UpdateInfo(GameTaskData data)
        {
            var info = GetCellNodeRedDotInfo(data);
            cachInfos[0] = info;
            return info;
        }

    }



    public class MainTaskDataNotifier : IRedDotDataNotifier<GameTaskData>
    {
        protected readonly EventGroup m_EventGroup = new();

        public event Action<GameTaskData, object> onDataChanged;

        public List<GameTaskData> GetDataList()
        {
            var result = new List<GameTaskData>();
            result.Add(TaskManager.Ins.m_MainTaskData);
            return result;
        }

        public void Init()
        {
            m_EventGroup.Register(LogicEvent.MainTaskChanged, (i, o) =>
            {
                //过来的是一个新的id,
                onDataChanged?.Invoke(o as GameTaskData, false);
            });

            m_EventGroup.Register(LogicEvent.TaskStateChanged, (i, o) =>
            {
                //过来的是一个老的id,
                var _TaskID = (int)o;
                onDataChanged?.Invoke(TaskManager.Ins.m_MainTaskData, true);
            });
        }
    }
}