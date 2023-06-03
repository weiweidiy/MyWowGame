using Chronos;
using Cysharp.Threading.Tasks;
using Framework.Extension;
using Framework.Helper;
using Framework.Pool;
using Logic.Fight.Actor;
using Logic.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Common
{
    /// <summary>
    /// 战斗 怪物 / 召唤物 生成
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class FightEnemySpawnManager : MonoSingleton<FightEnemySpawnManager>
    {
        [Header("怪物刷新点")]
        [LabelText("地面刷新点")]
        public Transform[] m_EnemyGroundSpawnPoss;
        [LabelText("空中刷新点")]
        public Transform[] m_EnemySkySpawnPoss;

        private int iEnemyGroundIndex = 0;
        public async UniTask<Enemy> SpawnEnemy(string pResName, Enemy.PositionType posType = Enemy.PositionType.Ground)
        {
            //设定初始位置
            if(iEnemyGroundIndex >= m_EnemyGroundSpawnPoss.Length)
                iEnemyGroundIndex = 0;
            //Vector3 _Pos = m_EnemyGroundSpawnPoss[iEnemyGroundIndex++].position;
            //_Pos.y += RandomHelper.Range(-0.2f, 0.2f);
            var _Enemy = await SpawnEnemy(pResName, iEnemyGroundIndex, posType);
            iEnemyGroundIndex++;
            return _Enemy;

        }

        public async UniTask<Enemy> SpawnEnemy(string pResName, int spawnPositionIndex, Enemy.PositionType posType = Enemy.PositionType.Ground)
        {
            Vector3 _Pos = GetPosition(posType, spawnPositionIndex).position;
            _Pos.y += RandomHelper.Range(-0.2f, 0.2f);

            var _GO = await FightAssetsPool.Ins.Spawn(pResName);
            var _Enemy = _GO.GetComponent<Enemy>();
            if (_Enemy == null)
                throw new System.Exception("Enemy is null! ResName: " + pResName);

            _Enemy.transform.position = _Pos;
            //_GO.ToLeft();

            return _Enemy;
        }

        /// <summary>
        /// boss是指定位置刷新
        /// </summary>
        /// <param name="pResName"></param>
        /// <returns></returns>
        public async UniTask<Enemy> SpawnEnemyBoss(string pResName, Enemy.PositionType posType = Enemy.PositionType.Ground)
        {
            var _Enemy = await SpawnEnemy(pResName, GetPosition(posType).Length -1, posType);
            return _Enemy;
        }


        Transform GetPosition(Enemy.PositionType posType, int spawnPositionIndex)
        {
            return GetPosition(posType)[spawnPositionIndex];
        }

        Transform[] GetPosition(Enemy.PositionType posType)
        {
            Transform[] transforms = null;
            switch (posType)
            {
                case Enemy.PositionType.Ground:
                    transforms = m_EnemyGroundSpawnPoss;
                    break;
                case Enemy.PositionType.Sky:
                    transforms = m_EnemySkySpawnPoss;
                    break;
                default:
                    Debug.LogError("未来实现的怪物类型 " + posType);
                    break;
            }

            return transforms;
        }


        public void RecycleEnemy(Enemy pEnemy)
        {
            FightAssetsPool.Ins.Recycle(pEnemy.gameObject);
        }
    }
}