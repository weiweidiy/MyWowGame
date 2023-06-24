/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class FormationCfg
    {
        public Dictionary<string, FormationData> AllData;
        public static FormationData GetData(int pID)
        {
            return ConfigManager.Ins.m_FormationCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class FormationData
    {
        //FormationID
        public int ID;

        /*
        Administrator:
1 挂机阵型
2 主线关卡
3 钻石副本
4 金币副本
5 料理副本
6 音乐副本
7 改造副本
        */
        //阵型类型
        public int Type;

        /*
        Administrator:
怪物ID序列

        */
        //Monster列表
        public List<int> MonsterList;

        /*
        Administrator:
刷新间隔时间 ms

        */
        //Monster刷新间隔
        public List<int> RefreshTime;

        /*
        刷新点
1 2 3 地面,3 最低
11 12 13 空中, 13最高
        */
        //Monster刷新点
        public List<int> RefreshPoint;

        /*
        Administrator:
阵型速度


        */
        //Monster移动速度
        public List<float> Speed;

        /*
        Monster掉落比例
阵型类型 1 2 时读取该字段
1 怪物掉落=挂机掉落*Monster掉落比例
2 怪物掉落=关卡掉落*Monster掉落比例
        */
        //Monster掉落比例
        public List<float> Drop;

    }
}
