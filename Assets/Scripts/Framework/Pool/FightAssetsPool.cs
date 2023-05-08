using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Framework.Extension;
using UnityEngine;
using YooAsset;

namespace Framework.Pool
{
    /*
     * 战斗场景资源GO对象池
     * 通过YoAssets加载资源 并生成GO
     * 主要针对需要频繁创建的GO (eg: 怪物, 特效,飘字 等)
     */
    [DefaultExecutionOrder(-100)]
    public class FightAssetsPool : MonoSingleton<FightAssetsPool>
    {
        //异步操作句柄
        private readonly Dictionary<string, AssetOperationHandle> m_ObjectHandle = new(64);
        //实例化GO
        private readonly Dictionary<string, Queue<GameObject>> m_ObjectPool = new(64);

        /// <summary>
        /// 生产GO
        /// </summary>
        /// <param name="pResPath">资源路径/资源名字</param>
        /// <param name="pParent">生成GO的父节点</param>
        /// <returns>返回生成的GO</returns>
        public async UniTask<GameObject> Spawn(string pResPath, Transform pParent = null)
        {
            if (m_ObjectPool.TryGetValue(pResPath, out var _GOQueue))
            {
                if (_GOQueue.Count <= 0)
                {
                    var _AssetHandle = await CheckHandle(pResPath);
                    var _GoHandle = _AssetHandle.InstantiateAsync(pParent);
                    await _GoHandle;

                    var _GO = _GoHandle.Result;
                    var _GOPool = _GO.GetComponent<IPoolAssets>();
                    _GOPool.PoolObjName = pResPath;
                    _GOPool.OnSpawn();
                    return _GO;
                }
                else
                {
                    var _GO = _GOQueue.Dequeue();
                    if (pParent != null)
                        _GO.SetParent(pParent.gameObject);
                    _GO.SetActive(true);
                    _GO.GetComponent<IPoolAssets>().OnSpawn();
                    return _GO;
                }
            }
            else
            {
                m_ObjectPool.Add(pResPath, new Queue<GameObject>(8));
                var _AssetHandle = await CheckHandle(pResPath);
                var _GoHandle = _AssetHandle.InstantiateAsync(pParent);
                await _GoHandle;

                var _GO = _GoHandle.Result;
                var _GOPool = _GO.GetComponent<IPoolAssets>();
                _GOPool.PoolObjName = pResPath;
                _GOPool.OnSpawn();
                return _GO;
            }
        }

        /// <summary>
        /// 判断异步加载资源句柄是否有效 返回资源句柄
        /// </summary>
        /// <param name="pResPath">资源路径/资源名字</param>
        /// <returns>资源句柄</returns>
        private async UniTask<AssetOperationHandle> CheckHandle(string pResPath)
        {
            if (m_ObjectHandle.TryGetValue(pResPath, out var _AssetHandle))
            {
                if (!_AssetHandle.IsDone)
                    await _AssetHandle.ToUniTask();
            }
            else
            {
                _AssetHandle = YooAssets.LoadAssetAsync<GameObject>(pResPath);
                m_ObjectHandle.Add(pResPath, _AssetHandle);
                await _AssetHandle.ToUniTask();
            }

            return _AssetHandle;
        }

        /// <summary>
        /// 回收Obj
        /// </summary>
        /// <param name="pGO">回收的Obj</param>
        /// <exception cref="Exception"></exception>
        public void Recycle(GameObject pGO)
        {
            var _PoolObj = pGO.GetComponent<IPoolAssets>();
            if (_PoolObj == null)
            {
                throw new Exception("GameObj Not PoolObj : " + pGO.name);
            }
            
            _PoolObj.OnRecycle();
            pGO.SetActive(false);
            pGO.SetParent(gameObject);
            var _ResPath = _PoolObj.PoolObjName;
            if (m_ObjectPool.TryGetValue(_ResPath, out var _GOQueue))
            {
                _GOQueue.Enqueue(pGO);
            }
            else
            {
                var _Q = new Queue<GameObject>(8);
                _Q.Enqueue(pGO);
                m_ObjectPool.Add(_ResPath, _Q);
            }
        }

        /// <summary>
        /// 切换场景清理缓存
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var _AssetHandle in m_ObjectHandle.Values)
            {
                _AssetHandle.Release();
            }
            
            m_ObjectHandle.Clear();
            m_ObjectPool.Clear();
        }

        public void Dump()
        {
            Debug.LogError("======================================================");
            foreach (var _Pool in m_ObjectPool)
            {
                Debug.LogError($"{_Pool.Key} : {_Pool.Value.Count}");
            }
            Debug.LogError("======================================================");
        }
    }
}