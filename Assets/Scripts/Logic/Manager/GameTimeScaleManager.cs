using Chronos;
using Framework.Extension;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Manager
{
    public class GameTimeScaleManager : MonoSingleton<GameTimeScaleManager>
    {
        public enum TimeScaleModule
        {
            Null,
            Root,
            Battle,
        }


        Dictionary<TimeScaleModule, float> timeMap = new Dictionary<TimeScaleModule, float>();

        float defaultScale = 1f;

        protected override void Awake()
        {
            base.Awake();

            var globalClocks = GetComponentsInChildren<GlobalClock>();
            foreach(var clock in globalClocks)
            {
                var moduleName = GetModuleName(clock.key);
                timeMap.Add(moduleName, defaultScale);
            }
        }

        protected override void OnDestroy()
        {
            
        }


        TimeScaleModule GetModuleName(string key)
        {
            switch(key)
            {
                case "Root":
                    {
                        return TimeScaleModule.Root;
                    }

                case "Battle":
                    {
                        return TimeScaleModule.Battle;
                    }
                default:
                    {
                        throw new System.Exception($"没有实现时间模块 {key} 的转换方法！");
                    }
            }

        }

        /// <summary>
        /// 设置时间缩放
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="scale"></param>
        public void SetScale(TimeScaleModule moduleName, float scale)
        {
            timeMap[moduleName] = scale;
        }

        /// <summary>
        /// 暂停某个模块的计时
        /// </summary>
        /// <param name="moduleName"></param>
        public void Pause(TimeScaleModule moduleName)
        {
            SetScale(moduleName, 0f);
        }

        /// <summary>
        /// 恢复时间设置
        /// </summary>
        /// <param name="moduleName"></param>
        public void Resume(TimeScaleModule moduleName)
        {
            SetScale(moduleName, defaultScale);
        }

        
    }
}