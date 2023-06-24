﻿/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class CopyReformCfg
    {
        public Dictionary<string, CopyReformData> AllData;
        public static CopyReformData GetData(int pID)
        {
            return ConfigManager.Ins.m_CopyReformCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class CopyReformData
    {
        //副本ID
        public int ID;

        //难度系数
        public int Diff;

        //奖励基础值
        public int RewardBase;

        //奖励成长值
        public int RewardGrow;

        //奖励体验值
        public int RewardExp;

        //波次怪物最小数量
        public int MonsterCountMin;

        //波次怪物最大数量
        public int MonsterCountMax;

        //怪物形象组
        public int ResGroupId;

        //BOSS总攻击基础值
        public int BOSSAtkBase;

        //BOSS总攻击成长值
        public int BOSSAtkGrow;

        //BOSS总攻击体验值
        public int BOSSAtkExp;

        //BOSS总体力基础值
        public int BOSSHPBase;

        //BOSS总体力成长值
        public int BOSSHPGrow;

        //BOSS总体力体验值
        public int BOSSHPExp;

        /*
        Administrator:
场景类型
1-99配置：从Scene表读取组
100： 从Scene表1-99中随机一个
100+：副本BOSS场景

        */
        //Boss怪场景
        public int BossScenes;

        //气缸品质随机组
        public int CylinderLevelGroup;

    }
}
