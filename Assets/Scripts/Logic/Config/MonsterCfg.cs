/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class MonsterCfg
    {
        public Dictionary<string, MonsterData> AllData;
        public static MonsterData GetData(int pID)
        {
            return ConfigManager.Ins.m_MonsterCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class MonsterData
    {
        //MonsterID
        public int ID;

        /*
        Administrator:
1 普通怪
2 精英怪
3  BOSS
        */
        //Monster类型
        public int Type;

        /*
        Administrator:
1 地面怪
2 空中怪
        */
        //Monster位置类型
        public int MonsterPosType;

        //Res组ID
        public int ResGroupID;

        //Hp
        public float HPWight;

        //Atk
        public float AtkWight;

        //攻击速度
        public float AtkSpeed;

        /*
        怪物特殊属性
0 没有特殊属性
其他 读取怪物属性表(暂未实现)
        */
        //怪物属性
        public int Attribute;

        /*
        怪物特殊技能
0 没有技能
其他 读取怪物技能表(暂未实现)
        */
        //怪物技能
        public int Skill;

        /*
        怪物死亡掉落特效
引用Res表

        */
        //掉落特效
        public int DropEffect;

    }
}
