/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class EquipCfg
    {
        public Dictionary<string, EquipData> AllData;
        public static EquipData GetData(int pID)
        {
            return ConfigManager.Ins.m_EquipCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class EquipData
    {
        //装备ID
        public int ID;

        //品质
        public int Quality;

        //类型
        public int EquipType;

        //装备名称
        public string EquipName;

        //初始拥有加成
        public float HasAdditionBase;

        //拥有加成成长
        public float HasAdditionGrow;

        //初始配备加成
        public float ValueBase;

        //配备加成成长
        public float ValueGrow;

        //资源
        public string ResourceName;

    }
}
