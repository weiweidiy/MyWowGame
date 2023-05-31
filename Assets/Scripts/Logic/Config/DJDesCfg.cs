/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class DJDesCfg
    {
        public Dictionary<string, DJDesData> AllData;
        public static DJDesData GetData(int pID)
        {
            return ConfigManager.Ins.m_DJDesCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class DJDesData
    {
        //DJDesID
        public int ID;

        //DJ属性
        public int AttributeID;

        /*
        Administrator:
7.SS
6.S
5.A
4.B
3.C
2.D
1.E
0.F

        */
        //品质
        public int Quality;

        //DJ描述
        public string DJDes;

    }
}
