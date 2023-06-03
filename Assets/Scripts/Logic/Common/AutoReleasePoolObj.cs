
using System;
using System.Collections;
using Framework.Pool;
using Logic.Fight;
using UnityEngine;
using DG.Tweening;
using Chronos;
using Logic.Manager;

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

        Timeline m_TimeLine;
        private void Awake()
        {
            WFS = new WaitForSeconds(DelayTime);

            m_TimeLine = GetComponent<Timeline>();
        }

        public void OnSpawn()
        {
            
        }

        public void OnRecycle()
        {
        }

        float curTime = 0f;

        private void Update()
        {
            Debug.Assert(m_TimeLine != null, "没有找到timeline" + gameObject);
            float deltaTime = m_TimeLine ? m_TimeLine.deltaTime : Time.deltaTime;
            deltaTime = deltaTime * Time.timeScale;
            if (curTime >= DelayTime)
            {
                //Debug.LogError("回收 " + gameObject);
                FightObjPool.Ins.Recycle(gameObject);
                return;
            }

            curTime += deltaTime;
        }

        private void OnEnable()
        {
            curTime = 0;


        }

        private IEnumerator DelayDestroy()
        {
            yield return WFS;
            Debug.LogError("回收 " + gameObject);
            FightObjPool.Ins.Recycle(gameObject);
        }
    }
}