using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Extension
{
    /// <summary>
    /// 对Button的扩展，代码中处理动画效果
    /// </summary>
    public class ButtonAni : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Vector2 Scale = new Vector2(0.95f, 0.95f);
        public float Interval = 0.15f;

        private RectTransform m_RectTrans;
        private Vector2 m_OriSize;
        private Vector2 m_ScaleTo;

        private void Awake()
        {
            m_RectTrans = GetComponent<RectTransform>();
            m_OriSize = m_RectTrans.localScale;
            m_ScaleTo = m_OriSize * Scale;
        }

        private void OnDisable()
        {
            m_RectTrans.localScale = m_OriSize;
            StopAllCoroutines();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(OnDown(Interval));
        }

        private IEnumerator OnDown(float interval)
        {
            float timer = 0;
            while (timer <= interval)
            {
                if (interval == 0) break;
                m_RectTrans.localScale = Vector2.Lerp(m_OriSize, m_ScaleTo, timer / interval);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            m_RectTrans.localScale = m_ScaleTo;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(OnUp(Interval));
        }

        private IEnumerator OnUp(float interval)
        {
            float timer = 0;
            while (timer <= interval)
            {
                if (interval == 0) break;
                var t = timer / interval;
                m_RectTrans.localScale = new Vector2(
                    OutBack(t, m_ScaleTo.x, m_OriSize.x),
                    OutBack(t, m_ScaleTo.y, m_OriSize.y)
                );
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            m_RectTrans.localScale = m_OriSize;
        }

        // outback 缓动曲线的实现
        public static float OutBack(float t, float startValue, float endValue, float overshoot = 10)
        {
            t -= 1f;
            float postFix = t;
            return startValue + (endValue - startValue) * (postFix * t * ((overshoot + 1f) * t + overshoot) + 1f);
        }
    }
}