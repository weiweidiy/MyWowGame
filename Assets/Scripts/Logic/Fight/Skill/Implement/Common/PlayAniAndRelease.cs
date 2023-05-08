using System;
using Framework.Pool;
using Logic.Common;
using UnityEngine;

namespace Logic.Fight.Skill.Implement.Common
{
    /// <summary>
    /// 播放动画 结束后回调 并释放进入池子
    /// </summary>
    public class PlayAniAndRelease : MonoBehaviour, IPoolObj
    {
        private Animator m_Animator;
        private ReleasePoolObjByEvent m_PoolAdapter;
        private static readonly int Play = Animator.StringToHash("Play");

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Animator.enabled = false;
            
            m_PoolAdapter = GetComponent<ReleasePoolObjByEvent>();
        }

        private void OnEnable()
        {
            m_Animator.enabled = true;
            m_Animator.SetTrigger(Play);
        }

        public void OnAni_PlayEnd()
        {
            m_PoolAdapter.OnRelease();
        }

        public void OnSpawn()
        {
            
        }

        public void OnRecycle()
        {
            m_Animator.enabled = false;
        }
    }
}
