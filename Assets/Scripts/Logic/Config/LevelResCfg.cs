/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class LevelResCfg
    {
        public Dictionary<string, LevelResData> AllData;
        public static LevelResData GetData(int pID)
        {
            return ConfigManager.Ins.m_LevelResCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class LevelResData
    {
        //ID
        public int ID;

        //地面怪资源组ID
        public int ResGroupId;

        //BOSS资源
        public string BOSSRes;

        //背景资源
        public string EnvRes;

    }
}
