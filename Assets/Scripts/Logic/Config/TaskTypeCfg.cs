/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class TaskTypeCfg
    {
        public Dictionary<string, TaskTypeData> AllData;
        public static TaskTypeData GetData(int pID)
        {
            return ConfigManager.Ins.m_TaskTypeCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class TaskTypeData
    {
        //任务类型
        public int ID;

        //任务描述
        public string TaskDes;

    }
}
