using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Common
{
    /// <summary>
    /// ScrollView导航
    /// </summary>
    public class ScrollViewNavigation : MonoBehaviour
    {
        public ScrollRect scrollRect;
        public RectTransform viewport;
        public RectTransform content;
        
        void Awake()
        {
            Init();
        }
        
        /// <summary>
        /// 如果没主动挂上 这里取一下
        /// </summary>
        private void Init()
        {
            if (scrollRect == null)
            {
                scrollRect = this.GetComponent<ScrollRect>();
            }

            if (viewport == null)
            {
                viewport = this.transform.Find("Viewport").GetComponent<RectTransform>();
            }

            if (content == null)
            {
                content = this.transform.Find("Viewport/Content").GetComponent<RectTransform>();
            }
        }

        public void Navigate(RectTransform item, bool pNeedTween = false)
        {
            Vector3 itemCurrentLocalPostion = scrollRect.GetComponent<RectTransform>()
                .InverseTransformVector(ConvertLocalPosToWorldPos(item));
            Vector3 itemTargetLocalPos = scrollRect.GetComponent<RectTransform>()
                .InverseTransformVector(ConvertLocalPosToWorldPos(viewport));

            Vector3 diff = itemTargetLocalPos - itemCurrentLocalPostion;
            diff.z = 0.0f;

            var newNormalizedPosition = new Vector2(
                diff.x / (content.GetComponent<RectTransform>().rect.width - viewport.rect.width),
                diff.y / (content.GetComponent<RectTransform>().rect.height - viewport.rect.height)
            );

            newNormalizedPosition = scrollRect.GetComponent<ScrollRect>().normalizedPosition - newNormalizedPosition;

            newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
            newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

            if(pNeedTween)
                DOTween.To(() => scrollRect.GetComponent<ScrollRect>().normalizedPosition, x => scrollRect.GetComponent<ScrollRect>().normalizedPosition = x, newNormalizedPosition, 0.8f);
            else
                scrollRect.GetComponent<ScrollRect>().normalizedPosition = newNormalizedPosition;
        }

        private Vector3 ConvertLocalPosToWorldPos(RectTransform target)
        {
            var pivotOffset = new Vector3(
                (0.5f - target.pivot.x) * target.rect.size.x,
                (0.5f - target.pivot.y) * target.rect.size.y,
                0f);

            var localPosition = target.localPosition + pivotOffset;

            return target.parent.TransformPoint(localPosition);
        }
    }
}
