using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonHpBar : MonoBehaviour
    {
        /// <summary>
        /// 血条填充条
        /// </summary>
        public Image m_FillImage;
        /// <summary>
        /// 血条白色标记条
        /// </summary>
        public Image m_Flag;

        /// <summary>
        /// 填充条的宽
        /// </summary>
        float m_width;

        /// <summary>
        /// 填充条的recttansform
        /// </summary>
        RectTransform m_rt;

        /// <summary>
        /// 延迟协程
        /// </summary>
        Coroutine co;

        private void Awake()
        {
            var rt = m_FillImage.GetComponent<RectTransform>();
            m_rt = m_Flag.GetComponent<RectTransform>();
            m_width = rt.rect.width;
        }

        /// <summary>
        /// 设置进度填充
        /// </summary>
        /// <param name="fillAmount"></param>
        public void SetFill(float fillAmount)
        {
            var preFill = m_FillImage.fillAmount;

            if (fillAmount < preFill)
            {
                m_Flag.transform.gameObject.SetActive(true);
                co = StartCoroutine(WaitForSeconds(0.1f, fillAmount));
                m_rt.sizeDelta = new Vector2(m_width * preFill - m_width * fillAmount, m_rt.sizeDelta.y);
                m_Flag.transform.localPosition = new Vector3(m_width * preFill, m_Flag.transform.localPosition.y, m_Flag.transform.localPosition.z);
            }
            else
            {
                m_Flag.transform.gameObject.SetActive(false);
                m_FillImage.fillAmount = fillAmount;
            }
        }

        private void OnDisable()
        {
            if (co != null)
                StopCoroutine(co);
        }

        /// <summary>
        /// 重置血条
        /// </summary>
        public void ResetFill()
        {
            m_FillImage.fillAmount = 1f;
        }


        IEnumerator WaitForSeconds(float interval, float fillAmount)
        {
            yield return interval;
            m_FillImage.fillAmount = fillAmount;

        }
    }
}