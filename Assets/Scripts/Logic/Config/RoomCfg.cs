/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class RoomCfg
    {
        public Dictionary<string, RoomData> AllData;
        public static RoomData GetData(int pID)
        {
            return ConfigManager.Ins.m_RoomCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class RoomData
    {
        //RoomID
        public int ID;

        //解锁ID
        public int LockID;

    }
}
