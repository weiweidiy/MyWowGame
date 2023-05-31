using Cysharp.Threading.Tasks;
using Framework.Extension;
using Framework.Pool;
using UnityEngine;

namespace Logic.UI.UIMain
{
    public class GameObjectSpawnManager : Singleton<GameObjectSpawnManager> 
    {
        public async UniTask<T> Spawn<T>(string pResName) where T:MonoBehaviour , IPoolAssets
        {
            var go = await FightAssetsPool.Ins.Spawn(pResName);
            var component = go.GetComponent<T>();
            if (component == null)
                throw new System.Exception("GO is null! ResName: " + pResName);
            return component;
        }

        public void Recycle<T>(T roomPartner) where T : MonoBehaviour , IPoolAssets
        {
            FightAssetsPool.Ins.Recycle(roomPartner.gameObject);
        }
    }

    /// <summary>
    /// 战斗 怪物 / 召唤物 生成
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class RoomPartnerSpawnManager : Singleton<RoomPartnerSpawnManager>
    {

        public async UniTask<RoomPartner> SpawnEnemy(string pResName)
        {

            var _GO = await FightAssetsPool.Ins.Spawn(pResName);
            var _Enemy = _GO.GetComponent<RoomPartner>();
            if (_Enemy == null)
                throw new System.Exception("Enemy is null! ResName: " + pResName);

            //_GO.ToLeft();

            return _Enemy;
        }

        public void RecycleEnemy(RoomPartner pEnemy)
        {
            FightAssetsPool.Ins.Recycle(pEnemy.gameObject);
        }
    }
}