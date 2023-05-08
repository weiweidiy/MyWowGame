/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class CopyCoinCfg
    {
        public Dictionary<string, CopyCoinData> AllData;
        public static CopyCoinData GetData(int pID)
        {
            return ConfigManager.Ins.m_CopyCoinCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class CopyCoinData
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

        //怪物形象
        public int ResGroupId;

        //BOSS总体力基础值
        public int BOSSHPBase;

        //BOSS总体力成长值
        public int BOSSHPGrow;

        //BOSS总体力体验值
        public int BOSSHPExp;

    }
}
