using System;
using System.Collections.Generic;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Networks;
using UnityTimer;

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
        
        public void Init(S2C_Login pLoginData)
        {
            m_MainTaskData = pLoginData.m_CurMainTask;
            m_MainTaskCount = pLoginData.m_MainTaskCount;
            m_DailyTaskList = pLoginData.m_DailyTaskList;
            
            m_MainTaskCfg = TaskCfg.GetData(m_MainTaskData.m_TaskID);
            
            //已经跨天 重置每日任务
            //TODO 临时在客户端实现
            if(GameDataManager.Ins.LastGameDate.Day != DateTime.Now.Day)
            {
                m_DailyTaskList.Clear();
                NetworkManager.Ins.SendMsg(new C2S_RequestDailyTaskList());
            }

            StartTimer();
        }

        #region 定时器
        private void StartTimer()
        {
            DateTime now = DateTime.Now;
            DateTime midnight = DateTime.Today.AddDays(1);
            TimeSpan timeLeft = midnight - now;
            int secondsLeft = (int)timeLeft.TotalSeconds;
        
            Timer.Register(secondsLeft, () =>
            {
                NetworkManager.Ins.SendMsg(new C2S_RequestDailyTaskList());
                OnTimeUp();
            }, null, false, true);
        }

        private void OnTimeUp()
        {
            var secondsLeft = 24 * 60 * 60;
            Timer.Register(secondsLeft, () =>
            {
                NetworkManager.Ins.SendMsg(new C2S_RequestDailyTaskList());
                OnTimeUp();
            }, null, false, true);
        }
        #endregion

        #region 消息处理

        public void OnUpdateTaskState(S2C_UpdateTaskState pMsg)
        {
            switch (pMsg.m_IsMain)
            {
                case true:
                    m_MainTaskData.m_TaskState = pMsg.m_TaskState;
                    break;
                case false:
                    var _TaskData = m_DailyTaskList.Find(p => p.m_TaskID == pMsg.m_TaskID);
                    if (_TaskData != null)
                    {
                        _TaskData.m_TaskState = pMsg.m_TaskState;
                    }
                    break;
            }
            EventManager.Call(LogicEvent.TaskStateChanged, pMsg.m_TaskID);
        }
        
        public void OnUpdateMainTask(S2C_UpdateMainTask pMsg)
        {
            m_MainTaskData = pMsg.m_TaskData;
            m_MainTaskCount = pMsg.m_MainTaskCount;
            m_MainTaskCfg = TaskCfg.GetData(m_MainTaskData.m_TaskID);
            EventManager.Call(LogicEvent.MainTaskChanged);
        }
        
        public void OnUpdateDailyTask(S2C_UpdateDailyTask pMsg)
        {
            var _TaskData = m_DailyTaskList.Find(p => p.m_TaskID == pMsg.m_TaskID);
            if (_TaskData != null)
            {
                _TaskData.m_TaskState = (int)TaskState.Done;
            }
            EventManager.Call(LogicEvent.DailyTaskChanged, pMsg.m_TaskID);
        }
        
        public void OnRequestDailyTaskList(S2C_RequestDailyTaskList pMsg)
        {
            m_DailyTaskList = pMsg.m_DailyTaskList;
            EventManager.Call(LogicEvent.DailyTaskListUpdate);
        }

        #endregion
        
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
                            m_MainTaskData.m_TaskProcess = (int)pProcess;
                        break;
                    case TaskType.TT_4001:
                    case TaskType.TT_4002:
                    case TaskType.TT_4003:
                        m_MainTaskData.m_TaskProcess = (int)pProcess;
                        break;
                    default:
                        m_MainTaskData.m_TaskProcess += pProcess;
                        break;
                }
                
                EventManager.Call(LogicEvent.MainTaskProcessChanged);
                
                //没有完成的任务 需要同步任务进度保存
                if(m_MainTaskData.m_TaskState != (int)TaskState.Complete)
                {
                    NetworkManager.Ins.SendMsg(new C2S_UpdateTaskProcess
                    {
                        m_IsMain = true, m_TaskID = m_MainTaskData.m_TaskID, m_Process = m_MainTaskData.m_TaskProcess
                    });
                }
            }
        }
        
        private void UpdateDaily(TaskType pType, long pProcess = 1)
        {
            foreach (var gameTaskData in m_DailyTaskList)
            {
                var _TaskCfg = TaskCfg.GetData(gameTaskData.m_TaskID);
                if(_TaskCfg.TaskType != (int)pType)
                    continue;
                if(gameTaskData.m_TaskState != (int)TaskState.Process)
                    continue;
                
                gameTaskData.m_TaskProcess += pProcess;
                
                EventManager.Call(LogicEvent.DailyTaskProcessChanged, gameTaskData.m_TaskID);
            
                if(gameTaskData.m_TaskState != (int)TaskState.Complete)
                {
                    NetworkManager.Ins.SendMsg(new C2S_UpdateTaskProcess
                    {
                        m_IsMain = false, m_TaskID = gameTaskData.m_TaskID, m_Process = gameTaskData.m_TaskProcess
                    });
                }
            }
        }
    }
}