/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class PartnerCfg
    {
        public Dictionary<string, PartnerData> AllData;
        public static PartnerData GetData(int pID)
        {
            return ConfigManager.Ins.m_PartnerCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class PartnerData
    {
        //伙伴ID
        public int ID;

        //品质
        public int Quality;

        //伙伴名称
        public string PartnerName;

        //拥有加成基础值
        public float HasAdditionBase;

        //拥有加成成长比例
        public float HasAdditionGrow;

        //伙伴攻击力基础分赃比
        public float AtkBase;

        //伙伴攻击力分赃比成长比例
        public float AtkGrow;

        //伙伴攻击速度
        public float AtkSpeed;

        //出现舱室
        public List<int> Rooms;

        //舱室权重
        public List<int> RoomWight;

        //移动速度区间
        public List<int> Speed;

        /*
        Administrator:
巡逻积极性
0,待机
越高越积极

        */
        //状态切换时间
        public int Patrol;

        /*
        Administrator:
巡逻积极性
0,待机
越高越积极

        */
        //偷懒时间
        public int LazyTime;

        //资源
        public string ResourceName;

        //伙伴合成ID
        public int CombineID;

        //伙伴合成数量
        public int CombineNum;

    }
}
