using System.Collections;
using DG.Tweening;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using TMPro;
using UnityEngine;

namespace Logic.UI.Common
{
    /// <summary>
    /// 通用Tips提示
    /// </summary>
    public class UICommonTips : UIPage
    {
        public GameObject m_TipObj;
        public TextMeshProUGUI m_Text;
        
        private readonly WaitForSeconds wfs = new (0.7f);
        private Coroutine coro;
        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.ShowTips, OnShowTips);
        }

        private Tweener _CurTween;
        private void OnShowTips(int pArg1, object pTipContext)
        {
            m_Text.text = pTipContext as string;
            if (_CurTween != null)
            {
                _CurTween.Kill();
            }

            if (coro != null)
            {
                StopCoroutine(coro);
                coro = null;
            }

            m_TipObj.transform.LocalScale(0.2f);
            m_TipObj.Show();
            _CurTween = m_TipObj.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(OnEnd);
        }

        void OnEnd()
        {
            coro = StartCoroutine(DoEnd());
        }
        IEnumerator DoEnd()
        {
            yield return wfs;
            m_TipObj.Hide();
            m_TipObj.transform.LocalScale(0.2f);
            coro = null;
        }
    }
}

