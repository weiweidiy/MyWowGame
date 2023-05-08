
using System;
using System.Collections;
using Framework.Pool;
using Logic.Fight;
using UnityEngine;

namespace Logic.Common
{
    /// <summary>
    /// 自动销毁GameObject 回收到FightObjPool对象池
    /// 定时销毁
    /// </summary>
    public class AutoReleasePoolObj : MonoBehaviour, IPoolObj
    {
        [Header("延迟销毁时间")]
        public float DelayTime = 1f;

        private WaitForSeconds WFS;
        private void Awake()
        {
            WFS = new WaitForSeconds(DelayTime);
        }

        public void OnSpawn()
        {
        }

        public void OnRecycle()
        {
        }
        
        private void OnEnable()
        {
            FightManager.Ins.StartCoroutine(DelayDestroy());
        }

        private IEnumerator DelayDestroy()
        {
            yield return WFS;
            FightObjPool.Ins.Recycle(gameObject);
        }
    }
}