using BreakInfinity;
using DG.Tweening;
using Framework.Extension;
using Framework.Pool;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Logic.Fight.Effect
{
    /// <summary>
    /// 暴击伤害显示
    /// </summary>
    public class ShowCriticalDamage : MonoBehaviour, IPoolAssets
    {
        public TextMeshPro m_Text;

        private static Vector3 _S = new Vector3(1.2f, 1.2f, 1);
        public void ShowText(BigDouble pDamage)
        {
            m_Text.SetText(pDamage.ToUIString());
            
            Sequence _Seq = DOTween.Sequence();
            _Seq.Append(transform.DOScale(new Vector3(0.6f, 0.6f, 1), 0.1f));
            _Seq.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1), 0.1f));
            _Seq.Append(transform.DOScale(new Vector3(1f, 1f, 1), 0.1f));
            _Seq.Append(transform.DOBlendableLocalMoveBy(new Vector3(0, .5f, 0), 0.5f).SetEase(Ease.OutExpo));
            _Seq.Append(m_Text.DOFade(0, 0.1f).SetEase(Ease.InQuint));
            _Seq.OnComplete(OnTweenComplete);
        }

        private void OnTweenComplete()
        {
            FightAssetsPool.Ins.Recycle(gameObject);
        }

        public string PoolObjName { get => m_PoolObjName; set => m_PoolObjName = value; }
        [ReadOnly]
        public string m_PoolObjName;
        public void OnSpawn()
        {
            
        }

        public void OnRecycle()
        {
            m_Text.SetText("");
            m_Text.color = Color.white;
            transform.LocalScale(_S);
        }
    }
}