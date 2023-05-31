using Logic.Config;
using System;
using System.Collections.Generic;

namespace Configs
{
    public partial class SceneCfg
    {
        public static List<SceneData> GetSceneDataList(Func<SceneData, bool> perdicate)
        {
            List<SceneData> result = new List<SceneData>();

            var keys = ConfigManager.Ins.m_SceneCfg.AllData.Keys;
            foreach(var key in keys)
            {
                var sceneData = ConfigManager.Ins.m_SceneCfg.AllData[key];
                if( perdicate(sceneData))
                {
                    result.Add(sceneData);
                }
            }

            return result;
        }
    }
}