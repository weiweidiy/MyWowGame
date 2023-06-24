/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class CylinderCfg
    {
        public Dictionary<string, CylinderData> AllData;
        public static CylinderData GetData(int pID)
        {
            return ConfigManager.Ins.m_CylinderCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class CylinderData
    {
        //气缸ID
        public int ID;

        /*
        Administrator:
1 基础属性类
2 技能类
3 伙伴类
4 特殊类
        */
        //类型
        public int Type;

        /*
        0 白
1 绿
2 蓝
3 紫
4 橙
5 青
6 红

        */
        //气缸品质
        public int Quilty;

        //气缸名称
        public string Name;

        //引擎攻击加成比
        public float EngineAtk;

        //随机表ID
        public int RandomAttribute;

        //ResID
        public int ResID;

        //分解科技点
        public int Decompose;

    }
}
