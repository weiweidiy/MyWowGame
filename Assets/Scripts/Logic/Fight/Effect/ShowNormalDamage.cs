using BreakInfinity;
using DG.Tweening;
using Framework.Pool;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Logic.Fight.Effect
{
    public class ShowNormalDamage : MonoBehaviour, IPoolAssets
    {
        public TextMeshPro m_Text;

        private Color _Color;
        public void ShowText(BigDouble pDamage, Color pColor, bool pIsEnemy)
        {
            _Color = pColor;
            m_Text.color = pColor;
            
            m_Text.SetText(pDamage.ToUIString());

            var _T = transform.DOBlendableLocalMoveBy(new Vector3((pIsEnemy ? 0.3f : -0.3f), 0.3f, 0), 0.8f).SetUpdate(UpdateType.Manual).SetEase(Ease.OutExpo);
            m_Text.DOFade(0, 0.8f).SetUpdate(UpdateType.Manual).SetEase(Ease.InQuint);
            _T.OnComplete(OnTweenComplete);
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
            m_Text.color = _Color;
        }
    }
}