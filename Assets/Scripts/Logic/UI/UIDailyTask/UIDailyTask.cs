using Framework.Extension;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;
using UnityEngine;

public class UIDailyTask : UIPage
{
    public GameObject m_ItemPrefabObj;
    public Transform m_ListRoot;
    public TextMeshProUGUI m_CDTimer;

    private void Awake()
    {
        m_EventGroup.Register(LogicEvent.DailyTaskListUpdate, OnListUpdate);
        m_EventGroup.Register(LogicEvent.TimeNextDaySecondsChanged, OnTimeNextDaySecondsChanged);
    }

    private void Start()
    {
        ShowAllTask();
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

    /// <summary>
    /// 每日任务跨天倒计时刷新
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="data"></param>
    private void OnTimeNextDaySecondsChanged(int eventId, object data)
    {
        var secondsLeft = (int)data;
        m_CDTimer.text = TimeHelper.FormatSecond(secondsLeft);
    }
}