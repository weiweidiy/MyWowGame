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

        //描述
        public string Des;

        //品质
        public int Quality;

        //资源
        public int Res;

        /*
        1	研究
2	DJ
3	DJ组合
4	司机/英雄
5	英雄突破树
6	改造副本-英雄加成
7	料理
8	引擎
9	火花塞
10	气缸
11	改造副本战斗属性
        */
        //属性归属系统类型
        public int SystemType;

    }
}
