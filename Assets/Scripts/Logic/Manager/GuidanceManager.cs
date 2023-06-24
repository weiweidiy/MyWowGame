using System.Collections.Generic;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Networks;

namespace Logic.Manager
{
    public class GuidanceManager : Singleton<GuidanceManager>
    {
        private readonly EventGroup eventGroup = new EventGroup();
        public GameTaskData mainTaskData { get; private set; }

        /* TODO:配置表软引导配置
         * 软引导
         * 任务ID列表
         * 触发路径列表
         * 完成路径(列表)
         */
        private GuidanceType taskGuidanceType = GuidanceType.Soft;

        private List<int> taskIDList = new List<int>() { 2, 3, 4, 7, 10, 13, 14, 15, 18, 21, 24 };

        private List<string> processPathList = new List<string>()
        {
            "UIRoot/Normal/UIRoom(Clone)/Scroll View/Viewport/Content/RoomATK/m_Btn",
            "",
            "UIRoot/Normal/UIRoom(Clone)/Scroll View/Viewport/Content/RoomHP/m_Btn",
            "",
            "UIRoot/Normal/UIRoom(Clone)/Scroll View/Viewport/Content/RoomHPRecover/m_Btn",
            "UIRoot/Normal/UIRoom(Clone)/Scroll View/Viewport/Content/RoomSpeed/m_Btn",
            "",
            "",
            "UIRoot/Normal/UIRoom(Clone)/Scroll View/Viewport/Content/RoomCritical/m_Btn",
            "UIRoot/Normal/UIRoom(Clone)/Scroll View/Viewport/Content/RoomCriticalDamage/m_Btn",
            ""
        };

        private string completePath = "UIRoot/BackGround/UIMainRight(Clone)/TaskNode";


        public void Init()
        {
            //注册
            eventGroup.Register(LogicEvent.TaskStateChanged, OnTaskStateChanged);
            eventGroup.Register(LogicEvent.MainTaskChanged, OnMainTaskChanged);
            eventGroup.Register(LogicEvent.MainTaskDoneChanged, OnMainTaskDoneChanged);

            //更新任务引导
            mainTaskData = TaskManager.Ins.m_MainTaskData;
            UpdateGuidance(mainTaskData.TaskID);
        }

        public override void OnSingletonRelease()
        {
            eventGroup.Release();
        }

        //主线任务变化
        private void OnMainTaskChanged(int eventId, object pData)
        {
            //更新任务引导
            mainTaskData = (GameTaskData)pData;
            UpdateGuidance(mainTaskData.TaskID);
        }

        //主线任务状态变化
        private void OnTaskStateChanged(int eventId, object pData)
        {
            //更新任务引导
            var taskID = (int)pData;
            UpdateGuidance(taskID);
        }

        private void UpdateGuidance(int pID)
        {
            if (!taskIDList.Contains(pID)) return;

            //获取当前任务ID在数组中的下标
            var index = taskIDList.IndexOf(mainTaskData.TaskID);
            var path = string.Empty;

            //设置任务在进行状态手指路径
            if ((TaskState)TaskManager.Ins.m_MainTaskData.TaskState == TaskState.Process)
            {
                path = processPathList[index];
            }

            //设置任务在完成状态手指路径
            if ((TaskState)TaskManager.Ins.m_MainTaskData.TaskState == TaskState.Complete)
            {
                path = completePath;
            }

            //设置传参数据
            var data = (taskGuidanceType, path);
            //路径不为空时开启引导
            if (path != string.Empty)
            {
                //开启引导页面
                EventManager.Call(LogicEvent.GuidanceStart, data);
            }
        }

        //主线任务已领取变化
        private void OnMainTaskDoneChanged(int eventId, object pData)
        {
            if (taskIDList.Contains((int)pData))
            {
                //关闭引导页面
                EventManager.Call(LogicEvent.GuidanceEnd);
            }
        }
    }
}