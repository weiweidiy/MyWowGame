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

        /*
        攻击力=
怪物攻击力*10^关卡攻击力幂值
无限关0 攻击力公式=怪物攻击力*10^(关卡ID*无限关卡攻击力成长)
        */
        //关卡攻击力幂值
        public float AtkPower;

        /*
        体力=怪物体力*10^关卡体力幂值
无限关0 体力力公式=怪物体力*10^(关卡ID*无限关卡体力成长)
        */
        //关卡体力幂值
        public float HPPower;

        /*
        关卡掉落
有限关  金币掉落公式=掉落基础值*10^(关卡掉落幂值)
无限关0 金币掉落公式=掉落基础值*10^(关卡ID*无限关卡掉落成长)
        */
        //关卡掉落基础值
        public float DropBase;

        //关卡掉落幂值
        public float DropPower;

        //挂机阵型组
        public int HangupFormationGroup;

        //挂机攻击系数
        public float HangupAtk;

        //挂机体力系数
        public float HangupHP;

        //挂机掉落系数
        public float HangupDrop;

        //关卡阵型组ID
        public List<int> FormationGroupID;

        //波次攻击系数
        public List<float> WaveAtk;

        //波次体力系数
        public List<float> WaveHP;

        //波次掉落系数
        public List<float> WaveDrop;

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
