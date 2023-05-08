using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Logic.UI.Common.Effect
{
    public class ArrowUpgradeEffect : MonoBehaviour
    {
        [Header("升级特效参数")] [SerializeField] private float m_Duration = 1f;
        [SerializeField] private float m_ScaleMultiplier = 1.1f;

        private Vector3 m_OriginalScale;

        private Coroutine m_ArrowUpgradeCoroutine;
        private WaitForSeconds m_Delay;

        private void Awake()
        {
            m_OriginalScale = transform.localScale;
            m_Delay = new WaitForSeconds(3f * m_Duration / 2f);
        }

        private void OnEnable()
        {
            m_ArrowUpgradeCoroutine = StartCoroutine(ArrowUpgrade());
        }

        private void OnDestroy()
        {
            if (m_ArrowUpgradeCoroutine == null) return;
            StopCoroutine(m_ArrowUpgradeCoroutine);
            m_ArrowUpgradeCoroutine = null;
        }

        private IEnumerator ArrowUpgrade()
        {
            while (true)
            {
                UpgradeEffect();
                yield return m_Delay;
            }
        }

        private void UpgradeEffect()
        {
            transform.DOScaleY(m_OriginalScale.y / 2f, m_Duration / 2f)
                .SetEase(Ease.Linear)
                .OnComplete
                (
                    () => transform.DOScaleY(m_OriginalScale.y * m_ScaleMultiplier, m_Duration / 2f)
                        .SetEase(Ease.OutBack)
                        .OnComplete
                        (
                            () => transform.DOScale(m_OriginalScale, m_Duration / 2f)
                                .SetEase(Ease.OutBounce)));
        }
    }
}