using DG.Tweening;
using Framework.Extension;
using UnityEngine;

namespace Logic.Common
{
    /// <summary>
    /// 场景背景移动控制
    /// </summary>
    public class EnvMoveCtrl : MonoWithEvent
    {
        [Header("速度-位置")]
        public float Speed = 0.1f;

        public float StartX;
        public float EndX;

        [Header("两张图片")] 
        public Transform F;
        public Transform S;

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
            F.Translate(-_FinalSpeed, 0, 0);
            S.Translate(-_FinalSpeed, 0, 0);

            if (F.transform.localPosition.x < EndX)
            {
                ReSetToStart(F.transform);
                ReSetToZero(S.transform);
            }
        
            if (S.transform.localPosition.x < EndX)
            {
                ReSetToStart(S.transform);
                ReSetToZero(F.transform);
            }
        }

        void ReSetToStart(Transform pTransform)
        {
            var _LocalPosition = pTransform.localPosition;
            _LocalPosition = new Vector3(StartX, _LocalPosition.y, _LocalPosition.z);
            pTransform.localPosition = _LocalPosition;
        }
    
        void ReSetToZero(Transform pTransform)
        {
            var _LocalPosition = pTransform.localPosition;
            _LocalPosition = new Vector3(0, _LocalPosition.y, _LocalPosition.z);
            pTransform.localPosition = _LocalPosition;
        }
    }
}