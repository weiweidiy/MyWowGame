/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class StoryCfg
    {
        public Dictionary<string, StoryData> AllData;
        public static StoryData GetData(int pID)
        {
            return ConfigManager.Ins.m_StoryCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class StoryData
    {
        //剧情ID
        public int ID;

        //下一个剧情
        public int NextID;

        //对话
        public string Dialogue;

    }
}
