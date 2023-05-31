/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class LevelCfg
    {
        public Dictionary<string, LevelData> AllData;
        public static LevelData GetData(int pID)
        {
            return ConfigManager.Ins.m_LevelCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class LevelData
    {
        //关卡ID
        public int ID;

        //关卡攻击力基础值
        public int AtkBase;

        //攻击力成长系数
        public int AtkGrow;

        //关卡攻击体验系数
        public int AtkFeel;

        //体力基础值
        public int HPBase;

        //体力成长系数
        public int HPGrow;

        //体力体验系数
        public int HpFeel;

        //掉落基础值
        public int DropBase;

        //掉落成长系数
        public int DropGrow;

        //掉落体验系数
        public int DropFeel;

        //挂机怪物数量
        public int HangUpCount;

        //关卡普通怪物最小数量
        public int MinCount;

        //关卡普通怪物最大数量
        public int MaxCount;

        //关卡精英怪物最小数量
        public int EliteMinCount;

        //关卡精英怪物最大数量
        public int EliteMaxCount;

        //BOSS波怪物数量
        public int BOSSCount;

        //怪物HP变化系数
        public List<float> HPAdds;

        //怪物ATK变化系数
        public List<float> AtkAdds;

        /*
        KuMo:
    /// <summary>
    /// 普通关卡刷怪类型
    /// </summary>
    public enum SpawnType
    {
        Normal = 1,
        Elite = 2,
        Both = 3,
        Boss = 4,
        BossNormal = 5,
        All = 6,
    }
        */
        //波怪物类型
        public List<int> SpawnType;

        //怪物ATK权重
        public List<int> AtkWight;

        //怪物HP权重
        public List<int> HPWight;

        //怪物掉落权重
        public List<int> DropWight;

        /*
        Administrator:
场景类型
1-99配置：从Scene表读取组
100： 从Scene表1-99中随机一个

        */
        //普通怪怪场景
        public int MonsterScenes;

    }
}
