using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Common.Effect
{
    public class BreathingEffect : MonoBehaviour
    {
        [Header("呼吸是否忽视暂停")] public bool breathIgnorePause = true;
        [Header("呼吸时间")] public float breathDuration = 1f;
        [Header("呼吸大小")] public float breathScale = 0.8f;
        [Header("呼吸渐变度")] public float breathAlpha = 0.9f;

        private Image m_BreathImage;

        private void Awake()
        {
            m_BreathImage = transform.GetComponent<Image>();
        }

        private void Start()
        {
            // 定义渐变所需的颜色
            var startColor = m_BreathImage.color;
            var endColor = new Color(startColor.r, startColor.g, startColor.b, breathAlpha);

            // 执行缓动动画以及颜色渐变特效
            var sequence = DOTween.Sequence()
                .Append(transform
                    .DOScale(new Vector3(breathScale, breathScale, breathScale), breathDuration)
                    .SetEase(Ease.InOutQuad))
                .Join(m_BreathImage.DOColor(endColor, breathDuration)
                    .SetEase(Ease.InOutQuad))
                .SetLoops(-1, LoopType.Yoyo);

            // 设置动画更新时间为unscaledTime
            sequence.SetUpdate(breathIgnorePause);
        }
    }
}