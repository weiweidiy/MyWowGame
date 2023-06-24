/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class AttributeRandomCfg
    {
        public Dictionary<string, AttributeRandomData> AllData;
        public static AttributeRandomData GetData(int pID)
        {
            return ConfigManager.Ins.m_AttributeRandomCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class AttributeRandomData
    {
        //属性随机表ID
        public int ID;

        //系统类型
        public int Type;

        /*
        属性ID组随机抽1条
，所有属性等概率出现
随机2次，允许重复抽到同一条

        */
        //属性ID组
        public List<int> Attribute;

    }
}
