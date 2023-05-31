using System.Collections.Generic;
using Configs;
using Logic.Common;
using Networks;
using UnityEngine;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        // 第一次进游戏 初始化任务数据
        public void InitTask(DummyDB pDB)
        {
            pDB.m_MainTaskCount = 1;
            pDB.m_CurMainTask = new GameTaskData
            {
                TaskID = GetNextMainTaskID(pDB.m_MainTaskCount),
                TaskState = (int)TaskState.Process,
                TaskProcess = 0,
            };
            
            RecountTaskProcess(pDB.m_CurMainTask, TaskCfg.GetData(pDB.m_CurMainTask.TaskID));
            
            pDB.m_DailyTaskList = new List<GameTaskData>(32);
            for (int i = 900001; i <= 900011; i++)
            {
                var _Data = new GameTaskData
                {
                    TaskID = i,
                    TaskState = (int)TaskState.Process,
                    TaskProcess = 0,
                };
                RecountTaskProcess(_Data, TaskCfg.GetData(i));
                
                pDB.m_DailyTaskList.Add(_Data);
            }
        }

        #region 消息处理

        public void On_C2S_UpdateTaskProcess(C2S_UpdateTaskProcess pMsg)
        {
            if (pMsg.IsMain)
            {
                if(pMsg.TaskID == m_DB.m_CurMainTask.TaskID)
                {
                    m_DB.m_CurMainTask.TaskProcess = pMsg.Process;
                    var _NeedCount = TaskCfg.GetData(pMsg.TaskID).TargetBaseCount;
                    if(pMsg.Process >= _NeedCount)
                    {
                        m_DB.m_CurMainTask.TaskState = (int)TaskState.Complete;
                        SendMsg(new S2C_UpdateTaskState{ TaskID = pMsg.TaskID, IsMain = true, TaskState = (int)TaskState.Complete });
                    }
                }
                else
                {
                    Debug.LogError("Error Task ID : " + pMsg.TaskID);
                }
            }
            else
            {
                foreach (var gameTaskData in m_DB.m_DailyTaskList)
                {
                    if (gameTaskData.TaskID == pMsg.TaskID)
                    {
                        gameTaskData.TaskProcess = pMsg.Process;
                        var _NeedCount = TaskCfg.GetData(gameTaskData.TaskID).TargetBaseCount;
                        if(pMsg.Process >= _NeedCount)
                        {
                            gameTaskData.TaskState = (int)TaskState.Complete;
                            SendMsg(new S2C_UpdateTaskState{ TaskID = pMsg.TaskID, IsMain = false, TaskState = (int)TaskState.Complete });
                        }
                        break;
                    }
                }
            }
            
            DummyDB.Save(m_DB);
        }

        private void On_C2S_TaskGetReward(C2S_TaskGetReward pMsg)
        {
            if (pMsg.IsMain)
            {
                var _TaskData = TaskCfg.GetData(pMsg.TaskID);
                if(pMsg.TaskID == m_DB.m_CurMainTask.TaskID && 
                   m_DB.m_CurMainTask.TaskState == (int)TaskState.Complete)
                {
                     UpdateDiamond(_TaskData.RewardCountBase);
                     m_DB.m_MainTaskCount++;
                     m_DB.m_CurMainTask = new GameTaskData
                     {
                         TaskID = GetNextMainTaskID(m_DB.m_MainTaskCount),
                         TaskState = (int)TaskState.Process,
                         TaskProcess = 0,
                     };
                     RecountTaskProcess(m_DB.m_CurMainTask, TaskCfg.GetData(m_DB.m_CurMainTask.TaskID));
                     SendMsg(new S2C_UpdateMainTask
                     {
                         TaskData = m_DB.m_CurMainTask,
                         MainTaskCount = m_DB.m_MainTaskCount,
                     });
                }
                else
                {
                    Debug.LogError("Error Task ID : " + pMsg.TaskID);
                }
            }
            else
            {
                foreach (var gameTaskData in m_DB.m_DailyTaskList)
                {
                    if (gameTaskData.TaskID == pMsg.TaskID)
                    {
                        var _TaskData = TaskCfg.GetData(pMsg.TaskID);
                        if (gameTaskData.TaskState == (int)TaskState.Complete)
                        {
                            gameTaskData.TaskState = (int)TaskState.Done;
                            UpdateDiamond(_TaskData.RewardCountBase);
                            SendMsg(new S2C_UpdateDailyTask
                            {
                                TaskID = pMsg.TaskID
                            });
                        }
                        else
                        {
                            Debug.LogError("Error Task ID : " + pMsg.TaskID);
                        }
                        break;
                    }
                }
            }
        }

        private void On_C2S_RequestDailyTaskList(C2S_RequestDailyTaskList pMsg)
        {
            m_DB.m_DailyTaskList.Clear();
            m_DB.m_DailyTaskList = new List<GameTaskData>(32);
            
            var _Msg = new S2C_RequestDailyTaskList
            {
                DailyTaskList = new List<GameTaskData>(32),
            };
            
            for (int i = 900001; i <= 900010; i++)
            {
                var _Data = new GameTaskData
                {
                    TaskID = i,
                    TaskState = (int)TaskState.Process,
                    TaskProcess = 0,
                };
                RecountTaskProcess(_Data, TaskCfg.GetData(i));
                
                m_DB.m_DailyTaskList.Add(_Data);
                
                _Msg.DailyTaskList.Add(_Data.Clone());
            }
            
            SendMsg(_Msg);
        }
        
        #endregion
        
        /// <summary>
        /// 部分技能接收的时候 就要计算一下是否已经完成了
        /// </summary>
        public void RecountTaskProcess(GameTaskData pData, TaskData pTaskData)
        {
            if (pData.TaskState != (int)TaskState.Process)
            {
                return;
            }
            
            var _NeedCount = pTaskData.TargetBaseCount;
            switch ((TaskType)pTaskData.TaskType)
            {
                case TaskType.TT_1001://完成关卡进度-累计
                    pData.TaskProcess = m_DB.m_CurLevelID;
                    if(m_DB.m_CurLevelID > pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_2001: //完成钻石副本进度-累计
                    pData.TaskProcess = m_DB.m_DiamondCopyData.Level;
                    if(m_DB.m_DiamondCopyData.Level > pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_2002: //完成金币副本进度-累计
                    pData.TaskProcess = m_DB.m_CoinCopyData.Level;
                    if(m_DB.m_CoinCopyData.Level > pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_3001://升级攻击力进度-累计
                    pData.TaskProcess = m_DB.m_GJJAtkLevel;
                    if(m_DB.m_GJJAtkLevel >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_3002://升级血量进度-累计
                    pData.TaskProcess = m_DB.m_GJJHPLevel;
                    if(m_DB.m_GJJHPLevel >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_3003://升级恢复进度-累计
                    pData.TaskProcess = m_DB.m_GJJHPRecoverLevel;
                    if(m_DB.m_GJJHPRecoverLevel >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_3004://升级暴击率进度-累计
                    pData.TaskProcess = m_DB.m_GJJCriticalLevel;
                    if(m_DB.m_GJJCriticalLevel >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_3005://升级暴击伤害进度-累计
                    pData.TaskProcess = m_DB.m_GJJCriticalDamageLevel;
                    if(m_DB.m_GJJCriticalDamageLevel >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_3006://升级攻速进度-累计
                    pData.TaskProcess = m_DB.m_GJJAtkSpeedLevel;
                    if(m_DB.m_GJJAtkSpeedLevel >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_3007://升级二连击进度-累计
                    pData.TaskProcess = m_DB.m_GJJDoubleHitLevel;
                    if(m_DB.m_GJJDoubleHitLevel >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_3008://升级三连击进度-累计
                    pData.TaskProcess = m_DB.m_GJJTripletHitLevel;
                    if(m_DB.m_GJJTripletHitLevel >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_4001://召唤装备进度-累计
                    pData.TaskProcess = m_DB.m_ShopEquipData.TotalExp;
                    if(m_DB.m_ShopEquipData.TotalExp >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_4002://召唤技能进度-累计
                    pData.TaskProcess = m_DB.m_ShopSkillData.TotalExp;
                    if(m_DB.m_ShopSkillData.TotalExp >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_4003://召唤伙伴进度-累计
                    pData.TaskProcess = m_DB.m_ShopPartnerData.TotalExp;
                    if(m_DB.m_ShopPartnerData.TotalExp >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_5001://考古层-累计
                    pData.TaskProcess = m_DB.m_MiningData.FloorCount;
                    if(m_DB.m_MiningData.FloorCount >= pTaskData.TargetBaseCount)
                    {
                        pData.TaskState = (int)TaskState.Complete;
                    }
                    break;
                case TaskType.TT_5002://研究级-累计
                    break;
            }
        }
        
        /// <summary>
        /// 获取下一个主线任务ID
        /// </summary>
        public static int GetNextMainTaskID(int pTaskCount)
        {
            int _Mod = 150;
            int _Add = 1;
            int _Min = 1;
            if (pTaskCount > 150 )
            {
                _Mod = 5;
                _Add = 100001;
                _Min = 151;
            }
            var _ID = (pTaskCount - _Min) % _Mod + _Add;
            return _ID;
        }
    }
}