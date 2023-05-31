/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class CopyOilCfg
    {
        public Dictionary<string, CopyOilData> AllData;
        public static CopyOilData GetData(int pID)
        {
            return ConfigManager.Ins.m_CopyOilCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class CopyOilData
    {
        /*
        Administrator:
0级: 通用公式
N级: 具体等级配置
        */
        //等级ID
        public int ID;

        //怪物形象
        public int ResGroupId;

        //副本名字
        public string CopyName;

        //消耗物品
        public int CostKey;

        //消耗数量
        public int CostNum;

        /*
        Administrator:
Group表格

        */
        //奖励组类型
        public int RewardGroup;

        /*
        Administrator:
固定奖励 item 7 原油

        */
        //奖励原油基础值
        public int RewardOilBase;

        //奖励原油成长值
        public int RewardOilGrow;

        //奖励原油体验值
        public int RewardOilExp;

        //BOSS血量基础值
        public int BOSSHPBase;

        //BOSS血量成长值
        public int BOSSHPGrow;

        //BOSS血量成长比例
        public float BOSSHPGrowMultiple;

        /*
        Administrator:
场景类型
1-99配置：从Scene表读取组
100： 从Scene表1-99中随机一个
100+：副本BOSS场景

        */
        //Boss怪场景
        public int BossScenes;

    }
}
