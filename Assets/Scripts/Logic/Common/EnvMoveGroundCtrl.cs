using DG.Tweening;
using Framework.Extension;
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
        private void Awake()
        {
            m_CurrentSpeed = 0;
            
            m_EventGroup.Register(LogicEvent.Fight_MapMove, ((_, _) => ReStart()))
                 .Register(LogicEvent.Fight_MapStop, ((_, _) => Stop()));
        }
        
        void Update()
        {
            if(m_CurrentSpeed > 0)
                Move();
        }

        public void Stop()
        {
            DOTween.To(() => m_CurrentSpeed, pValue => m_CurrentSpeed = pValue, 0f, 0.5f);
        }

        public void ReStart()
        {
            DOTween.To(() => m_CurrentSpeed, pValue => m_CurrentSpeed = pValue, Speed, 0.6f);
        }

        void Move()
        {
            float _FinalSpeed = m_CurrentSpeed * Time.deltaTime;
            transform.Translate(-_FinalSpeed, 0, 0);
        }
    }
}