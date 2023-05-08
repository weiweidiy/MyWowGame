
using System;
using Framework.Extension;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;
using UnityEngine;
using UnityTimer;

public class UIDailyTask : UIPage
{
    public GameObject m_ItemPrefabObj;
    public Transform m_ListRoot;
    public TextMeshProUGUI m_CDTimer;
    
    private void Awake()
    {
        m_EventGroup.Register(LogicEvent.DailyTaskListUpdate, OnListUpdate);
    }

    private void Start()
    {
        ShowAllTask();
    }

    private Timer m_Timer;
    private void OnEnable()
    {
        DateTime now = DateTime.Now;
        DateTime midnight = DateTime.Today.AddDays(1);
        TimeSpan timeLeft = midnight - now;
        int secondsLeft = (int)timeLeft.TotalSeconds;
        
        m_CDTimer.text = TimeHelper.FormatSecond(secondsLeft);

        m_Timer = Timer.Register(1f, () =>
        {
            secondsLeft--;
            if (secondsLeft <= 0)
            {
                secondsLeft = 24 * 60 * 60 - 1;
                return;
            }

            m_CDTimer.text = TimeHelper.FormatSecond(secondsLeft);
        }, null, true, true, this);
    }

    private void OnDisable()
    {
        m_Timer?.Cancel();
    }

    private void ShowAllTask()
    {
        foreach (var _taskData in TaskManager.Ins.m_DailyTaskList)
        {
            var _Obj = m_ItemPrefabObj.Clone(Vector3.zero, m_ListRoot, Quaternion.identity);
            _Obj.Show();
            var _Task = _Obj.GetComponent<DailyTaskCell>();
            _Task.Init(_taskData);
        }
    }

    private void OnListUpdate(int arg1, object arg2)
    {
        m_ListRoot.DestroyChildren();
        ShowAllTask();
    }
}
