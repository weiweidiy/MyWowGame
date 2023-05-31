/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class DJComboCfg
    {
        public Dictionary<string, DJComboData> AllData;
        public static DJComboData GetData(int pID)
        {
            return ConfigManager.Ins.m_DJComboCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class DJComboData
    {
        //DJComboID
        public int ID;

        //音轨类型
        public int MelodyType;

        //乐章等级
        public int MelodyLvl ;

        //组合数量
        public int ComboCount;

        //属性
        public int AttributeID;

        //属性描述
        public string AttributeDes;

        //音频
        public string Audio;

    }
}
