/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class ReformFightAttributeCfg
    {
        public Dictionary<string, ReformFightAttributeData> AllData;
        public static ReformFightAttributeData GetData(int pID)
        {
            return ConfigManager.Ins.m_ReformFightAttributeCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class ReformFightAttributeData
    {
        //战斗属性ID
        public int ID;

        //属性ID
        public int AttributeID;

    }
}
