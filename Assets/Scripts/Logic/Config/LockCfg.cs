/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class LockCfg
    {
        public Dictionary<string, LockData> AllData;
        public static LockData GetData(int pID)
        {
            return ConfigManager.Ins.m_LockCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class LockData
    {
        //解锁ID
        public int ID;

        //解锁关卡ID
        public int UnlockLvl;

        //解锁类型
        public string SysType;

        //解锁描述
        public string UnlockDes;

        //剧情ID
        public int StoryID;

    }
}
