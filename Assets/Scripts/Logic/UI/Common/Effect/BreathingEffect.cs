using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Common.Effect
{
    public class BreathingEffect : MonoBehaviour
    {
        [Header("呼吸是否忽视暂停")] public bool breathIgnorePause = true;
        [Header("呼吸时间")] public float breathDuration = 1f;
        [Header("呼吸缩放大小")] public float scaleOffset = 0.1f;
        [Header("呼吸渐变度")] public float breathAlpha = 0.9f;

        public Image m_BreathImage;

        private Vector3 m_OriginalScale;
        private Sequence m_Sequence;

        private void Awake()
        {
            m_OriginalScale = transform.localScale;
        }

        private void OnEnable()
        {
            // 定义渐变所需的颜色
            var startColor = m_BreathImage.color;
            var endColor = new Color(startColor.r, startColor.g, startColor.b, breathAlpha);

            // 执行缓动动画以及颜色渐变特效
            m_Sequence = DOTween.Sequence()
                .Append(transform
                    .DOScale(new Vector3(m_OriginalScale.x + scaleOffset, m_OriginalScale.y + scaleOffset,
                        m_OriginalScale.z + scaleOffset), breathDuration)
                    .SetEase(Ease.InOutQuad))
                .Join(m_BreathImage.DOColor(endColor, breathDuration)
                    .SetEase(Ease.InOutQuad))
                .SetLoops(-1, LoopType.Yoyo);

            // 设置动画更新时间为unscaledTime
            m_Sequence.SetUpdate(breathIgnorePause);
        }

        private void OnDisable()
        {
            m_Sequence.Kill();
            transform.localScale = m_OriginalScale;
        }
    }
}