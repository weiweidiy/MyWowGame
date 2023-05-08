using System.Collections.Generic;
using Framework.Extension;
using UnityEngine;

namespace Framework.Pool
{
    /// <summary>
    /// 通过 直接Instantiate挂载在Prefab上的其他Prefab引用 来产生的GO对象 使用这个对象池来管理.
    /// 一般用在通用性不是太高 比较确定的场景中.
    /// eg: 某个具体的技能的特效 子弹
    ///     某个具体武器的子弹 特效 等
    ///
    /// ! 如果是通过YooAssets动态加载的 请使用 FightAssetsPool 对象池
    /// 
    /// </summary>
    public class FightObjPool : MonoSingleton<FightObjPool>
    {
        private readonly Dictionary<string, Transform> m_ObjRoot = new(12);
        private readonly Dictionary<string, Queue<GameObject>> m_ObjPool = new(12);
        
        //生产
        public GameObject Spawn(GameObject pPrefab)
        {
            var _Name = pPrefab.name;
            
            if (!m_ObjPool.TryGetValue(_Name, out var _Pool))
            {
                _Pool = new Queue<GameObject>(4);
                m_ObjPool.Add(_Name, _Pool);
                
                var _Obj = new GameObject(_Name + "_Pool");
                _Obj.SetParent(gameObject);
                var _RootObj = _Obj.transform;
                m_ObjRoot.Add(_Name, _RootObj);
            }

            if (_Pool.Count <= 0)
            {
                var _Obj = Instantiate(pPrefab, m_ObjRoot[_Name]);
                _Obj.name = _Name;
                _Obj.GetComponent<IPoolObj>()?.OnSpawn();
                return _Obj;
            }

            {
                var _Obj = _Pool.Dequeue();
                _Obj.SetActive(true);
                _Obj.GetComponent<IPoolObj>()?.OnSpawn();
                return _Obj;
            }
        }

        //回收
        public void Recycle(GameObject pGO)
        {
            var _Parent = m_ObjRoot[pGO.name]; 
            #if UNITY_EDITOR
            if (_Parent == null)
            {
                Debug.LogError("Recycle GO ERROR : " + pGO.name);
                Destroy(pGO);
                return;
            }
            #endif
            
            pGO.GetComponent<IPoolObj>()?.OnRecycle();
            pGO.SetActive(false);
            pGO.transform.SetParent(_Parent);
            
            m_ObjPool[pGO.name].Enqueue(pGO);
        }
    }
}