/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class AttributeCfg
    {
        public Dictionary<string, AttributeData> AllData;
        public static AttributeData GetData(int pID)
        {
            return ConfigManager.Ins.m_AttributeCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class AttributeData
    {
        //属性ID
        public int ID;

        //属性类型
        public int Type;

        //属性名称
        public string Name;

        /*
        Administrator:
百分比
1011 攻击力加成1%

        */
        //属性值
        public float Value;

    }
}
