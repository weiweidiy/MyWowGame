using Framework.Extension;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Common.RedDot
{
    public class BigBoomRedDotManager : RedDotManager , ISingleton
    {

        public override void Init()
        {
            //注册逻辑
            mapObservables.Add(RedDotKey.MainTaskComplete.ToString(), new MainTaskComplete()); //主线任务红点逻辑
            mapObservables.Add(RedDotKey.DailyTaskComplete.ToString(), new DailyTaskComplete()); //日常任务红点逻辑
            mapObservables.Add(RedDotKey.SpoilSlotEquipable.ToString(), new SpoilSlotEquipable()); //可装备战利品槽位
            mapObservables.Add(RedDotKey.EquipWeaponUpgradable.ToString(), new EquipWeaponUpgradable()); //可升级武器
            mapObservables.Add(RedDotKey.EquipArmorUpgradable.ToString(), new EquipArmorUpgradable()); //可升级防具
            mapObservables.Add(RedDotKey.EquipWeaponEquipable.ToString(), new EquipWeaponEquipable()); //可装备武器
            mapObservables.Add(RedDotKey.EquipArmorEquipable.ToString(), new EquipArmorEquipable()); //可装备防具
            mapObservables.Add(RedDotKey.SkillUpgradable.ToString(), new SkillUpgradable()); //可强化技能
            mapObservables.Add(RedDotKey.PartnerUpgradable.ToString(), new PartnerUpgradable()); //可强化伙伴
        }



        /// <summary>
        /// 静态实例
        /// </summary>
        protected static BigBoomRedDotManager mInstance;

        /// <summary>
        /// 标签锁：确保当一个线程位于代码的临界区时，另一个线程不进入临界区。
        /// 如果其他线程试图进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放
        /// </summary>
        static object mLock = new object();

        /// <summary>
        /// 静态属性
        /// </summary>
        public static BigBoomRedDotManager Ins
        {
            get
            {
                lock (mLock)
                {
                    if (mInstance == null)
                    {
                        mInstance = SingletonCreator.CreateSingleton<BigBoomRedDotManager>();
                    }
                }

                return mInstance;
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public virtual void Dispose()
        {
            mInstance = null;
        }



        /// <summary>
        /// 单例初始化方法
        /// </summary>
        public virtual void OnSingletonInit()
        {
        }

    }

}