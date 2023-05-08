/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SkillCfg
    {
        public Dictionary<string, SkillData> AllData;
        public static SkillData GetData(int pID)
        {
            return ConfigManager.Ins.m_SkillCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SkillData
    {
        //技能ID
        public int ID;

        //品质
        public int Quality;

        //技能名称
        public string SkillName;

        /*
        Administrator:
显示为百分比

        */
        //拥有加成基础值
        public float HasAdditionBase;

        //拥有加成成长比例
        public float HasAdditionGrow;

        //Icon资源
        public string ResourceName;

        //技能CD
        public int CD;

        //技能持续时间
        public float DurationTime;

        //技能描述
        public string SkillDes;

        //技能基础伤害
        public int DamageBase;

        //技能伤害成长
        public int DamageGrow;

        //Prefab资源
        public string PrefabResName;

    }
}
