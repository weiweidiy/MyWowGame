using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Common.Effect
{
    public class BreathingEffect : MonoBehaviour
    {
        [Header("呼吸特效参数")] public float m_Duration = 2f;
        public float m_ScaleAmount = 0.2f;
        public float m_OpacityAmount = 0.8f;

        private Image m_Image;
        private Vector3 m_OriginalScale;
        private Color m_OriginalColor;
        private Coroutine m_BreathingCoroutine;

        private void Awake()
        {
            m_Image = GetComponent<Image>();
            m_OriginalScale = transform.localScale;
            m_OriginalColor = m_Image.color;
        }

        private void OnEnable()
        {
            m_BreathingCoroutine = StartCoroutine(Breathing());
        }

        private void OnDisable()
        {
            if (m_BreathingCoroutine == null) return;
            StopCoroutine(m_BreathingCoroutine);
            m_BreathingCoroutine = null;
        }

        private IEnumerator Breathing()
        {
            while (true)
            {
                yield return StartCoroutine(ScaleUp());
                yield return StartCoroutine(ScaleDown());
            }
        }

        private IEnumerator ScaleUp()
        {
            var elapsedTime = 0f;
            while (elapsedTime < m_Duration / 2f)
            {
                transform.localScale = Vector3.Lerp(m_OriginalScale, m_OriginalScale * (1f + m_ScaleAmount),
                    elapsedTime / (m_Duration / 2f));
                m_Image.color = Color.Lerp(m_OriginalColor,
                    new Color(m_OriginalColor.r, m_OriginalColor.g, m_OriginalColor.b, m_OpacityAmount),
                    elapsedTime / (m_Duration / 2f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ScaleDown()
        {
            var elapsedTime = 0f;
            while (elapsedTime < m_Duration / 2f)
            {
                transform.localScale = Vector3.Lerp(m_OriginalScale * (1f + m_ScaleAmount), m_OriginalScale,
                    elapsedTime / (m_Duration / 2f));
                m_Image.color = Color.Lerp(
                    new Color(m_OriginalColor.r, m_OriginalColor.g, m_OriginalColor.b, m_OpacityAmount),
                    m_OriginalColor,
                    elapsedTime / (m_Duration / 2f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}