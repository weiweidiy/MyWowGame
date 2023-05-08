/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class TaskCfg
    {
        public Dictionary<string, TaskData> AllData;
        public static TaskData GetData(int pID)
        {
            return ConfigManager.Ins.m_TaskCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class TaskData
    {
        //任务ID
        public int ID;

        //任务类型
        public int TaskType;

        //目标数量基础值
        public int TargetBaseCount;

        //目标数量成长值
        public int TargetCountGrow;

        //目标数量体验值
        public int TargetCountExp;

        //奖励item类型
        public int RewardType;

        //奖励基础值
        public int RewardCountBase;

        //奖励成长值
        public int RewardCountGrow;

        //奖励体验值
        public int RewardCountExp;

    }
}
