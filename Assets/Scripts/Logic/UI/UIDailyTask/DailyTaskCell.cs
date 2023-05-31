
using System.Collections;
using Configs;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Common.RedDot;
using Logic.Manager;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyTaskCell : MonoWithEvent
{
    public GameObject m_CantBK;
    public GameObject m_CanBK;
    public TextMeshProUGUI m_RewardCount;
    public GameObject m_CanProcess;
    public Image m_TaskProcess;
    public GameObject m_TextProcess;
    public TextMeshProUGUI m_TaskName;

    public GameObject m_BtnGet;
    public GameObject m_BtnGetYet;
    public GameObject m_BtnCantGet;
    public GameObject m_YetObj;


    private TaskData m_TaskData;
    private TaskTypeData m_TaskTypeData;
    private GameTaskData m_GameTaskData;


    
    private void Awake()
    {
        m_EventGroup.Register(LogicEvent.TaskStateChanged, (i, o) =>
        {
            var _TaskID = (int)o;
            if (_TaskID == m_GameTaskData.TaskID)
                UIManager.Ins.StartCoroutine(Refresh());
        });
        
        m_EventGroup.Register(LogicEvent.DailyTaskProcessChanged, (i, o) =>
        {
            var _TaskID = (int)o;
            if (_TaskID == m_GameTaskData.TaskID)
            {
                var _S = m_TaskTypeData.TaskDes;
                var _S1 = _S.Replace("\n", "");
                m_TaskName.text = string.Format(_S1, GetTaskProgressStr());
                m_TaskProcess.fillAmount = m_GameTaskData.TaskProcess / (float)m_TaskData.TargetBaseCount;
            }
        });
        
        m_EventGroup.Register(LogicEvent.DailyTaskChanged, (i, o) =>
        {
            var _TaskID = (int)o;
            if (_TaskID == m_GameTaskData.TaskID)
                UIManager.Ins.StartCoroutine(Refresh());
        });
    }
    
    public void Init(GameTaskData pTaskData)
    {
        m_GameTaskData = pTaskData;
        m_TaskData = TaskCfg.GetData(m_GameTaskData.TaskID);
        m_TaskTypeData = TaskTypeCfg.GetData(m_TaskData.TaskType);
        m_RewardCount.text = m_TaskData.RewardCountBase.ToString();

        UIManager.Ins.StartCoroutine(Refresh());
    }

    private IEnumerator Refresh()
    {
        var _S = m_TaskTypeData.TaskDes;
        var _S1 = _S.Replace("\n", "");
        m_TaskName.text = string.Format(_S1, GetTaskProgressStr());



        m_CantBK.Hide();
        m_CanBK.Hide();
        m_CanProcess.Hide();
        m_TaskProcess.Hide();
        m_TextProcess.Hide();
        m_BtnGet.Hide();
        m_BtnGetYet.Hide();
        m_BtnCantGet.Hide();
        m_YetObj.Hide();
        
        switch ((TaskState)m_GameTaskData.TaskState)
        {
            case TaskState.Process:
            {
                m_CantBK.Show();
                m_TaskProcess.Show();
                m_TaskProcess.fillAmount = m_GameTaskData.TaskProcess / (float)m_TaskData.TargetBaseCount;
                m_BtnCantGet.Show();
            }
                break;
            case TaskState.Complete:
            {
                m_CanBK.Show();
                m_CanProcess.Show();
                m_TextProcess.Show();
                m_BtnGet.Show();
                yield return null;
                gameObject.transform.SetAsFirstSibling();
            }
                break;
            case TaskState.Done:
            {
                m_CanBK.Show();
                m_CanProcess.Show();
                m_TextProcess.Show();
                m_BtnGetYet.Show();
                m_YetObj.Show();
                yield return null;
                gameObject.transform.SetAsLastSibling();
            }
                break;
        }
    }
    
    private string GetTaskProgressStr()
    {
        var _TaskProgress = m_GameTaskData.TaskProcess;
        var _TaskProgressMax = m_TaskData.TargetBaseCount;

        // switch ((TaskType)m_TaskData.TaskType)
        // {
        //     case TaskType.TT_1001:
        //         return UICommonHelper.GetLevelNameByID(_TaskProgressMax);
        //     case TaskType.TT_2001:
        //     case TaskType.TT_2002:
        //         return _TaskProgressMax.ToString();
        // }
        return (_TaskProgress > _TaskProgressMax ? _TaskProgressMax :  _TaskProgress) + "/" + _TaskProgressMax;
    }

    public void OnClickGet()
    {
        if (m_GameTaskData.TaskState == (int)TaskState.Complete)
        {
            NetworkManager.Ins.SendMsg(new C2S_TaskGetReward { IsMain = false, TaskID = m_GameTaskData.TaskID });
        }
    }
}
