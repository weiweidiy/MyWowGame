/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class GameDefineCfg
    {
        public Dictionary<string, GameDefineData> AllData;
        public static GameDefineData GetData(int pID)
        {
            return ConfigManager.Ins.m_GameDefineCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class GameDefineData
    {
        //ID
        public int ID;

        //整数值类型
        public int IntValue;

        //浮点值类型
        public float floatValue;

        //字符串类型
        public string StringValue;

    }
}
