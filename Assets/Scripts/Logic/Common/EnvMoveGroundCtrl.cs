using Chronos;
using DG.Tweening;
using Framework.Extension;
using System;
using UnityEngine;

namespace Logic.Common
{
    /// <summary>
    /// 场景背景移动控制
    /// </summary>
    public class EnvMoveGroundCtrl : MonoWithEvent
    {
        [Header("速度-位置")]
        public float Speed = 0.1f;
        
        private float m_CurrentSpeed;

        Timeline m_TimeLine;
        private void Awake()
        {
            m_CurrentSpeed = 0;
            m_TimeLine = GetComponent<Timeline>();

            m_EventGroup.Register(LogicEvent.Fight_MapMove, ((_, _) => ReStart()))
                 .Register(LogicEvent.Fight_MapStop, ((_, _) => Stop()))
                .Register(LogicEvent.Fight_MapMoveBack, ((_, _) => MoveBack()));

        }

        private void MoveBack()
        {
            DOTween.To(() => m_CurrentSpeed, pValue => m_CurrentSpeed = pValue, 0f, 0.5f).SetUpdate(UpdateType.Manual);
        }

        void Update()
        {
            if(m_CurrentSpeed > 0)
                Move();
        }

        public void Stop()
        {
            m_CurrentSpeed = 0;
            //DOTween.To(() => m_CurrentSpeed, pValue => m_CurrentSpeed = pValue, 0f, 0.5f);
        }

        public void ReStart()
        {
            DOTween.To(() => m_CurrentSpeed, pValue => m_CurrentSpeed = pValue, Speed, 0.6f).SetUpdate(UpdateType.Manual);
        }

        void Move()
        {
            Debug.Assert(m_TimeLine != null, "没有找到timeline" + gameObject.name);
            float deltaTime = m_TimeLine ? m_TimeLine.deltaTime : Time.deltaTime;
            deltaTime *= Time.timeScale;

            float _FinalSpeed = m_CurrentSpeed * deltaTime;// Time.deltaTime;
            transform.Translate(-_FinalSpeed, 0, 0);
        }
    }
}