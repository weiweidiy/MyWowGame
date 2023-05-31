using BreakInfinity;
using Framework.Helper;
using Framework.Pool;
using UnityEngine;

namespace Logic.UI.Common.Effect
{
    /// <summary>
    /// 金币收集动画
    /// </summary>
    public class CoinEffectMgr : MonoBehaviour
    {
        [Header("金币预制体")] public GameObject m_CoinPrefab;
        [Header("最终目的地")] public Transform m_TargetPos;
        [Header("金币飞行速度")] public float m_FlySpeed = 1;

        public static CoinEffectMgr Ins;

        private Camera MainCamera;
        private Camera UICamera;

        private void Awake()
        {
            Ins = this;
        }

        private void Start()
        {
            MainCamera = Camera.main;
            UICamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
        }

        public void StartEffect(Vector3 pStartPos, BigDouble pDropCoin, int pMaxNum = 6)
        {
            var _Pos = MainCamera.WorldToScreenPoint(pStartPos);
            var _LocalPos = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, _Pos, UICamera,
                out _LocalPos);

            var _Num = RandomHelper.Range(pMaxNum - 3, +pMaxNum + 3);
            var _Coin = pDropCoin / _Num;

            for (int i = 0; i < _Num; i++)
            {
                GameObject
                    go = FightObjPool.Ins
                        .Spawn(m_CoinPrefab); //Instantiate(m_CoinPrefab, Vector3.zero, Quaternion.identity);

                go.transform.SetParent(transform);
                go.transform.localPosition = _LocalPos;
                go.transform.localScale = Vector3.one;

                UICurrencyCollect cc = go.GetComponent<UICurrencyCollect>();
                if (cc != null)
                {
                    cc.InitAnimation(m_TargetPos, m_FlySpeed, _Coin);
                }
            }
        }
    }
}