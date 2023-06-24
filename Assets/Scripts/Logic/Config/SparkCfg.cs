/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SparkCfg
    {
        public Dictionary<string, SparkData> AllData;
        public static SparkData GetData(int pID)
        {
            return ConfigManager.Ins.m_SparkCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SparkData
    {
        //火花塞ID
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
        //火花塞品质
        public int Quality;

        //火花塞名称
        public string Name;

        //引擎体力加成比
        public float EngineHP;

        //火花塞随机表ID
        public int RandomAttribute;

        //ResID
        public int ResID;

        //分解科技点
        public int Decompose;

    }
}
