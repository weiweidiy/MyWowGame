using System;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTimer;

namespace Logic.UI.UIMain
{
    public class UIMainRight : UIPage
    {
        #region 任务

        public TextMeshProUGUI m_TaskName;
        public TextMeshProUGUI m_TaskContent;
        public TextMeshProUGUI m_TaskReward;
        public GameObject m_TaskBreathObj;

        private TaskData m_TaskData;
        private TaskTypeData m_TaskTypeData;
        private GameTaskData m_GameTaskData;

        #endregion

        #region 放置奖励

        public Button m_BtnPlaceRewards;
        private Timer m_BtnPlaceRewardsTimer;
        private Timer m_PlaceRewardPopTimer;

        private int k_PlaceRewardBtnShowTime = GameDefine.PlaceRewardBtnShowTime * 60;
        private int m_GameExitTime; //记录离线时间

        #endregion

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.TaskStateChanged, (i, o) =>
            {
                var _TaskID = (int)o;
                if (_TaskID == m_GameTaskData.TaskID && m_GameTaskData.TaskState == (int)TaskState.Complete)
                {
                    m_TaskBreathObj.Show();
                }
            });

            m_EventGroup.Register(LogicEvent.MainTaskChanged, (i, o) => UpdateMainTask());
            m_EventGroup.Register(LogicEvent.MainTaskProcessChanged,
                (i, o) => { m_TaskContent.text = string.Format(m_TaskTypeData.TaskDes, GetTaskProgressStr()); });

            UpdateMainTask();

            m_BtnPlaceRewards.onClick.AddListener(OnBtnPlaceRewardClick);
        }

        private void Start()
        {
            StartPlaceRewardTime();
        }

        #region 任务

        private void UpdateMainTask()
        {
            m_GameTaskData = TaskManager.Ins.m_MainTaskData;
            m_TaskData = TaskCfg.GetData(m_GameTaskData.TaskID);
            m_TaskTypeData = TaskTypeCfg.GetData(m_TaskData.TaskType);

            m_TaskName.text = "任务 " + TaskManager.Ins.m_MainTaskCount;
            m_TaskContent.text = string.Format(m_TaskTypeData.TaskDes, GetTaskProgressStr());
            m_TaskReward.text = m_TaskData.RewardCountBase.ToString();

            if (m_GameTaskData.TaskState == (int)TaskState.Complete)
            {
                m_TaskBreathObj.Show();
            }
            else
            {
                m_TaskBreathObj.Hide();
            }
        }

        private string GetTaskProgressStr()
        {
            var _TaskProgress = m_GameTaskData.TaskProcess;
            var _TaskProgressMax = m_TaskData.TargetBaseCount;

            switch ((TaskType)m_TaskData.TaskType)
            {
                case TaskType.TT_1001:
                    return UICommonHelper.GetLevelNameByID(_TaskProgressMax);
                case TaskType.TT_2001:
                case TaskType.TT_2002:
                case TaskType.TT_5001:
                    return _TaskProgressMax.ToString();
            }

            return _TaskProgress + "/" + _TaskProgressMax;
        }

        public void OnClickMainTask()
        {
            if (m_GameTaskData.TaskState == (int)TaskState.Complete)
            {
                NetworkManager.Ins.SendMsg(
                    new C2S_TaskGetReward { IsMain = true, TaskID = m_GameTaskData.TaskID });
                //主线任务已领取变化
                EventManager.Call(LogicEvent.MainTaskDoneChanged, m_GameTaskData.TaskID);
            }
            else
            {
                //TODO UI 跳转
            }
        }

        #endregion

        public async void OnClickDailyTask()
        {
            await UIManager.Ins.OpenUI<UIDailyTask>();
        }

        #region 放置奖励

        private void StartPlaceRewardTime()
        {
            m_GameExitTime =
                TimeHelper.GetBetween(DateTime.UtcNow, GameDataManager.Ins.LastGameDate);
            StartShowPlaceRewardBtnTimer();
        }

        private void StartShowPlaceRewardBtnTimer()
        {
            // 计时器时间 + 离线时间
            var btnPlaceRewardShowTime = GameDataManager.Ins.BtnPlaceRewardShowTime + m_GameExitTime;
            if (btnPlaceRewardShowTime >= k_PlaceRewardBtnShowTime)
            {
                PlaceRewardPop();
            }
            else
            {
                var duration = k_PlaceRewardBtnShowTime - btnPlaceRewardShowTime;
                m_BtnPlaceRewardsTimer = Timer.Register(10f,
                    () =>
                    {
                        duration -= 10;
                        btnPlaceRewardShowTime += 10;
                        GameDataManager.Ins.BtnPlaceRewardShowTime = btnPlaceRewardShowTime;
                        if (duration <= 0)
                        {
                            m_BtnPlaceRewardsTimer.Cancel();
                            PlaceRewardPop();
                        }
                    },
                    isLooped: true, useRealTime: true);
            }
        }

        /// <summary>
        /// 处理每日上线后能领取放置奖励却没领情况
        /// </summary>
        private void PlaceRewardPop()
        {
            if (GameDataManager.Ins.PopPlaceRewardTime == DateTime.Today.Day)
            {
                m_BtnPlaceRewards.Show();
            }
            else
            {
                //在Home下才会主动弹出
                if (UIBottomMenu.Ins.m_BottomBtnType == BottomBtnType.Home)
                {
                    m_PlaceRewardPopTimer?.Cancel();
                    GameDataManager.Ins.PopPlaceRewardTime = DateTime.Today.Day;
                    OnBtnPlaceRewardClick();
                }
                else
                {
                    m_PlaceRewardPopTimer?.Cancel();
                    m_PlaceRewardPopTimer = Timer.Register(30f, PlaceRewardPop, isLooped: true, useRealTime: true);
                }
            }
        }

        private async void OnBtnPlaceRewardClick()
        {
            // 点击放置奖励按钮后相关数据清零
            GameDataManager.Ins.BtnPlaceRewardShowTime = 0;
            m_GameExitTime = 0;

            StartShowPlaceRewardBtnTimer();
            m_BtnPlaceRewards.Hide();
            await UIManager.Ins.OpenUI<UIPlaceRewards.UIPlaceRewards>();
        }

        #endregion
    }
}