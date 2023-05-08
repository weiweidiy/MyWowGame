/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class EngineLvlUpCfg
    {
        public Dictionary<string, EngineLvlUpData> AllData;
        public static EngineLvlUpData GetData(int pID)
        {
            return ConfigManager.Ins.m_EngineLvlUpCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class EngineLvlUpData
    {
        //引擎等级ID
        public int ID;

        //升级消耗基础值
        public int LvlUpBaseCost;

        //升级消耗加成
        public int LvlUpAdditionCost;

        //改造次数消耗加成
        public int ReformAdditionCost;

        //分解基础值
        public int DecomposeBase;

        //分解等级加成
        public int DecomposeLvlAddition;

        //改造次数加成
        public int DecomposeReformAddition;

    }
}
