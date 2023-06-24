/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class ResCfg
    {
        public Dictionary<string, ResData> AllData;
        public static ResData GetData(int pID)
        {
            return ConfigManager.Ins.m_ResCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class ResData
    {
        //形象ID
        public int ID;

        //形象组ID
        public int ResGroupId;

        //资源
        public string Res;

    }
}
