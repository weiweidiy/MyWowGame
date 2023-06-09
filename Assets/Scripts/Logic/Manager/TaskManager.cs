﻿using System.Collections.Generic;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Networks;

namespace Logic.Manager
{
    /// <summary>
    /// 客户端任务管理器
    /// </summary>
    public class TaskManager : Singleton<TaskManager>
    {
        public GameTaskData m_MainTaskData { get; private set; }
        public TaskData m_MainTaskCfg { get; private set; }

        public int m_MainTaskCount { get; private set; }
        public List<GameTaskData> m_DailyTaskList { get; private set; }

        private EventGroup m_EventGroup = new();

        public void Init(S2C_Login pLoginData)
        {
            m_MainTaskData = pLoginData.CurMainTask;
            m_MainTaskCount = pLoginData.MainTaskCount;
            m_DailyTaskList = pLoginData.DailyTaskList;

            m_MainTaskCfg = TaskCfg.GetData(m_MainTaskData.TaskID);

            //已经跨天 更新每日任务
            m_EventGroup.Register(LogicEvent.TimeDayChanged, (i, o) => SendC2S_RequestDailyTaskList());
        }

        public override void OnSingletonRelease()
        {
            m_EventGroup.Release();
        }

        public GameTaskData GetDailyTask(int taskId)
        {
            return m_DailyTaskList.Find(p => p.TaskID == taskId);
        }

        #region 消息处理

        public void OnUpdateTaskState(S2C_UpdateTaskState pMsg)
        {
            switch (pMsg.IsMain)
            {
                case true:
                    m_MainTaskData.TaskState = pMsg.TaskState;
                    break;
                case false:
                    var _TaskData = m_DailyTaskList.Find(p => p.TaskID == pMsg.TaskID);
                    if (_TaskData != null)
                    {
                        _TaskData.TaskState = pMsg.TaskState;
                    }

                    break;
            }

            EventManager.Call(LogicEvent.TaskStateChanged, pMsg.TaskID);
        }

        public void OnUpdateMainTask(S2C_UpdateMainTask pMsg)
        {
            m_MainTaskData = pMsg.TaskData;
            m_MainTaskCount = pMsg.MainTaskCount;
            m_MainTaskCfg = TaskCfg.GetData(m_MainTaskData.TaskID);
            EventManager.Call(LogicEvent.MainTaskChanged, m_MainTaskData);
        }

        public void OnUpdateDailyTask(S2C_UpdateDailyTask pMsg)
        {
            var _TaskData = m_DailyTaskList.Find(p => p.TaskID == pMsg.TaskID);
            if (_TaskData != null)
            {
                _TaskData.TaskState = (int)TaskState.Done;
            }

            EventManager.Call(LogicEvent.DailyTaskChanged, pMsg.TaskID);
        }

        public void OnRequestDailyTaskList(S2C_RequestDailyTaskList pMsg)
        {
            m_DailyTaskList = pMsg.DailyTaskList;
            EventManager.Call(LogicEvent.DailyTaskListUpdate);
        }

        #endregion

        #region 消息发送

        /// <summary>
        /// 已经跨天
        /// 向服务器发送更新每日任务列表协议
        /// </summary>
        public void SendC2S_RequestDailyTaskList()
        {
            NetworkManager.Ins.SendMsg(new C2S_RequestDailyTaskList());
        }

        //任务检查点回调
        public void DoTaskUpdate(TaskType pType, long pProcess = 1)
        {
            UpdateMain(pType, pProcess);
            UpdateDaily(pType, pProcess);
        }

        private void UpdateMain(TaskType pType, long pProcess = 1)
        {
            if (m_MainTaskCfg.TaskType == (int)pType)
            {
                switch ((TaskType)m_MainTaskCfg.TaskType)
                {
                    case TaskType.TT_1001:
                    case TaskType.TT_2001:
                    case TaskType.TT_2002:
                    case TaskType.TT_5001:
                        if (pProcess > m_MainTaskCfg.TargetBaseCount)
                            m_MainTaskData.TaskProcess = (int)pProcess;
                        break;
                    case TaskType.TT_4001:
                    case TaskType.TT_4002:
                    case TaskType.TT_4003:
                        m_MainTaskData.TaskProcess = (int)pProcess;
                        break;
                    default:
                        m_MainTaskData.TaskProcess += pProcess;
                        break;
                }

                EventManager.Call(LogicEvent.MainTaskProcessChanged);

                //没有完成的任务 需要同步任务进度保存
                if (m_MainTaskData.TaskState != (int)TaskState.Complete)
                {
                    NetworkManager.Ins.SendMsg(new C2S_UpdateTaskProcess
                    {
                        IsMain = true, TaskID = m_MainTaskData.TaskID, Process = m_MainTaskData.TaskProcess
                    });
                }
            }
        }

        private void UpdateDaily(TaskType pType, long pProcess = 1)
        {
            foreach (var gameTaskData in m_DailyTaskList)
            {
                var _TaskCfg = TaskCfg.GetData(gameTaskData.TaskID);
                if (_TaskCfg.TaskType != (int)pType)
                    continue;
                if (gameTaskData.TaskState != (int)TaskState.Process)
                    continue;

                gameTaskData.TaskProcess += pProcess;

                EventManager.Call(LogicEvent.DailyTaskProcessChanged, gameTaskData.TaskID);

                if (gameTaskData.TaskState != (int)TaskState.Complete)
                {
                    NetworkManager.Ins.SendMsg(new C2S_UpdateTaskProcess
                    {
                        IsMain = false, TaskID = gameTaskData.TaskID, Process = gameTaskData.TaskProcess
                    });
                }
            }
        }

        #endregion
    }
}