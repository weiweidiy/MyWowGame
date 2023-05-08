using BreakInfinity;
using DG.Tweening;
using Framework.Pool;
using Logic.Data;
using UnityEngine;

namespace Logic.UI.Common.Effect
{
    public class UICurrencyCollect : MonoBehaviour, IPoolObj
    {
        private Transform targetPos;
        private Vector3 _startPos1;
        private Vector3 _endPos;
        private Vector3[] _Paths = new Vector3[2];

        public void InitAnimation(Transform pTargetPos, float pTime, BigDouble pDropCoin)
        {
            _startPos1 = transform.position;
            _endPos = pTargetPos.position;

            transform.SetAsLastSibling();
            transform.parent.transform.SetAsLastSibling();

            var random_x = Random.Range(-0.5f, 0.5f);
            var random_y = Random.Range(-0.1f, 0.7f);
            
            _Paths[0] = _startPos1 + new Vector3(random_x, random_y, 0);
            _Paths[1] = _endPos;
            
            transform.DOPath(_Paths, pTime, PathType.CatmullRom).SetEase(Ease.InCubic).OnComplete(() =>
            {
                FightObjPool.Ins.Recycle(gameObject);
                // TODO:需要进行特殊的表现处理，目前是先表现再存储
                GameDataManager.Ins.Coin += pDropCoin;
            });
        }
        
        public void OnSpawn()
        {
            
        }

        public void OnRecycle()
        {
            
        }
    }
}
