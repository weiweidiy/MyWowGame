using Chronos;
using Framework.Extension;
using System;
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

        /// <summary>
        /// 默认的时间缩放
        /// </summary>
        float defaultScale = 1f;


        GlobalClock[] globalClocks;



        protected override void Awake()
        {
            if (Ins != null)
            {
                Debug.LogError("重复单例 :" + Ins.gameObject.name);
                Destroy(gameObject);
                return;
            }
            Ins = this;

            globalClocks = GetComponentsInChildren<GlobalClock>();
        }



        /// <summary>
        /// 设置时间缩放
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="scale"></param>
        public void SetScale(TimeScaleModule moduleName, float scale)
        {
            var clock = GetClock(moduleName);
            clock.localTimeScale = scale;
        }

        /// <summary>
        /// 获取时钟
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        private GlobalClock GetClock(TimeScaleModule moduleName)
        {
            foreach(var clock in globalClocks)
            {
                if(clock.key.Equals(moduleName.ToString()))
                {
                    return clock;

                }
            }

            return null;
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


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Pause(TimeScaleModule.Battle);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume(TimeScaleModule.Battle);
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetScale(TimeScaleModule.Battle, 2f);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetScale(TimeScaleModule.Battle, 3f);
            }
        }

        //public float DeltaTime()
        //{

        //}
    }
}