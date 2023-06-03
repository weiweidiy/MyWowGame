using DG.Tweening;
using UnityEngine;

namespace Logic.UI.Common.Effect
{
    public class ArrowUpgradeEffect : MonoBehaviour
    {
        [Header("是否忽视暂停")] public bool upgradeIgnorePause = true;
        [Header("动画时间")] public float upgradeDuration = 0.5f;
        [Header("偏移距离")] public float offsetPos = 10f;

        private Vector3 m_OriginalPos;
        private Sequence upLevelSequence;

        private void Awake()
        {
            m_OriginalPos = transform.localPosition;
        }

        private void OnEnable()
        {
            upLevelSequence = DOTween.Sequence();
            upLevelSequence
                .Append(transform.DOLocalMoveY(m_OriginalPos.y + offsetPos, upgradeDuration))
                .SetEase(Ease.OutSine)
                .SetLoops(-1, LoopType.Yoyo);

            // 设置动画更新时间为unscaledTime
            upLevelSequence.SetUpdate(upgradeIgnorePause);
        }

        private void OnDisable()
        {
            upLevelSequence.Kill();
            transform.localPosition = m_OriginalPos;
        }
    }
}