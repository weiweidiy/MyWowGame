using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Logic.Common
{
    /// <summary>
    /// UI 序列帧图片播放
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SpriteFrameAni : MonoBehaviour
    {
        public Image m_Renderer;
        
        //帧数
        public float m_PreSecFrame;
        //是否循环
        public bool m_IsLoop;
        //序列中图片
        public Sprite[] m_Sprites;

        private IEnumerator _Coroutine;
        private WaitForSeconds _WFS;
        
        [LabelText("播放结束事件")]
        public UnityEvent OnPlayEnd;

        private void Awake()
        {
            _WFS = new WaitForSeconds(m_PreSecFrame);
        }

        private void OnEnable()
        {
            _Coroutine = Play();
            StartCoroutine(_Coroutine);
        }

        private void OnDisable()
        {
            StopCoroutine(_Coroutine);
        }
        
        private IEnumerator Play()
        {
            for (int j = 0; j < m_Sprites.Length; j++)
            {
                m_Renderer.sprite = m_Sprites[j];
                yield return _WFS;
            }
            
            OnPlayEnd?.Invoke();
                
            while (m_IsLoop)
            {
                for (int j = 0; j < m_Sprites.Length; j++)
                {
                    m_Renderer.sprite = m_Sprites[j];
                    yield return _WFS;
                }
                
                OnPlayEnd?.Invoke();
            }
        }
    }
}
