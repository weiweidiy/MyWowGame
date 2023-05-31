/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections.Generic;
using Logic.Config;

namespace Configs
{
    public partial class SceneCfg
    {
        public Dictionary<string, SceneData> AllData;
        public static SceneData GetData(int pID)
        {
            return ConfigManager.Ins.m_SceneCfg.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;
        }
    }

    public class SceneData
    {
        //场景ID
        public int ID;

        /*
        Administrator:
1-100
常规场景随机组
101+
副本场景组

        */
        //场景组
        public int ScenesGroup;

        //场景类型
        public int ScenesType;

        /*
        Administrator:
场景类型
1.前景
2.近景
3.中景
4.后景
5.远景
6.背景
        */
        //层级类型
        public int LayersType;

        /*
        Administrator:
读表Res

        */
        //场景资源ID
        public int ScenesRes;

    }
}
